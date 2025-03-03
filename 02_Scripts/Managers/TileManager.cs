using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonWithoutDonDestroy<TileManager>
{
    public Transform startPoint;
    public Transform endPoint;

    public Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        SetBuildableTiles();
    }

    private void SetBuildableTiles()
    {
        Tile[] tiles = GetComponentsInChildren<Tile>();
        TowerSpawner spawner = FindFirstObjectByType<TowerSpawner>();
        foreach(Tile tile in tiles)
        {
            tile.Inject(spawner);
        }
    }
}