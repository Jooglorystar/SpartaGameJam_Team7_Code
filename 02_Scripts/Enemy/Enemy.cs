using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, IPointerDownHandler
{
    private int _waypointNum;

    public EnemyMovement Movement { get; private set; }

    [SerializeField] private Animator _animator;

    [SerializeField] private float waypointRange = 0.3f;

    [field: SerializeField] public Transform PrefabOffset { get; private set; }
    [field: SerializeField] public EnemyEffectController EnemyEffectController { get; private set; }

    [SerializeField] int _currentHp;

    [SerializeField] int _maxHp;

    public int CurrnetHp
    {
        get { return _currentHp; }
        set
        {
            int prev = _currentHp;
            _currentHp = Mathf.Clamp(value, 0, _maxHp);

            if (prev == _maxHp)
                onChangedHpFirst?.Invoke(prev, _currentHp);

            onChangedHp?.Invoke(prev, _currentHp);

            if (this.EnemyData.IsBoss)
            {
                GameManager.Instance.EnemySpawner.bossHPBar.RefreshBossHPbar(this);
            }

            if (_currentHp <= 0 && !IsDead)
            {
                Dead();
            }

        }
    }

    public bool IsDead { get; private set; }
    public EnemyData EnemyData { get; private set; }
    public event UnityAction<int, int> onChangedHpFirst;
    public event UnityAction<int, int> onChangedHp;
    public event UnityAction onDied;
    public event UnityAction onRelease;
    EnemySpawner _enemySpawner;

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();
    }

    private void OnEnable()
    {
        _waypointNum = 0;
        Movement.TurnAround();
    }


    private void FixedUpdate()
    {
        CheckWaypoint();
        CheckEndTile();
    }

    private void CheckEndTile()
    {
        if (TileManager.Instance.grid.WorldToCell(transform.position) == TileManager.Instance.grid.WorldToCell(TileManager.Instance.endPoint.position))
        {
            GameManager.Instance.EnemySpawner.CurWave.curCount--;
            if (EnemyData.IsBoss)
            {
                GameManager.Instance.EnemySpawner.bossHPBar.gameObject.SetActive(false);
            }
            PlayerManager.Instance.HitPlayer(this);
            if (GameManager.Instance.EnemySpawner.CurWave.curCount <= 0 && PlayerManager.Instance.playerHealthCur > 0)
            {
                GameManager.Instance.EnemySpawner.EndWave();
            }
            this.gameObject.SetActive(false);
        }
    }

    private void CheckWaypoint()
    {
        if (_waypointNum >= WaypointManager.Instance.wayPoints.Length) return;

        Transform waypoint = WaypointManager.Instance.wayPoints[_waypointNum].transform;
        float distance = Vector3.Distance(transform.position, waypoint.position);

        if (distance < waypointRange)
        {
            Movement.TurnAround(WaypointManager.Instance.wayPoints[_waypointNum]);
            _waypointNum++;
        }
    }

    public void Inject(EnemySpawner enemySpawner, EnemyData enemyData)
    {
        _enemySpawner = enemySpawner;
        EnemyData = enemyData;
    }

    public void Spawn()
    {
        if (!EnemyData.IsBoss)
        {
            onChangedHpFirst += (prev, cur) => FindFirstObjectByType<HpBarController>().GetHpBar(this, prev, cur);

        }

        EnemyEffectController.Init(this);

        IsDead = false;
        _maxHp = EnemyData.Hp + (GameManager.Instance.EnemySpawner.stageBuff * GameManager.Instance.EnemySpawner.curWaveIdx);
        CurrnetHp = _maxHp;

        Movement.EnemySpeed = EnemyData.Speed;

        PrefabOffset.localPosition = EnemyData.Offset;
        PrefabOffset.DestroyAllChildren();
        GameObject instance = Instantiate(EnemyData.EnemyObject, PrefabOffset);
        _animator = GetComponentInChildren<Animator>();
        _animator.SetTrigger("Reset");
        _animator.SetInteger("State", 15);
        if (EnemyData.IsBoss)
        {
            GameManager.Instance.EnemySpawner.bossHPBar.ActivateBossHPBar(this);
        }
    }

    void Dead()
    {
        onDied?.Invoke();

        IsDead = true;
        Movement.EnemySpeed = 0f;
        _animator.SetTrigger("Reset");
        _animator.SetInteger("State", 6);
        StartCoroutine(DeadCoroutine());

        GameManager.Instance.EnemySpawner.CurWave.curCount--;

        if (EnemyData.IsBoss)
        {
            GameManager.Instance.EnemySpawner.bossHPBar.gameObject.SetActive(false);
        }

        if (GameManager.Instance.EnemySpawner.CurWave.curCount <= 0)
        {
            GameManager.Instance.EnemySpawner.EndWave();
        }
    }

    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    private void OnDisable()
    {
        onRelease?.Invoke();
    }
}
