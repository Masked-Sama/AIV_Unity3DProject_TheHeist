using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.SceneManagement;
using System;

public class StartingPosition : MonoBehaviour
{

    void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player.Player.Get().transform.position = transform.position;
        Player.Player.Get().transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
