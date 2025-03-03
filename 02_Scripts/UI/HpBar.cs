using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HpBar : MonoBehaviour
{
    Enemy _enemy;
    Slider _slider;
    Camera _cam;

    public void Init(Enemy enemy)
    {
        _cam = Camera.main;

        _enemy = enemy;
        if (_slider == null)
            _slider = GetComponent<Slider>();

        enemy.onChangedHp -= SetHp;
        enemy.onChangedHp += SetHp;
        enemy.onRelease -= HideHpBar;
        enemy.onRelease += HideHpBar;

        Vector3 screenPosition = _cam.WorldToScreenPoint(_enemy.transform.position);
        transform.position = screenPosition;
    }

    private void Update()
    {
        if (_enemy != null)
        {
            Vector3 screenPosition = _cam.WorldToScreenPoint(_enemy.transform.position);
            transform.position = screenPosition;
        }
    }

    public void SetHp(int prev, int cur)
    {
        _slider.value = (float)cur / _enemy.EnemyData.Hp;
    }

    void HideHpBar()
    {
        if(this != null)
            Destroy(gameObject);
    }
}
