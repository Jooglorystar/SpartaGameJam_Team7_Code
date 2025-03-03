using UnityEngine;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField]private Image[] _playerHPIcons;

    private void Awake()
    {
        _playerHPIcons = GetComponentsInChildren<Image>();
    }

    public void RefreshHPIcon(int p_value)
    {
        for (int i = 0; i < _playerHPIcons.Length; i++)
        {
            _playerHPIcons[i].gameObject.SetActive(false);
            if (i < p_value)
            {
                _playerHPIcons[i].gameObject.SetActive(true);
            }
        }
    }
}