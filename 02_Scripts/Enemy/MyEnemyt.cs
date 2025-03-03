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
        if (isDead || wayPoint.Length == 0) return; // ����߰ų� ��������Ʈ�� ������ �̵� �ߴ�

        MoveToWayPoint();

        UpdateEffect();
    }

    void MoveToWayPoint()
    {
        Transform target = wayPoint[index]; // ���� ��ǥ ��������Ʈ

        // ��ǥ �������� �̵�
        transform.position = Vector3.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);

        // ��ǥ�� �����ϸ� ���� ��������Ʈ�� ����
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            index++;

            // ������ ��������Ʈ �����ϸ� ó������ ����
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
