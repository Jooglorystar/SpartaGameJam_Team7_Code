using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ListItem_Combination : MonoBehaviour
{
    [SerializeField] Image[] _ingredientImages;
    [SerializeField] TextMeshProUGUI[] _ingredientTexts;

    [SerializeField] Image _upgradeImage;
    [SerializeField] TextMeshProUGUI _upgradeText;

    [SerializeField] Image[] _opers;

    [SerializeField] Sprite _addSprite;
    [SerializeField] Sprite _equalSprite;

    [SerializeField] Color[] _textColor;

    public void SetUI(TowerData towerData)
    {
        foreach (Image ingredientImage in _ingredientImages)
        {
            ingredientImage.gameObject.SetActive(false);
        }

        foreach (Image oper in _opers)
        {
            oper.gameObject.SetActive(false);
        }

        for (int i = 0; i < towerData.UpgradableTower.IngredientTowers.Length; ++i)
        {
            TowerData ingredientData = towerData.UpgradableTower.IngredientTowers[i];

            _ingredientImages[i].gameObject.SetActive(true);
            _ingredientImages[i].sprite = ingredientData.TowerPortrait;
            _ingredientTexts[i].text = ingredientData.TowerName;
            _ingredientTexts[i].color = _textColor[ingredientData.TowerLevel];

            _opers[i].gameObject.SetActive(true);
            _opers[i].sprite = i == towerData.UpgradableTower.IngredientTowers.Length - 1 ? _equalSprite : _addSprite;
        }

        _upgradeImage.gameObject.SetActive(true);
        _upgradeImage.sprite = towerData.UpgradableTower.TowerPortrait;
        _upgradeText.text = towerData.UpgradableTower.TowerName;
        _upgradeText.color = _textColor[towerData.UpgradableTower.TowerLevel];
    }
}
