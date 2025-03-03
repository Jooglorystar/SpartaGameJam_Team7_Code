using System;
using UnityEngine;
using UnityEngine.Events;

public class MyEnemyt : MonoBehaviour
{
    public int currentECC;

    public Transform[] wayPoint = null;
    public float speed =3f;
    public float currentSpeed = 3f;
    public int index = 0;
    public bool isDead = false;

    public float[] ccTimer;

    public UnityAction[] onEffectEnd;

    private void Start()
    {
        int ccCount = Enum.GetValues(typeof(ECC)).Length;
        ccTimer = new float[ccCount];
        onEffectEnd = new UnityAction[ccCount];
    }

    private void Update()
    {
        if (isDead || wayPoint.Length == 0) return; // 사망했거나 웨이포인트가 없으면 이동 중단

        MoveToWayPoint();

        UpdateEffect();
    }

    void MoveToWayPoint()
    {
        Transform target = wayPoint[index]; // 현재 목표 웨이포인트

        // 목표 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        // 목표에 도착하면 다음 웨이포인트로 변경
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            index++;

            // 마지막 웨이포인트 도착하면 처음으로 리셋
            if (index >= wayPoint.Length)
            {
                index = 0;
            }
        }
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
