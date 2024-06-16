//using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



public class WaveMenager : MonoBehaviour
{
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
    private List<Spanwer> spanwers;

    [SerializeField]
    private ChangeScene changeScene;

    private int counter;
    private int counterEnemiesDied;

    private bool poolEnemySpawned;
    private bool poolEnemiesDied;



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

        foreach (var spanwer in spanwers)
        {
            spanwer.SpawnRate = waveData.Rate;
        }

    }

    #endregion
  
    public void CountSpawn()
    {
        counter++;
        if (counter >= spanwers.Count)
        {
            foreach (var spanwer in spanwers)
            {
                spanwer.CanSpawns = false;
            }
            poolEnemySpawned = true;
            changeScene.ChangeSceneStarter = true;
        }
    }
    public void EnemyDied()
    {
        counterEnemiesDied++;
        if (counterEnemiesDied >= waveData.Counter) poolEnemiesDied = true;
    }
    public bool LevelEnd()
    {
        return poolEnemySpawned && poolEnemiesDied;
    }
}
