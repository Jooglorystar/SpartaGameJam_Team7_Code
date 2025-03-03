using UnityEngine;

public class WaypointManager : SingletonWithoutDonDestroy<WaypointManager>
{
    public Waypoint[] wayPoints;

    private void Start()
    {
        wayPoints = GetComponentsInChildren<Waypoint>();
    }
}
