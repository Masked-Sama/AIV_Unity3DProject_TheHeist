using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    
    private static DontDestroyOnLoad instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);

    }
}
