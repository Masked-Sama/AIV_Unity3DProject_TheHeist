using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    [SerializeField]
    private string sceneToLoad;

    private Coroutine changeSceneCoroutine;
    [SerializeField]
    private bool changeSceneStarter;

    public bool ChangeSceneStarter{
        get { return changeSceneStarter; }
        set { changeSceneStarter = value; }
    }

    // Update is called once per frame
    void Update()
    {
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