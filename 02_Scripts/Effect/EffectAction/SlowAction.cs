using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class SlowAction : EffectAction
{
    float _duration = 3f;
    float _damp = 0.5f;
    Enemy _target;

    public override void Attach(Enemy target)
    {
        _target = target;
        target.EnemyEffectController.SetCC(ECC.Slow, _duration);

        target.EnemyEffectController.onEffectEnd[Utills.GetFlagIndex(ECC.Slow)] = OnEndEffect;

        _target.Movement.EnemySpeed = _target.EnemyData.Speed * _damp;
    }

    void OnEndEffect()
    {
        _target.Movement.EnemySpeed = _target.EnemyData.Speed;
    }
}
