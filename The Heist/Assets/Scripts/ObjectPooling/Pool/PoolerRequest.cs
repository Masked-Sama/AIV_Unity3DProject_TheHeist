using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerRequest : MonoBehaviour
{
    [SerializeField] 
    private PoolData data;

    private void Awake()
    {
        Pooler.Istance.AddToPool(data);
    }
}
