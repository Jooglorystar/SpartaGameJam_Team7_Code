using UnityEngine;
using UnityEngine.EventSystems;

public enum ETileType
{
    Default,
    Road,
    Buildable,
    Waypoint,
    Start,
    End
}

public class Tile : MonoBehaviour, IPointerDownHandler
{
    public ETileType tileType;

    protected TowerSpawner _spawner;

    public void Inject(TowerSpawner spawner)
    {
        _spawner = spawner;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _spawner.HideRadiusCircle();
        UIManagerTemp.Instance.HideBuildableButton();
        UIManagerTemp.Instance.HideUpgradeUI();
    }
}
