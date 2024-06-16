using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


enum Scenes
{
    LobbyScene,
    EasyLevel
}

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private Scenes scene = new Scenes();
    [SerializeField]
    private bool changeSceneStarter;
    
    private Coroutine changeSceneCoroutine;
    private string sceneToLoad;

    public bool ChangeSceneStarter{
        get { return changeSceneStarter; }
        set
        {
            changeSceneStarter = value;
            if (changeSceneStarter)
            {
                if (changeSceneCoroutine != null) return;
                ChooseScene(scene);
                if (sceneToLoad == null) return;
                changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine());
            }
           
        }
    }

    private void Awake()
    {
        changeSceneStarter = false;
    }
    
    private void ChooseScene(Scenes choosedScene)
    {
        switch (choosedScene)
        {
            case Scenes.LobbyScene: sceneToLoad = "LobbyScene";
                break;            
            case Scenes.EasyLevel: sceneToLoad = "EasyLevel";
                break;
        }
    }
    private IEnumerator ChangeSceneCoroutine () {
        var loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!loadScene.isDone) {
            yield return new WaitForEndOfFrame();
        }

    }
}