using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Spanwer : MonoBehaviour, IPoolRequester
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private PoolData[] enemies;
    [SerializeField] private float[] percentages;
    [SerializeField] private bool canSpawn = true;

    private bool spawn = true;
    private  GameObject currentEnemy;
    public PoolData[] Datas {
        get { return enemies; }
    }
    
    private void SpawnerMethod()
    {
        if (canSpawn) {
            if (spawn) {
                int rand = Random.Range(0, 101);

                Debug.Log(rand);
                if (percentages[0] <= rand) {
                    Debug.Log("Sono detro la prima percentuale");
                    currentEnemy = Pooler.Istance.GetPooledObject(enemies[0]).GetComponent<GameObject>();
                }
                else if (percentages[1] <= rand) {
                    Debug.Log("Sono detro la seconda percentuale");
                    currentEnemy = Pooler.Istance.GetPooledObject(enemies[1]).GetComponent<GameObject>();
                }

                if (currentEnemy == null) {
                    Debug.Log("Non c'è nulla");
                    return;
                }
                currentEnemy.SetActive(true);
                currentEnemy.transform.position = transform.position;
                spawnRate = 1f;
            }
        }
    }
    private void Update() {
        if (spawnRate > 0) {
            spawnRate -= Time.deltaTime;
            if (spawnRate <= 0)
            {
                SpawnerMethod();
            }
        }
    }
}
