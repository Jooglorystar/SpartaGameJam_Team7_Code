using UnityEngine;

public class UI_Combination : MonoBehaviour
{
    [SerializeField] TowerData[] _allData;
    [SerializeField] Transform _contents;
    [SerializeField] ListItem_Combination _listItemCombinationPrefab;

    private void Awake()
    {
        foreach(TowerData towerData in _allData)
        {
            if (towerData.UpgradableTower != null)
            {
                ListItem_Combination instance = Instantiate(_listItemCombinationPrefab, _contents);
                instance.SetUI(towerData);
                instance.gameObject.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        Invoke("StopTime", 0.6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        Time.timeScale = 1.0f;
    }

    void StopTime()
    {
        Time.timeScale = 0.0f;
    }
}
