using System.Collections;
using UnityEngine;

public abstract class ChefBaseState
{
    protected ChefBotFSM fsm; //only its subclasses can access this variable

    //define methods to be overridden in states (children/subclasses)
    public virtual void OnEnter()
    { }

    public virtual void OnUpdate()
    { }

    public virtual void OnExit()
    { }
}
