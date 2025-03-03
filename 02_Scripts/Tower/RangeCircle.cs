using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    public int segments = 100; // ���� �����ϴ� �� ����
    public Color lineColor = Color.red; // ���� ����
    Transform _originParent;

    void Awake()
    {
        _originParent = transform.parent;

        _lineRenderer.positionCount = segments + 1; // ������ ���� ù ���� ����ǵ��� 1 �߰�
        _lineRenderer.useWorldSpace = false; // ���� ��ǥ �������� ���� �׸�
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.loop = true; // ���� �������� ����
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ Shader ���
    }

    public void Hide()
    {
        if(_originParent != null)
        {
            transform.SetParent(_originParent);
            gameObject.SetActive(false);
        }
    }

    public void DrawCircle(float newRadius, GenericTower tower)
    {
        tower.onDestroy -= Hide;
        tower.onDestroy += Hide;

        transform.SetParent(tower.transform);
        transform.localPosition = Vector3.up;

        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = (i / (float)segments) * 2 * Mathf.PI;
            points[i] = new Vector3(Mathf.Cos(angle) * newRadius, 0, Mathf.Sin(angle) * newRadius); // Y�� ���� (XZ ��鿡 �� �׸���)
        }

        _lineRenderer.SetPositions(points);
        _lineRenderer.startColor = lineColor;
        _lineRenderer.endColor = lineColor;
    }
}
