using System;
using System.Collections.Generic;
using UnityEngine;

public class Spanwer : MonoBehaviour
{
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private PoolData enemies;
    [SerializeField] private bool canSpawn = true;

    [Header("Snipers Only")] 
    [SerializeField] private List<Spot> covers;

    public bool CanSpawns { get { return canSpawn; } set { canSpawn = value; } }

    private bool spawn;
    private float CD;
    private  GameObject currentEnemy;
  //  private BehaviorTreeSniper sniperBT;

    public float SpawnRate { get { return spawnRate; } set {  spawnRate = value; } }

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
                    // if (currentEnemy.CompareTag("Untagged")) 
                    //if (!currentEnemy.CompareTag("Untagged") && currentEnemy.CompareTag("Snipers"))
                    //{
                    //    sniperBT = currentEnemy.GetComponent<BehaviorTreeSniper>();
                        
                    //    sniperBT.Spots = covers;
                    //    WaveMenager.Get().CountSpawn(); 
                    //}
                    currentEnemy.transform.position = transform.position;
                    currentEnemy.SetActive(true);
                    WaveMenager.Get().CountSpawn();
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
