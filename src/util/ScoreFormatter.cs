/// <summary>
/// Formats an integer score into a string for display.
/// </summary>
public class ScoreFormatter : IFormatable<string, int>
{
    public const int SCORE_LENGTH = 7;
    public const char SCORE_PADDING = '0';

    public string Format(int data)
    {
        string value = data.ToString().PadLeft(SCORE_LENGTH, SCORE_PADDING);
        return value;
    }
}
