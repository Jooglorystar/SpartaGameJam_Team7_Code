using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private enum EEnemyDirection
    {
        Forward,
        Back,
        Right,
        Left
    }

    [SerializeField] private EEnemyDirection enemyDirection;
    public Vector3 targetDirection;
    public Vector3 moveDireciton;

    private void Start()
    {
        switch (enemyDirection)
        {
            case EEnemyDirection.Forward:
                targetDirection = new Vector3(0, 180, 0);
                moveDireciton = Vector3.forward;
                break;
            case EEnemyDirection.Back:
                targetDirection = Vector3.zero;
                moveDireciton = -Vector3.forward;
                break;
            case EEnemyDirection.Right:
                targetDirection = new Vector3(0, 270, 0);
                moveDireciton = -Vector3.left;
                break;
            case EEnemyDirection.Left:
                targetDirection = new Vector3(0, 90, 0);
                moveDireciton = Vector3.left;
                break;
            default:
                Debug.LogWarning("enemyDirectionEmpty");
                break;
        }
    }
}
