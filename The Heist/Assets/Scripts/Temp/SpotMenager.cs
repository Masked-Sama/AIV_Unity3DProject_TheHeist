using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotMenager : MonoBehaviour
{
    [SerializeField]
    private List<Spot> spots;

    public List<Spot> Spots {  get { return spots; } }

}
