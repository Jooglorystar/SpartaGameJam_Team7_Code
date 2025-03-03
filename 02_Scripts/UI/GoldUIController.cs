using TMPro;
using UnityEngine;

public class GoldUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldText;

    public void RefreshGoldText()
    {
        _goldText.text = PlayerManager.Instance.gold.ToString();
    }
}