using System;


    public interface IStrategy {
    Node.Status Process(ref BehaviourState currentState);
    //Node.Status Process();

    //Node.Status Process();

    void Reset() {
            // Noop
        }
    }

    public class ActionStrategy : IStrategy {
        readonly Action doSomething;
        
        public ActionStrategy(Action doSomething) {
            this.doSomething = doSomething;
        }
        
    public Node.Status Process(ref BehaviourState currentState)
    {
        doSomething();
        return Node.Status.Success;
    }
}
    
    public class Condition : IStrategy {
        protected Func<bool> predicate;
        
        public Condition(Func<bool> predicate) {
            this.predicate = predicate;
        }
        public Condition() { }
        public Node.Status Process(ref BehaviourState currentState) => predicate() ? Node.Status.Success : Node.Status.Failure;
    }

   
    
    

