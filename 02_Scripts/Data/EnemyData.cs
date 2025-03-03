using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("���� �⺻ ����")]
    [SerializeField] int _id = 1;
    [SerializeField] string _enemyName = "�⺻�̸�";
    [SerializeField] int _hp = 10;
    [SerializeField] int _damage = 1;
    [SerializeField] float _speed = 3f;
    [SerializeField] bool _isBoss = false;

    [Header("���� visual ����")]
    [SerializeField] GameObject _enemyObject;
    [SerializeField] Vector3 _offset;
    public int Id { get => _id; }
    public string EnemyName { get => _enemyName; }
    public int Hp { get => _hp; }
    public int Damage { get => _damage; }
    public float Speed { get => _speed; }
    public bool IsBoss { get => _isBoss; }
    public GameObject EnemyObject { get => _enemyObject; }
    public Vector3 Offset { get => _offset; }
}
