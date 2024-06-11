using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


enum Scenes
{
    LobbyScene,
    EasyMapScene
}

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    Scenes scene = new Scenes();
    [SerializeField]
    private bool changeSceneStarter;
    
    private Coroutine changeSceneCoroutine;
    private string sceneToLoad;

    public bool ChangeSceneStarter{
        get { return changeSceneStarter; }
        set { changeSceneStarter = value; }
    }

    // Update is called once per frame
    void Update()
    {
        switch (scene)
        {
            case Scenes.LobbyScene: sceneToLoad = "LobbyScene";
                break;            
            case Scenes.EasyMapScene: sceneToLoad = "EasyMapScene";
                break;
        }
        
        if (changeSceneCoroutine != null) return;
        if (sceneToLoad == null) return;
        if (changeSceneStarter) changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine());
        
    }

    private IEnumerator ChangeSceneCoroutine () {
        var loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!loadScene.isDone) {
            yield return new WaitForEndOfFrame();
        }
    }
}