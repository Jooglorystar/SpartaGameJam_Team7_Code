using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : Singleton<SceneManagerEX>
{
    public void LoadScene(string sceneName)
    {
        FadeIn();

        SceneManager.LoadScene(sceneName);

        FadeOut();
    }

    void FadeIn() // 씬전환 fadeIn, Out 메소드입니다. 실제로 만들 땐 코루틴이나 awaitable 사용하시면 됩니다. (누가 만들지는 아직 모름)
    {

    }

    void FadeOut()
    {

    }
}
