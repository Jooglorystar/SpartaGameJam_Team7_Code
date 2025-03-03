using UnityEngine;

public class PlayerManager : SingletonWithoutDonDestroy<PlayerManager>
{
    public int playerHealthMax = 5;
    public int playerHealthCur;

    public GoldUIController goldUI;
    [SerializeField] private PlayerHPController playerHPUI;

    public int gold;

    [SerializeField] private int _startGold = 500;

    [SerializeField] private ParticleSystem _gameoverEffect;

    public bool isGameOver;

    private void SetStartGold()
    {
        gold = 0;
        ChangeGold(_startGold);
    }

    public void ChangeGold(int p_value)
    {
        gold += p_value;
        goldUI.RefreshGoldText();
    }

    public void SetPlayerHealth()
    {
        isGameOver = false;
        playerHealthCur = playerHealthMax;
        SetStartGold();
    }

    public void HitPlayer(Enemy p_enemy)
    {
        playerHealthCur--;

        if(p_enemy.EnemyData.IsBoss) playerHealthCur = 0;

        playerHPUI.RefreshHPIcon(playerHealthCur);

        if(playerHealthCur<=0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        // TODO 게임 오버 관련 처리
        Debug.Log("GameOver");
        isGameOver = true;
        GameManager.Instance.EnemySpawner.clearBG.GameOver();
        _gameoverEffect.Play();
    }
}
