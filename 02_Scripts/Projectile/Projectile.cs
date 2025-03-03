using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _rotateXSpeed = 360f;

    TowerData _data;
    Enemy _target;
    UnityAction<Enemy, bool> _onHitAction = null;
    CancellationTokenSource _cancellationTokenSource = null;

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

    public void ShotProjectile(TowerData data, Enemy target, UnityAction<Enemy, bool> onHitAction = null)
    {
        _data = data;
        _target = target;
        _onHitAction = onHitAction;

        _cancellationTokenSource = new CancellationTokenSource();
        _ = MoveToTarget(_cancellationTokenSource.Token);
    }

    async Awaitable MoveToTarget(CancellationToken token)
    {
        if (_target == null) // Ÿ���� ������ �ٷ� ����
        {
            Destroy(gameObject);
            return;
        }

        while (_target != null && Vector3.Distance(transform.position, _target.PrefabOffset.position) > 0.1f)
        {
            if (token.IsCancellationRequested) break; // ��ҵǾ����� ����

            // Ÿ�� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, _target.PrefabOffset.position, _data.ProjectileSpeed * Time.deltaTime);
            transform.LookAt(_target.transform); // Ÿ���� ���ϵ��� ȸ��

            // X������ ȸ��
            transform.Rotate(Vector3.right * _rotateXSpeed * Time.deltaTime);

            await Awaitable.NextFrameAsync(); // ���� �����ӱ��� ���
        }

        OnHitTarget();
    }

    void OnHitTarget()
    {
        _onHitAction?.Invoke(_target, _data.IsSplash);

        Destroy(gameObject); // ����ü ����
    }

    void OnDestroy()
    {
        _cancellationTokenSource?.Cancel(); // ��ü ���� ���� ��ū ���
    }
}
