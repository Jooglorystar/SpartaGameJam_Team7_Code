using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string poolTag;
        public int poolSize;
        public GameObject gameObject;
        public Queue<GameObject> objectQueue = new Queue<GameObject>();
    }

    public Pool[] pools;
    public Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();

    public void CreatePool()
    {
        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.gameObject);
                obj.SetActive(false);
                pool.objectQueue.Enqueue(obj);
            }

            poolDict.Add(pool.poolTag, pool);
        }
    }

    public GameObject DrawObject(string p_poolTag)
    {
        if (!poolDict.ContainsKey(p_poolTag)) return null;

        GameObject obj = poolDict[p_poolTag].objectQueue.Dequeue();
        obj.SetActive(true);
        poolDict[p_poolTag].objectQueue.Enqueue(obj);

        return obj;
    }

    public T DrawObject<T>(string p_poolTag)
    {
        if (!poolDict.ContainsKey(p_poolTag)) return default;

        GameObject obj = poolDict[p_poolTag].objectQueue.Dequeue();
        if (obj.TryGetComponent<T>(out T test))
        {
            obj.SetActive(true);
            poolDict[p_poolTag].objectQueue.Enqueue(obj);
            return test;
        }
        return default;
    }
}
