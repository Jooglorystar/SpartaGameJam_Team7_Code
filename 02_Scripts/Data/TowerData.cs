using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [Header("Ÿ���� �⺻ ����")]
    [SerializeField] int _id = 1;
    [SerializeField] string _towerName = "�⺻�̸�";
    [SerializeField] Sprite _towerPortrait;
    [SerializeField] int _towerLevel = 1;
    [SerializeField] float _towerRange = 3f;
    [SerializeField] int _towerDamage = 1;
    [SerializeField] float _towerAttackSpeed = 1;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] int _towerSellCost = 15;
    [SerializeField, Multiline] string _towerExplain; 

    [Header("Ÿ���� visual ����")]
    [SerializeField] TowerObjectController _towerObject;
    [SerializeField] Vector3 _offset;
    [SerializeField] Vector3 _offSetScale = Vector3.one;

    [Header("�ɷ�")]
    [SerializeField] string _spawnSound;
    [SerializeField] string _hitSound;
    [SerializeField] string _attackSound;
    [SerializeField] string _normalVfxName;
    [SerializeField] string _splashVfxName;
    [SerializeField] Projectile _projectile;
    [SerializeField] int _multiCount = 1;
    [SerializeField] float _projectileSpeed = 10f;
    [SerializeField] bool _isSplash = false;
    [SerializeField] float _splashRadius = 3f;
    [SerializeField] int _maxSplashCount = 3;
    [SerializeField] float _splashDamageReductionRate = 0f;
    [SerializeReference, SubclassSelector] EffectAction _effect;

    [Header("����")]
    [SerializeField] TowerData _upgradableTower; // ���׷��̵��� �� �ִ� ���� Ÿ�� �ε���
    [SerializeField] TowerData[] _ingredientTowers; // ��ᰡ �Ǵ� Ÿ�� �ε���

    public int Id { get => _id; }
    public string TowerName { get => _towerName; }
    public Sprite TowerPortrait { get => _towerPortrait; }
    public int TowerLevel { get => _towerLevel; }
    public float TowerRange { get => _towerRange; }
    public int TowerDamage { get => _towerDamage; }
    public float TowerAttackSpeed { get => _towerAttackSpeed; }
    public LayerMask TargetLayer { get => _targetLayer; }
    public int TowerSellCost { get => _towerSellCost; }
    public string TowerExplain { get => _towerExplain; }
    public TowerObjectController TowerObject { get => _towerObject; }
    public string SpawnSound { get => _spawnSound; }
    public string HitSound { get => _hitSound; }
    public string AttackSound { get => _attackSound; }
    public string SplashVfxName { get => _splashVfxName; }
    public string NormalVfxName { get => _normalVfxName; }
    public Projectile Projectile { get => _projectile; }
    public int MultiCount { get => _multiCount; }
    public float ProjectileSpeed { get => _projectileSpeed; }
    public bool IsSplash { get => _isSplash; }
    public float SplashRadius { get => _splashRadius; }
    public int MaxSplashCount { get => _maxSplashCount; }
    public float SplashDamageReductionRate { get => _splashDamageReductionRate; }
    public EffectAction Effect { get => _effect; }
    public Vector3 Offset { get => _offset; }
    public Vector3 OffSetScale { get => _offSetScale; }
    public TowerData UpgradableTower { get => _upgradableTower; }
    public TowerData[] IngredientTowers { get => _ingredientTowers; }
}
