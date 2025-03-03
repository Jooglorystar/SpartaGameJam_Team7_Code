using UnityEngine;

public class GameManager : SingletonWithoutDonDestroy<GameManager>
{
    [field: SerializeField] public EnemySpawner EnemySpawner { get; private set; }
}
