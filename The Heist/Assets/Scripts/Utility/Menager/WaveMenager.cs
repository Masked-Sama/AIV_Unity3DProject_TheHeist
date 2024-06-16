//using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;



public class WaveMenager : MonoBehaviour
{
    // [SerializeField]
    // private int enemiesAmount;

    private static WaveMenager instance;
    public static WaveMenager Get()
    {
        if (instance != null) return instance;
        instance = GameObject.FindObjectOfType<WaveMenager>();
        return instance;
    }

    [SerializeField]
    private WaveData waveData;

    [SerializeField]
    private List<Spawner> spawners;

    [SerializeField]
    private ChangeScene changeScene;

    private int counter;
    private int counterEnemiesDied;

    private bool poolEnemySpawned;
    private bool poolEnemiesDied;

    private int totalEnemies;
    #region Mono
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
            return;
        }
        instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (instance != this) return;

        foreach (var spawner in spawners)
        {
            spawner.SpawnRate = 1;//waveData.Rate;
        }

    }

    private void FixedUpdate()
    {
        if (LevelEnd())
        {
            //chiama change scene
            changeScene.ChangeSceneStarter = true;
        }
    }

    #endregion
  
    public void CountSpawn()
    {
        counter++;
        if (counter > totalEnemies)
        {
            foreach (var spawner in spawners)
            {
                spawner.CanSpawns = false;
            }
            poolEnemySpawned = true;
            //changeScene.ChangeSceneStarter = true;
        }
        
        foreach (var spawner in spawners)
        {
            totalEnemies += spawner.GetComponent<Spawner>().Enemies.PoolNumber;
        }
        
    }
    public void EnemyDied()
    {
        //Debug.Log("Shono morto");
        counterEnemiesDied++;
        if (counterEnemiesDied >= totalEnemies) poolEnemiesDied = true; //waveData.Counter) poolEnemiesDied = true;
    }
    public bool LevelEnd()
    {
        return poolEnemySpawned && poolEnemiesDied;
    }
}
