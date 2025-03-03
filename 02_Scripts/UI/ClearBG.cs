using TMPro;
using UnityEngine;

public class ClearBG : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _panelText;


    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        RefreshText("Game Over");
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX("Grenade10Short");
    }

    public void GameClear()
    {
        gameObject.SetActive(true);
        RefreshText("농장 지키기를 성공했습니다!!");
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX("SFX - Rooster, Doodle-Doo, 04");
    }

    private void RefreshText(string p_string)
    {
        _panelText.text = p_string;
    }


    public void OnRetryButton()
    {
        SoundManager.Instance.PlayBGM("Golden-Hour-chosic.com_");
        SceneManagerEX.Instance.LoadScene("MainScene");
    }

    public void OnGoLobbyButton()
    {
        SoundManager.Instance.PlayBGM("Golden-Hour-chosic.com_");
        SceneManagerEX.Instance.LoadScene("StartScene");
    }
}