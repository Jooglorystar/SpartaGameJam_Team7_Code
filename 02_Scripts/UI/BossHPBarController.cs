using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBarController : MonoBehaviour
{
    [SerializeField] private Image _bossHPbar;
    [SerializeField] private TextMeshProUGUI _bossNameText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ActivateBossHPBar(Enemy p_enemy)
    {
        gameObject.SetActive(true);
        RefreshBossHPbar(p_enemy);
        RefreshBossNameTest(p_enemy);
    }

    public void RefreshBossHPbar(Enemy p_enemy)
    {
        _bossHPbar.fillAmount = (float)p_enemy.CurrnetHp / p_enemy.EnemyData.Hp;
    }

    private void RefreshBossNameTest(Enemy p_enemy)
    {
        _bossNameText.text = p_enemy.EnemyData.EnemyName;
    }
}
