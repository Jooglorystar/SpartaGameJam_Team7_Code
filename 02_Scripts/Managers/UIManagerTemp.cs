using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManagerTemp : SingletonWithoutDonDestroy<UIManagerTemp>
{
    Camera _cam;

    [SerializeField] LogMessage _log;
    [SerializeField] Button _button;
    [SerializeField] UI_Upgrade _upgradeUI;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void OnClickBuildableTile(Vector3 tileWorldPosition, UnityAction onClick)
    {
        Vector3 screenPosition = _cam.WorldToScreenPoint(tileWorldPosition);

        _button.gameObject.SetActive(true);
        _button.transform.position = screenPosition;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(onClick);
        _button.onClick.AddListener(() => _button.gameObject.SetActive(false));
    }

    public void HideBuildableButton()
    {
        _button.gameObject.SetActive(false);
    }

    public void ShowUpgradeUI(TowerSpawner spawner, GenericTower tower)
    {
        _upgradeUI.gameObject.SetActive(true);
        _upgradeUI.SetUI(spawner, tower);
    }

    public void HideUpgradeUI()
    {
        _upgradeUI.gameObject.SetActive(false);
    }

    public void SetLog(string message)
    {
        _log.SetLog(message);
    }
}
