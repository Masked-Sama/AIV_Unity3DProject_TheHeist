using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFollowThePlayer : Condition
{
    public CanFollowThePlayer(Func<bool> predicate) : base(predicate)
    {

    }
}
