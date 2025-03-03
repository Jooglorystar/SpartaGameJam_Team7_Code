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

    void FadeIn() // ����ȯ fadeIn, Out �޼ҵ��Դϴ�. ������ ���� �� �ڷ�ƾ�̳� awaitable ����Ͻø� �˴ϴ�. (���� �������� ���� ��)
    {

    }

    void FadeOut()
    {

    }
}
