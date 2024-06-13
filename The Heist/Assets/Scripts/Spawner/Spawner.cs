using System;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private PoolData enemies;
    [SerializeField] private bool canSpawn = true;

    private bool spawn;
    private float CD;
    private  GameObject currentEnemy;

    private void Start()
    {
        spawn = true;
        CD = spawnRate;
    }

    private void SpawnerMethod()
    {
        if (canSpawn) {
            if (spawn) {
                currentEnemy = Pooler.Istance.GetPooledObject(enemies);
                if (currentEnemy != null) {
                    //Debug.Log("Found and istantiated");
                    currentEnemy.SetActive(true);
                    currentEnemy.transform.position = transform.position;
                }
                CD = spawnRate;
            }
        }
    }
    private void Update() {
        if (CD > 0) {
            CD -= Time.deltaTime;
            if (CD <= 0) {
                SpawnerMethod();
            }
        }
    }
}
