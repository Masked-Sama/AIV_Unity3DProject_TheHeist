using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrategyAction : IStrategy
{
    //protected bool everyFrame;
    //protected bool hasExecutedOnce = false;
    public StrategyAction(bool everyFrame = false)
    {
        //this.everyFrame = everyFrame;
    }
    public abstract Node.Status Process();

}
