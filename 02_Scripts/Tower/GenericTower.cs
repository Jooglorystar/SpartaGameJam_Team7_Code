using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GenericTower : MonoBehaviour
{
    [SerializeField] Transform _prefabOffset;
    [SerializeField] float _rotationSpeed = 10f;

    TowerSpawner _towerSpawner;
    public TowerData TowerData { get; private set; }
    CancellationTokenSource _cancellationTokenSource;

    TowerObjectController objectController;

    Enemy _targetObject = null;
    List<Enemy> _extraTargetTargets = new List<Enemy>();

    public event UnityAction onDestroy = null;

    public void Inject(TowerSpawner towerSpawner, TowerData towerData)
    {
        _towerSpawner = towerSpawner;
        TowerData = towerData;
    }


    public void UpdateTower()
    {
        _prefabOffset.localPosition = TowerData.Offset;
        _prefabOffset.localScale = TowerData.OffSetScale;
        _prefabOffset.DestroyAllChildren();

        objectController = Instantiate(TowerData.TowerObject, _prefabOffset);
        objectController.Inject(this);

        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
        }

        _cancellationTokenSource = new CancellationTokenSource();

        _ = StartTowerAI(_cancellationTokenSource.Token);

        if(!string.IsNullOrEmpty(TowerData.SpawnSound)) // 소환소리
        {
            SoundManager.Instance.PlaySFX(TowerData.SpawnSound);
        }
    }

    public void DestroyTower()
    {
        Destroy(gameObject, 0.1f);
    }

    void Update()
    {
        if(_targetObject != null)
        {
            Vector3 direction = _targetObject.transform.position - transform.position;
            direction.y = 0; // Y축 회전 제한

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }

    void OnDestroy()
    {
        onDestroy?.Invoke();
        _cancellationTokenSource?.Cancel();

        onDestroy = null;
    }

    async Awaitable StartTowerAI(CancellationToken token)
    {
        while(true)
        {
            if(token.IsCancellationRequested)
            {
                return;
            }

            TryAct();

            await Awaitable.WaitForSecondsAsync(TowerData.TowerAttackSpeed);
        }
    }

    void TryAct()
    {
        if(_targetObject != null) // 적이 있지만
        {
            if (Vector3.Distance(_targetObject.transform.position, transform.position) > TowerData.TowerRange || _targetObject.IsDead) // 멀어지거나 죽어있는 적이라면
            {
                _targetObject = null;
            }
        }

        if(_targetObject == null)
        {
            _targetObject = GetClosestTarget();
        }

        if(_targetObject != null)
        {
            objectController.SetTrigger("Attack");

            if(!string.IsNullOrEmpty(TowerData.AttackSound)) // 공격 소리
            {
                SoundManager.Instance.PlaySFX(TowerData.AttackSound);
            }
        }

        Act(_targetObject);

        if (TowerData.MultiCount > 1 && _targetObject != null)
        {
            DetectExtraTarget(_targetObject);
            foreach(var target in _extraTargetTargets)
            {
                Act(target);
            }
        }
    }

    Enemy GetClosestTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, TowerData.TowerRange, TowerData.TargetLayer);

        Enemy closestObject = null;
        float minDistance = float.MaxValue;

        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out Enemy target) && !target.IsDead)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObject = target;
                }
            }
        }

        return closestObject;
    }

    void DetectExtraTarget(Enemy targetOrigin)
    {
        _extraTargetTargets = new List<Enemy>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, TowerData.TowerRange, TowerData.TargetLayer);

        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out Enemy target) && !target.IsDead)
            {
                if(targetOrigin != target && _extraTargetTargets.Count < TowerData.MultiCount - 1)
                {
                    _extraTargetTargets.Add(target);
                }
            }
        }
    }

    void Act(Enemy target)
    {
        if(target == null)
        {
            Debug.Log("타겟이 없습니다.");
        }
        else
        {
            Debug.Log("공격합니다.");

            Projectile projectile = Instantiate(TowerData.Projectile);
            projectile.transform.position = objectController.shootPoint.position;
            projectile.ShotProjectile(TowerData, target, OnHitTarget);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _towerSpawner.SetRadiusCircle(this);
    }

    void OnHitTarget(Enemy target, bool isSplash)
    {
        if (target == null)
            return;

        TowerData.Effect?.Attach(target);

        OnDamage(target);

        if(!string.IsNullOrEmpty(TowerData.HitSound)) // 타격 사운드
        {
            SoundManager.Instance.PlaySFX(TowerData.HitSound);
        }

        // 일반 타격 vfx
        if (!string.IsNullOrEmpty(TowerData.NormalVfxName) && !TowerData.IsSplash)
        {
            ParticleSystem normalVfx = VFXManager.Instance.GetPool(TowerData.NormalVfxName);
            normalVfx.transform.position = target.PrefabOffset.position;
        }

        if (isSplash)
        {
            if (!string.IsNullOrEmpty(TowerData.SplashVfxName))
            {
                // splashVfx
                ParticleSystem splashVfx = VFXManager.Instance.GetPool(TowerData.SplashVfxName);
                splashVfx.transform.position = target.PrefabOffset.position;
                splashVfx.transform.localScale = Vector3.one * TowerData.SplashRadius;
            }

            List<Enemy> enemies = ProcessSplash(target);
            foreach(Enemy enemy in enemies)
            {
                OnHitTarget(enemy, false);
            }
        }
    }

    List<Enemy> ProcessSplash(Enemy originTarget)
    {
        List<Enemy> result = new List<Enemy>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, TowerData.SplashRadius, TowerData.TargetLayer);

        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out Enemy target) && !target.IsDead)
            {
                if(target != originTarget && result.Count < TowerData.MaxSplashCount)
                {
                    result.Add(target);
                }
            }
        }

        return result;
    }

    void OnDamage(Enemy target)
    {
        target.CurrnetHp -= TowerData.TowerDamage; 
    }
}
