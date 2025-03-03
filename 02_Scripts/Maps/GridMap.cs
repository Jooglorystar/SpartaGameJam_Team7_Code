using UnityEngine;

public class GridMap : MonoBehaviour
{
    [SerializeField] private GameObject _testobject;

    private Camera _main;

    [SerializeField] private LayerMask _detectedLayer;

    private Vector3 _tileCenter = new Vector3(0.5f, 0, 0.5f);

    private void Awake()
    {
        _main = Camera.main;
    }
}
