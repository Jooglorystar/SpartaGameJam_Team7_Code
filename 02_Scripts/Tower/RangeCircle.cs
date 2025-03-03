using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;

    public int segments = 100; // 원을 구성하는 점 개수
    public Color lineColor = Color.red; // 원의 색상
    Transform _originParent;

    void Awake()
    {
        _originParent = transform.parent;

        _lineRenderer.positionCount = segments + 1; // 마지막 점이 첫 점과 연결되도록 1 추가
        _lineRenderer.useWorldSpace = false; // 로컬 좌표 기준으로 원을 그림
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.loop = true; // 원이 닫히도록 설정
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 Shader 사용
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
            points[i] = new Vector3(Mathf.Cos(angle) * newRadius, 0, Mathf.Sin(angle) * newRadius); // Y축 고정 (XZ 평면에 원 그리기)
        }

        _lineRenderer.SetPositions(points);
        _lineRenderer.startColor = lineColor;
        _lineRenderer.endColor = lineColor;
    }
}
