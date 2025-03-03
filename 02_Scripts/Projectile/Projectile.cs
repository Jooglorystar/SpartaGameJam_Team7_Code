using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _rotateXSpeed = 360f;

    TowerData _data;
    Enemy _target;
    UnityAction<Enemy, bool> _onHitAction = null;
    CancellationTokenSource _cancellationTokenSource = null;

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

    public void ShotProjectile(TowerData data, Enemy target, UnityAction<Enemy, bool> onHitAction = null)
    {
        _data = data;
        _target = target;
        _onHitAction = onHitAction;

        _cancellationTokenSource = new CancellationTokenSource();
        _ = MoveToTarget(_cancellationTokenSource.Token);
    }

    async Awaitable MoveToTarget(CancellationToken token)
    {
        if (_target == null) // 타겟이 없으면 바로 제거
        {
            Destroy(gameObject);
            return;
        }

        while (_target != null && Vector3.Distance(transform.position, _target.PrefabOffset.position) > 0.1f)
        {
            if (token.IsCancellationRequested) break; // 취소되었으면 종료

            // 타겟 방향으로 이동
            transform.position = Vector3.MoveTowards(transform.position, _target.PrefabOffset.position, _data.ProjectileSpeed * Time.deltaTime);
            transform.LookAt(_target.transform); // 타겟을 향하도록 회전

            // X축으로 회전
            transform.Rotate(Vector3.right * _rotateXSpeed * Time.deltaTime);

            await Awaitable.NextFrameAsync(); // 다음 프레임까지 대기
        }

        OnHitTarget();
    }

    void OnHitTarget()
    {
        _onHitAction?.Invoke(_target, _data.IsSplash);

        Destroy(gameObject); // 투사체 제거
    }

    void OnDestroy()
    {
        _cancellationTokenSource?.Cancel(); // 객체 제거 전에 토큰 취소
    }
}
