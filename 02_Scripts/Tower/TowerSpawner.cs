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
        if (Random.Range(0, 1f) <= _chanceTier2) // 2�ܰ� ���� Ȯ��
        {
            towerData = _towerDataLevel2[Random.Range(0, _towerDataLevel2.Length)];
            UIManagerTemp.Instance.SetLog("2�ܰ� ���� ��ȯ!!");
        }
        else
        {
            towerData = _towerDataLevel1[Random.Range(0, _towerDataLevel1.Length)];
        }

        GenericTower instance = Instantiate(_genericTower, GridPosition(position), Quaternion.identity); // Ŭ���� ��ġ�� ������ ����\
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

    // �ش� Ÿ���� ���� ���׷��̵� �� �� �ִ��� ��ȯ�ϴ� �޼ҵ�
    public bool CheckUpgradableTower(GenericTower towerToUpgrade)
    {
        bool result = true;

        TowerData upgradableTower = towerToUpgrade.TowerData.UpgradableTower;

        if (upgradableTower == null || upgradableTower.IngredientTowers == null)
            return false;

        Dictionary<TowerData, int> requiredIngredients = DictionaryPool<TowerData, int>.Get();

        // �ʿ��� ��� ���� ī����
        foreach (TowerData ingredient in upgradableTower.IngredientTowers)
        {
            if (!requiredIngredients.ContainsKey(ingredient))
                requiredIngredients[ingredient] = 0;

            requiredIngredients[ingredient]++;
        }

        Dictionary<TowerData, int> availableTowers = DictionaryPool<TowerData, int>.Get();

        // �ʵ忡 �ִ� Ÿ�� ���� ī����
        foreach (GenericTower tower in _towers)
        {
            if (requiredIngredients.ContainsKey(tower.TowerData))
            {
                if (!availableTowers.ContainsKey(tower.TowerData))
                    availableTowers[tower.TowerData] = 0;

                availableTowers[tower.TowerData]++;
            }
        }

        // �ʿ��� ��ᰡ ������� Ȯ��
        foreach (var ingredient in requiredIngredients)
        {
            if (!availableTowers.ContainsKey(ingredient.Key) || availableTowers[ingredient.Key] < ingredient.Value)
            {
                result = false; // �ʿ��� ��� ����
                break;
            }
        }

        DictionaryPool<TowerData, int>.Release(requiredIngredients);
        DictionaryPool<TowerData, int>.Release(availableTowers);

        return result;
    }

    // CheckUpgradableTower �޼ҵ忡�� true�� ��ȯ���� ��쿡�� �����ؾ��Ѵ�.
    public void UpgradeTower(GenericTower towerToUpgrade)
    {
        if (towerToUpgrade == null || towerToUpgrade.TowerData.UpgradableTower == null)
            return;

        // �ʿ��� ����� ������ Dictionary�� ����
        Dictionary<TowerData, int> requiredIngredients = DictionaryPool<TowerData, int>.Get(); ;

        foreach (TowerData ingredient in towerToUpgrade.TowerData.UpgradableTower.IngredientTowers)
        {
            if (requiredIngredients.ContainsKey(ingredient))
                requiredIngredients[ingredient]++;
            else
                requiredIngredients[ingredient] = 1;
        }

        // �ڱ��ڽ��� ����.
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

                // ��� ��ᰡ ����� ���̸� ���� ����
                if (requiredIngredients.Values.All(count => count == 0))
                    break;
            }
        }

        // ���� Ÿ�� ����
        foreach (GenericTower tower in towersToRemove)
        {
            _towers.Remove(tower);
            tower.DestroyTower();
        }

        DictionaryPool<TowerData, int>.Release(requiredIngredients);
        ListPool<GenericTower>.Release(towersToRemove);

        // Ÿ�� ���׷��̵� ����
        towerToUpgrade.Inject(this, towerToUpgrade.TowerData.UpgradableTower);
        towerToUpgrade.UpdateTower();
    }

    public void DestroyTower(GenericTower tower)
    {
        _towers.Remove(tower);
        tower.DestroyTower();
    }
}
