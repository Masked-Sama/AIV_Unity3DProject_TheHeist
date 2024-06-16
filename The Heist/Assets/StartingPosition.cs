using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.SceneManagement;
using System;

public class StartingPosition : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnEnable()
    {

        
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player.Player.Get().transform.position = transform.position;
        Player.Player.Get().transform.rotation = transform.rotation;
    }

    
}
