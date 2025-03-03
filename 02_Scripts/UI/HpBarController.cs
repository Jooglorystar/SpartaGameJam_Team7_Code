using UnityEngine;

public class HpBarController : MonoBehaviour
{
    public HpBar _hpBarPrefab;

    public void GetHpBar(Enemy enemy, int prev, int cur)
    {
        HpBar instance = Instantiate(_hpBarPrefab, transform);
        instance.gameObject.SetActive(true);
        instance.Init(enemy);
        instance.SetHp(prev, cur);
    }
}
