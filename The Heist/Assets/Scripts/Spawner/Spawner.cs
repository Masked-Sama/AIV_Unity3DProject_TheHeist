using UnityEngine;

public class Spanwer : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private PoolData enemies;
    [SerializeField] private bool canSpawn = true;

    private bool spawn = true;
    private float CD = 1f;
    private  IGrenade currentEnemy;
    
    private void SpawnerMethod()
    {
        if (canSpawn) {
            if (spawn) {
                currentEnemy = Pooler.Istance.GetPooledObject(enemies).GetComponent<IGrenade>();
                if (currentEnemy != null) {
                    Debug.Log("Found and istantiated");
                    // currentEnemy.SetActive(true);
                    // currentEnemy.transform.position = transform.position;
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
