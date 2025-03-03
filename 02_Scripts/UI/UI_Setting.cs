using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] Slider _sfxVolumeSlider;
    [SerializeField] Slider _bgmVolumeSlider;

    private void Awake()
    {
        _sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        _sfxVolumeSlider.onValueChanged.AddListener(OnValueChangedSFX);

        _bgmVolumeSlider.onValueChanged.RemoveAllListeners();
        _bgmVolumeSlider.onValueChanged.AddListener(OnValueChangedBGM);
    }

    void OnValueChangedBGM(float value)
    {
        SoundManager.Instance.BGMSource.volume = value;
    }

    void OnValueChangedSFX(float value)
    {
        SoundManager.Instance.SFXSource.volume = value;
    }

    private void OnEnable()
    {
        _sfxVolumeSlider.value = SoundManager.Instance.SFXSource.volume;
        _bgmVolumeSlider.value = SoundManager.Instance.BGMSource.volume;

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

    public void LoadSceneStart()
    {
        SceneManagerEX.Instance.LoadScene("StartScene");
    }
}
