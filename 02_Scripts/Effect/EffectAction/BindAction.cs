using UnityEngine;

[System.Serializable]
public class BindAction : EffectAction
{
    float _duration = 3f;
    float _damp = 0f;
    Enemy _target;

    public override void Attach(Enemy target)
    {
        _target = target;
        target.EnemyEffectController.SetCC(ECC.Binding, _duration);

        target.EnemyEffectController.onEffectEnd[Utills.GetFlagIndex(ECC.Binding)] = OnEndEffect;

        _target.Movement.EnemySpeed = _target.EnemyData.Speed * _damp;
    }

    void OnEndEffect()
    {
        _target.Movement.EnemySpeed = _target.EnemyData.Speed;
    }
}
