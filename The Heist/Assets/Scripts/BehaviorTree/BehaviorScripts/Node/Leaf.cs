using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    readonly IStrategy strategy;

    BehaviourTree owner;

    BehaviourState state = BehaviourState.END;

    public Leaf(string name, IStrategy strategy, int priority = 0, BehaviourTree behaviorTree = null) : base(name, priority)
    {
        // Preconditions.CheckNotNull(strategy);
        this.strategy = strategy;
        owner = behaviorTree;
    }
    
    public override Status Process() => strategy.Process(ref owner != null ? ref owner.currentState : ref state);

    public override void Reset() => strategy.Reset();
}
