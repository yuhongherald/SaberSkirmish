/// <summary>
/// Formats a integer score into a string for display.
/// </summary>
public interface IFormatable<Value, Data>
{
    /// <summary>
    /// Formats <see cref="Data"/> into <see cref="Value"/> representation.
    /// </summary>
    Value Format(Data data);    
}
