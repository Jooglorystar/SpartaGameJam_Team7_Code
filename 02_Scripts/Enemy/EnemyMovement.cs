using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float EnemySpeed { get; set; }
    [SerializeField] private Vector3 _moveDirection;

    private Vector3 _defaultDirection = new Vector3(0, 270, 0);

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        if (PlayerManager.Instance.isGameOver) EnemySpeed = 0f;

        _rigidbody.linearVelocity = _moveDirection * EnemySpeed;
    }

    public void TurnAround(Waypoint p_waypoint)
    {
        _moveDirection = p_waypoint.moveDireciton;
        transform.rotation = Quaternion.Euler(p_waypoint.targetDirection);
    }

    public void TurnAround()
    {
        _moveDirection = -Vector3.left;
        transform.rotation = Quaternion.Euler(_defaultDirection);
    }
}
