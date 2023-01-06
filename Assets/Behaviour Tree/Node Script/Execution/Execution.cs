using UnityEngine;

public abstract class Execution : Node 
{
    /// <summary>
    /// Do override in concrete implementations of this type to set variables values if there is any
    /// </summary>
    public override Node Copy()
    {
        return base.Copy();
    }
}
