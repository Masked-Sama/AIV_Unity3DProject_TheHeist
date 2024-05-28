using UnityEngine;

[CreateAssetMenu(fileName = "PoolData", menuName = "ObjectPooling/Pool", order = 1)]
public class PoolData : ScriptableObject
{
    [SerializeField] 
    private string poolKey;
    [SerializeField] 
    private GameObject prefab;
    [SerializeField] 
    private int poolNumber;

    public string PoolKey {
        get { return poolKey; }
    }    
    public GameObject Prefab {
        get { return prefab; }
    }    
    public int PoolNumber {
        get { return poolNumber; }
    }
}
