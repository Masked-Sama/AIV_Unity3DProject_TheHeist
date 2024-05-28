using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    private Dictionary<string, GameObject[]> pool;

    public void AddToPool(PoolData data) {
        if (!pool.ContainsKey(data.PoolKey)) {
            InternalAddToPool(data);
            return;
        }
        if (pool[data.PoolKey].Length < data.PoolNumber) {
            ExtendExistingPool(data);
        }
    }

    private void InternalAddToPool(PoolData data)
    {
        GameObject[] pooledObject = new GameObject[data.PoolNumber];
        for(int i = 0; i < pooledObject.Length; i++) {
            pooledObject[i] = Instantiate(data.Prefab);
            pooledObject[i].SetActive(false);
        }
    }
    
    private void ExtendExistingPool(PoolData data)
    {
        GameObject[] pooledObject = new GameObject[data.PoolNumber];
        GameObject[] existingPool = pool[data.PoolKey];
        int i = 0;
        for (; i < existingPool.Length; i++)
        {
            pooledObject[i] = existingPool[i];
        }

        for (; i < pooledObject.Length; i++)
        {
            pooledObject[i] = Instantiate(data.Prefab);
            pooledObject[i].SetActive(false);
        }
        pool[data.PoolKey] = pooledObject;
    }
}
