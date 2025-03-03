using System;
using UnityEngine;
using UnityEngine.Events;

[Flags]
public enum ECC
{
    None = 0,
    Slow = 1 << 0,
    Binding = 1 << 1,
}

public class EnemyEffectController : MonoBehaviour
{
    Enemy _enemy;

    public int currentECC;

    public float[] ccTimer;
    public UnityAction[] onEffectEnd;

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
        currentECC = 0;
        int ccCount = Enum.GetValues(typeof(ECC)).Length;
        ccTimer = new float[ccCount];
        onEffectEnd = new UnityAction[ccCount];
    }

    private void Update()
    {
        if (_enemy.IsDead)
            return;

        UpdateEffect();
    }

    void UpdateEffect()
    {
        foreach (ECC cc in Enum.GetValues(typeof(ECC)))
        {
            if ((currentECC & (int)cc) != 0)
            {
                int index = Utills.GetFlagIndex(cc);
                if (index != -1)
                {
                    ccTimer[index] -= Time.deltaTime;

                    if (ccTimer[index] <= 0)
                    {
                        ccTimer[index] = 0;
                        currentECC &= ~(int)cc;

                        onEffectEnd[index]?.Invoke();
                    }
                }
            }
        }
    }

    public void SetCC(ECC cc, float duration)
    {
        currentECC |= (int)cc;
        int index = Utills.GetFlagIndex(cc);
        ccTimer[index] = duration;
    }
}
