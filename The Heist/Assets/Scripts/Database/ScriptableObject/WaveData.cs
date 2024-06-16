using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaveTempDataTemplate", menuName = "Wave/Temp", order = 1)]
public class WaveData : ScriptableObject
{

    [SerializeField]
    private int counter;

    [SerializeField]
    private float rate;

    public int Counter { get; private set; }
    public float Rate { get; private set; }
}
