using UnityEngine;
using UnityEngine.UI;

public class WaitingUI : MonoBehaviour
{
    [SerializeField] private Image _waitingFill;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void RefreshImage(float p_value, float p_total)
    {
        _waitingFill.fillAmount = p_value / p_total;
    }
}
