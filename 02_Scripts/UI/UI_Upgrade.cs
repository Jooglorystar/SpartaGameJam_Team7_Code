using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Upgrade : MonoBehaviour
{
    [SerializeField] Image _imageBackground;
    [SerializeField] TextMeshProUGUI _textUnitName;
    [SerializeField] Image _imageUnitPortrait;
    [SerializeField] TextMeshProUGUI _textExplain;
    [SerializeField] Button _buttonUpgrade;
    [SerializeField] Image _imageUpgrade;
    [SerializeField] TextMeshProUGUI _textUpgrade;
    [SerializeField] TextMeshProUGUI _textCost;

    [SerializeField] Color[] _uiColor;

    [SerializeField] Color _possibleUpgradeColor;
    [SerializeField] Color _impossibleUpgradeColor;

    TowerSpawner _spawner;
    GenericTower _tower;
    TowerData _data;

    public void SetUI(TowerSpawner spawner, GenericTower tower)
    {
        _spawner = spawner;
        _tower = tower;
        _data = tower.TowerData;

        _imageBackground.color = _uiColor[_data.TowerLevel];

        _textUnitName.text = _data.TowerName;
        _imageUnitPortrait.sprite = _data.TowerPortrait;

        _textExplain.text = _data.TowerExplain;

        SetTextUpgrade(_data);
        _textCost.text = _data.TowerSellCost.ToString() + "원";
    }

    void SetTextUpgrade(TowerData data)
    {
        if (data.UpgradableTower == null)
        {
            _buttonUpgrade.interactable = false;
            _imageUpgrade.color = _impossibleUpgradeColor;
            _textUpgrade.text = "최종 단계입니다.";
        }
        else
        {
            _buttonUpgrade.interactable = true;
            _imageUpgrade.color = _possibleUpgradeColor;
            _textUpgrade.text = data.UpgradableTower.TowerName + "로 업그레이드";
        }
    }

    #region OnClick
    public void OnClickUpgrade()
    {
        if(_spawner.CheckUpgradableTower(_tower))
        {
            _spawner.UpgradeTower(_tower);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("개수 모자르다.");
        }
    }

    public void onClickSell()
    {
        PlayerManager.Instance.ChangeGold(_data.TowerSellCost);
        _spawner.DestroyTower(_tower);
        gameObject.SetActive(false);
    }
    #endregion
}
