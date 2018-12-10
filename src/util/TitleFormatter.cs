/// <summary>
/// Formats a integer score into a string for display.
/// </summary>
public class TitleFormatter : IFormatable<string, int>
{
    public const int PADAWAN_SCORE_LIMIT = 5000;
    public const int JEDI_SCORE_LIMIT = 10000;

    public const string PADAWAN = "Padawan";
    public const string JEDI_KNIGHT = "Jedi knight";
    public const string GRANDMASTER = "Grandmaster";

    public string Format(int data)
    {
        if (data < PADAWAN_SCORE_LIMIT)
        {
            return PADAWAN;
        }
        else if (data < JEDI_SCORE_LIMIT)
        {
            return JEDI_KNIGHT;
        }
        else
        {
            return GRANDMASTER;
        }
    }
}
