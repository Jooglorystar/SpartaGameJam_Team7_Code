using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    protected abstract void Init();
}
