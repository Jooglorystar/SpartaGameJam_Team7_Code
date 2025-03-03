using System.Collections;
using UnityEngine;

public class MainScene : BaseScene
{
    [field: SerializeField] PlayerManager PlayerManager { get; set; }
    [field: SerializeField] PoolSystem EnemyPoolSystem { get; set; }

    protected override void Init()
    {
        SoundManager.Instance.PlayBGM("in game bgm");

        PlayerManager.SetPlayerHealth();
        EnemyPoolSystem.CreatePool();
    }
}
