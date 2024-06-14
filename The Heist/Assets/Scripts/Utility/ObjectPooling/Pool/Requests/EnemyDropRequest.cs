using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropRequest : MonoBehaviour, IPoolRequester
{
    [SerializeField] 
    private PoolData[] drops;

    public PoolData[] Datas {
        get { return drops; }
    }
}
