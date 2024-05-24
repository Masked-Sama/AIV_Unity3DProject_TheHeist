using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StayInIdle : IStrategy
{
    private Enemy owner;
    private bool everyFrame;

    public StayInIdle(bool everyFrame, Enemy owner = null) 
    {
        if (owner)
            this.owner = owner;
            this.everyFrame = everyFrame;
    }

    public Node.Status Process()
    {
        //Da inserire il check per evitare le chiamate ad ogni frame! 
        /*if (everyFrame || "EnemyInIdle")
        {
            hasExecutedOnce = true;
            owner.ClearDestination();
        }*/

        Debug.Log("Stay in Idle");

        if (owner) owner.ClearDestination();

        return Node.Status.Success;
    }


}
