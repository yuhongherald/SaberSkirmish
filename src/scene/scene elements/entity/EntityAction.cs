using System;

/// <summary>
/// A class that encodes an action of an <see cref="AEntity"/>.
/// </summary>
public class EntityAction
{
    public readonly Func<bool> func;
    public readonly AEntity.Status status;
    public EntityAction(Func<bool> func, AEntity.Status status)
    {
        this.func = func;
        this.status = status;
    }
}
