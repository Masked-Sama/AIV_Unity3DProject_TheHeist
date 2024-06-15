using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public bool IsFree { get; set; }

    [SerializeField]
    private Transform spotPosition;

    private void Awake()
    {
        IsFree = true;
    }
    public Vector3 SpotPosition()
    {
        return spotPosition.position;
    }
}
