using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class StartScene : BaseScene
{
    [SerializeField] VideoPlayer _videoPlayer;

    protected override void Init()
    {
        SoundManager.Instance.PlayBGM("title gbm");
    }

    public void StartGame()
    {
        if(PlayerPrefs.GetInt("IsFirst") != 1) // 처음이다.
        {
            PlayerPrefs.SetInt("IsFirst", 1);

            SoundManager.Instance.PlayBGM("crazy-chase-126687");

            _videoPlayer.gameObject.SetActive(true);
            _videoPlayer.Play();
            _videoPlayer.loopPointReached += (vp) => SceneManagerEX.Instance.LoadScene("MainScene");
        }
        else
        {
            SceneManagerEX.Instance.LoadScene("MainScene");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void PlaySFX(string sfx)
    {
        SoundManager.Instance.PlaySFX(sfx);
    }
}
