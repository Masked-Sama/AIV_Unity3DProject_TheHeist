using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletRequest : MonoBehaviour, IPoolRequester
{
    [SerializeField] 
    private PoolData[] bullets;

    public PoolData[] Datas {
        get { return bullets; }
    }
}
