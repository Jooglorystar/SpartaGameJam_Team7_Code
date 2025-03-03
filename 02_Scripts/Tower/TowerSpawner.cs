using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] TowerData[] _towerDataLevel1;
    [SerializeField] TowerData[] _towerDataLevel2;

    [SerializeField] float _chanceTier2 = 0.05f;

    [SerializeField] GenericTower _genericTower;
    [SerializeField] RangeCircle _rangeCircle;
    [SerializeField] LayerMask _detectedLayer;
    [SerializeField] LayerMask _ignoreLayer;

    [SerializeField] List<GenericTower> _towers = new List<GenericTower>();

    private Vector3 _tileCenter = new Vector3(0.5f, 0, 0.5f);

    private int _towerCost = 10;

    public bool TrySpawn(Vector3 position, out GenericTower tower)
    {
        if (PlayerManager.Instance.gold < _towerCost)
        {
            tower = null;
            return false;
        }

        tower = Spawn(position);
        return true;
    }

    private GenericTower Spawn(Vector3 position)
    {
        TowerData towerData = null;
        if (Random.Range(0, 1f) <= _chanceTier2) // 2단계 뽑을 확률
        {
            towerData = _towerDataLevel2[Random.Range(0, _towerDataLevel2.Length)];
            UIManagerTemp.Instance.SetLog("2단계 유닛 소환!!");
        }
        else
        {
            towerData = _towerDataLevel1[Random.Range(0, _towerDataLevel1.Length)];
        }

        GenericTower instance = Instantiate(_genericTower, GridPosition(position), Quaternion.identity); // 클릭한 위치에 프리팹 생성\
        instance.Inject(this, towerData);
        instance.UpdateTower();
        PlayerManager.Instance.ChangeGold(-_towerCost);
        _towers.Add(instance);

        return instance;
    }

    public void SetRadiusCircle(GenericTower tower)
    {
        _rangeCircle.gameObject.SetActive(true);
        _rangeCircle.DrawCircle(tower.TowerData.TowerRange, tower);
    }

    public void HideRadiusCircle()
    {
        _rangeCircle.gameObject.SetActive(false);
    }

    private Vector3 GridPosition(Vector3 p_inputPosition)
    {
        Vector3Int treatedPosition = TileManager.Instance.grid.WorldToCell(p_inputPosition);
        Vector3 worldPosition = TileManager.Instance.grid.CellToWorld(treatedPosition) + _tileCenter;

        return worldPosition;
    }

    // 해당 타워가 지금 업그레이드 할 수 있는지 반환하는 메소드
    public bool CheckUpgradableTower(GenericTower towerToUpgrade)
    {
        bool result = true;

        TowerData upgradableTower = towerToUpgrade.TowerData.UpgradableTower;

        if (upgradableTower == null || upgradableTower.IngredientTowers == null)
            return false;

        Dictionary<TowerData, int> requiredIngredients = DictionaryPool<TowerData, int>.Get();

        // 필요한 재료 개수 카운팅
        foreach (TowerData ingredient in upgradableTower.IngredientTowers)
        {
            if (!requiredIngredients.ContainsKey(ingredient))
                requiredIngredients[ingredient] = 0;

            requiredIngredients[ingredient]++;
        }

        Dictionary<TowerData, int> availableTowers = DictionaryPool<TowerData, int>.Get();

        // 필드에 있는 타워 개수 카운팅
        foreach (GenericTower tower in _towers)
        {
            if (requiredIngredients.ContainsKey(tower.TowerData))
            {
                if (!availableTowers.ContainsKey(tower.TowerData))
                    availableTowers[tower.TowerData] = 0;

                availableTowers[tower.TowerData]++;
            }
        }

        // 필요한 재료가 충분한지 확인
        foreach (var ingredient in requiredIngredients)
        {
            if (!availableTowers.ContainsKey(ingredient.Key) || availableTowers[ingredient.Key] < ingredient.Value)
            {
                result = false; // 필요한 재료 부족
                break;
            }
        }

        DictionaryPool<TowerData, int>.Release(requiredIngredients);
        DictionaryPool<TowerData, int>.Release(availableTowers);

        return result;
    }

    // CheckUpgradableTower 메소드에서 true로 반환됐을 경우에만 실행해야한다.
    public void UpgradeTower(GenericTower towerToUpgrade)
    {
        if (towerToUpgrade == null || towerToUpgrade.TowerData.UpgradableTower == null)
            return;

        // 필요한 재료의 개수를 Dictionary에 저장
        Dictionary<TowerData, int> requiredIngredients = DictionaryPool<TowerData, int>.Get(); ;

        foreach (TowerData ingredient in towerToUpgrade.TowerData.UpgradableTower.IngredientTowers)
        {
            if (requiredIngredients.ContainsKey(ingredient))
                requiredIngredients[ingredient]++;
            else
                requiredIngredients[ingredient] = 1;
        }

        // 자기자신은 뺀다.
        if (requiredIngredients.ContainsKey(towerToUpgrade.TowerData))
        {
            requiredIngredients[towerToUpgrade.TowerData]--;
        }

        List<GenericTower> towersToRemove = ListPool<GenericTower>.Get();

        foreach (GenericTower tower in _towers)
        {
            if (tower == towerToUpgrade)
                continue;

            if (requiredIngredients.ContainsKey(tower.TowerData) && requiredIngredients[tower.TowerData] > 0)
            {
                towersToRemove.Add(tower);
                requiredIngredients[tower.TowerData]--;

                // 모든 재료가 충분히 모이면 조기 종료
                if (requiredIngredients.Values.All(count => count == 0))
                    break;
            }
        }

        // 실제 타워 제거
        foreach (GenericTower tower in towersToRemove)
        {
            _towers.Remove(tower);
            tower.DestroyTower();
        }

        DictionaryPool<TowerData, int>.Release(requiredIngredients);
        ListPool<GenericTower>.Release(towersToRemove);

        // 타워 업그레이드 적용
        towerToUpgrade.Inject(this, towerToUpgrade.TowerData.UpgradableTower);
        towerToUpgrade.UpdateTower();
    }

    public void DestroyTower(GenericTower tower)
    {
        _towers.Remove(tower);
        tower.DestroyTower();
    }
}
