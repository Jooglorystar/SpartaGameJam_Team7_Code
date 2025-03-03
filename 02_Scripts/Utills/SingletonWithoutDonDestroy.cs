using UnityEngine;

// ������Ʈ�� �ش� ������Ʈ�� ������ ���� �� ���� �ϴ� ��Ʈ����Ʈ
[DisallowMultipleComponent]
public abstract class SingletonWithoutDonDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isApplicationQuit = false;

    public static T Instance
    {
        get
        {
            if (isApplicationQuit == true)
                return null;

            if (instance == null)
            {
                T[] _finds = FindObjectsByType<T>(FindObjectsSortMode.None);
                if (_finds.Length > 0)
                {
                    instance = _finds[0];
                }

                if (_finds.Length > 1)
                {
                    for (int i = 1; i < _finds.Length; i++)
                        Destroy(_finds[i].gameObject);
                }

                if (instance == null)
                {
                    GameObject _createGameObject = new GameObject(typeof(T).Name);
                    instance = _createGameObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuit = true;
    }
}