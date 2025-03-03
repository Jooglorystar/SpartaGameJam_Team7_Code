using UnityEngine.EventSystems;

public class BuildableTile : Tile, IPointerDownHandler
{
    bool _isLocated;
    GenericTower _tower;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        
        if (_isLocated)
        {
            _spawner.SetRadiusCircle(_tower);
            // 업그레이드 보여주기

            UIManagerTemp.Instance.ShowUpgradeUI(_spawner, _tower);
        }
        else
        {
            UIManagerTemp.Instance.OnClickBuildableTile(transform.position, SpawnTower);
        }
    }

    void SpawnTower()
    {
        if (_spawner.TrySpawn(transform.position, out _tower))
        {
            _tower.onDestroy -= ClearTile;
            _tower.onDestroy += ClearTile;

            _isLocated = true;
            _spawner.SetRadiusCircle(_tower);
        }
    }

    void ClearTile()
    {
        _isLocated = false;
    }

    private void OnEnable()
    {
        _isLocated = false;
    }
}