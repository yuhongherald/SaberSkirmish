using System;

using UnityEngine;

/// <summary>
/// Manages the scoring system for the game.
/// </summary>
public class ScoreBoard : MonoBehaviour
{
    private static ScoreBoard scoreBoard;

    [SerializeField]
    private UIText multiplier;
    [SerializeField]
    private UIText levelScore;

    [SerializeField]
    private UIText finalGrade;
    [SerializeField]
    private UIText bestMultiplier;
    [SerializeField]
    private UIText finalScore;

    private const int STOPPED = -1;
    private const int FINAL_SCORE = 0;
    private const int BEST_COMBO = 80;
    private const int FINAL_GRADE = 160;
    private const int END = 240;

    public const string SCORE_PREFIX = "Score: ";
    private const string COMBO_PREFIX = "Best combo: x";
    private const string GRADE_PREFIX = "Grade: ";
    private const float MAX_SCALE = 2.0f;
    private const int DURATION = 80;
    private const int SCORE_MULTIPLIER = 100;
    private const int MULTIPLIER_DURATION = 20;
    private const float MULTIPLIER_START_SIZE = 1.2f;
    private int bestCombo = 0;
    private int combo = 0;
    private int score = 0;

    private int animationFrame = STOPPED;
    private bool isLevelActive = false;

    public static ScoreBoard GetInstance()
    {
        if (!scoreBoard)
        {
            throw new NullReferenceException("Execution order error, got null scoreboard reference");
        }
        return scoreBoard;
    }

    private void Awake()
    {
        scoreBoard = this;
    }

    private void Update()
    {
        AnimateScoreboard();
    }

    private void AnimateScoreboard()
    {
        if (animationFrame != STOPPED)
        {
            if (animationFrame == FINAL_SCORE)
            {
                finalScore.gameObject.SetActive(true);
                finalScore.SetText(SCORE_PREFIX + new ScoreFormatter().Format(score));
                finalScore.Grow(MAX_SCALE, 1.0f, DURATION);
            }
            else if (animationFrame == BEST_COMBO)
            {
                bestMultiplier.gameObject.SetActive(true);
                bestMultiplier.SetText(COMBO_PREFIX + bestCombo);
                bestMultiplier.Grow(MAX_SCALE, 1.0f, DURATION);
            }
            else if (animationFrame == FINAL_GRADE)
            {
                finalGrade.gameObject.SetActive(true);
                finalGrade.SetText(GRADE_PREFIX + new TitleFormatter().Format(score));
                finalGrade.Grow(MAX_SCALE, 1.0f, DURATION);
            }
            animationFrame++;
            if (animationFrame >= END)
            {
                animationFrame = STOPPED;
            }
        }
    }

    /// <summary>
    /// Shows the highscore board.
    /// </summary>
    public void ShowHighscores()
    {
        Debug.Log("Highscores shown");
        isLevelActive = false;
        animationFrame = 0;
        BreakStreak();
        levelScore.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides the highscore board.
    /// </summary>
    public void HideHighScores()
    {
        Debug.Log("Highscores hidden");
        animationFrame = -1;
        finalScore.gameObject.SetActive(false);
        bestMultiplier.gameObject.SetActive(false);
        finalGrade.gameObject.SetActive(false);
    }

    /// <summary>
    /// Changes scoreboard state for level display.
    /// </summary>
    public void StartLevel()
    {
        Debug.Log("Highscores set to level start");
        isLevelActive = true;
        combo = 0;
        BreakStreak();
        score = 0;
        levelScore.gameObject.SetActive(true);
        levelScore.SetText(new ScoreFormatter().Format(score));
    }

    /// <summary>
    /// Increments the current combo streak and score.
    /// </summary>
    public void IncrementStreak()
    {
        Debug.Log("Highscore streak increase!");
        if (!isLevelActive) return;
        multiplier.gameObject.SetActive(true);
        combo++;
        multiplier.SetText("x" + combo);
        multiplier.Grow(MULTIPLIER_START_SIZE, 1.0f, MULTIPLIER_DURATION);
        score += combo * SCORE_MULTIPLIER;
        levelScore.SetText(new ScoreFormatter().Format(score));
    }

    /// <summary>
    /// Breaks the current combo streak.
    /// </summary>
    public void BreakStreak()
    {
        Debug.Log("Highscore streak broken!");
        bestCombo = Math.Max(bestCombo, combo);
        combo = 0;
        multiplier.gameObject.SetActive(false);
    }
}
