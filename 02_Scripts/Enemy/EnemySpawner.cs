using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveEnemyData
    {
        public EnemyData enemyData;
        public int enemyCount;
    }

    [System.Serializable]
    public class WaveData
    {
        public Enemy npc;
        public float respawnTime = 1f;
        public int maxCount = 30;
        public int curCount;
        public WaveEnemyData[] waveEnemyDatas;

        public void StartWave()
        {
            maxCount = 0;
            for (int i = 0; i < waveEnemyDatas.Length; i++)
            {
                maxCount += waveEnemyDatas[i].enemyCount;
            }
            curCount = maxCount;
        }

        public bool IsBossWave()
        {
            for (int i = 0; i < waveEnemyDatas.Length; i++)
            {
                if (waveEnemyDatas[i].enemyData.IsBoss)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public WaveData[] waveDatas;

    float timer = 0;
    float canSpawnTimer = 0f;
    int count = 0;

    public int curWaveIdx;

    private int spawnIdx;

    public int stageBuff;

    private PoolSystem _enemyPool;

    private bool canSpawn;

    public BossHPBarController bossHPBar;

    public PortalColorController portalColor;

    [SerializeField] private int _rewardGold = 20;

    [SerializeField] private GameObject _windParticle;

    [SerializeField] private WaitingUI _waitingUI;

    public ClearBG clearBG;

    public WaveData CurWave { get { return waveDatas[curWaveIdx]; } }

    private void Awake()
    {
        _enemyPool = GetComponent<PoolSystem>();
    }

    private void Start()
    {
        canSpawn = false;
        curWaveIdx = 0;
        spawnIdx = 0;
        waveDatas[curWaveIdx].StartWave();
        CheckBossWave();
    }

    private void Update()
    {
        CanSpawnTimer();

        if (curWaveIdx >= waveDatas.Length || spawnIdx >= waveDatas[curWaveIdx].waveEnemyDatas.Length) return;
        if (canSpawn)
        {
            timer += Time.deltaTime;
            if (timer > waveDatas[curWaveIdx].respawnTime && count < waveDatas[curWaveIdx].waveEnemyDatas[spawnIdx].enemyCount)
            {
                timer = 0;
                Spawn();
            }
        }
    }

    private void CanSpawnTimer()
    {
        if (canSpawn)
        {
            return;
        }

        _waitingUI.gameObject.SetActive(true);
        canSpawnTimer += Time.deltaTime;
        _waitingUI.RefreshImage(canSpawnTimer, 15f);
        if (canSpawnTimer >= 15f)
        {
            canSpawn = true;
            _waitingUI.gameObject.SetActive(false);
        }
    }

    public void EndWave()
    {
        canSpawn = false;
        canSpawnTimer = 0f;
        count = 0;
        spawnIdx = 0;

        PlayerManager.Instance.ChangeGold(_rewardGold);

        curWaveIdx++;
        if (curWaveIdx >= waveDatas.Length)
        {
            ClearGame();
            return;
        }

        waveDatas[curWaveIdx].StartWave();
        CheckBossWave();

    }

    void Spawn()
    {
        Enemy instance = _enemyPool.DrawObject<Enemy>("Enemy");

        instance.Inject(this, CurWave.waveEnemyDatas[spawnIdx].enemyData);
        count++;
        if (count >= CurWave.waveEnemyDatas[spawnIdx].enemyCount)
        {
            spawnIdx++;
            count = 0;
        }
        instance.Spawn();
        instance.transform.position = TileManager.Instance.startPoint.position;
    }

    private void CheckBossWave()
    {
        if (waveDatas[curWaveIdx].IsBossWave())
        {
            portalColor.ChangeBossPortal();
            _windParticle.SetActive(true);
            SoundManager.Instance.PlayBGM("Hitman(chosic.com)");
        }
        else
        {
            portalColor.ChangeDefaultPortal();
            _windParticle.SetActive(false);
            SoundManager.Instance.PlayBGM("Golden-Hour-chosic.com_");
        }
    }

    private void ClearGame()
    {
        clearBG.GameClear();
    }
}
