using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A scene that introduces the background story to the player.
/// </summary>
public class StoryScene : Scene
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private CollectiveInstructions instructions;
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private GameObject scrollingText;
    [SerializeField]
    private RectTransform crystal;
    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 endPos;

    [SerializeField]
    private Image blueScreen;
    [SerializeField]
    private HealthBar healthBar;

    private Quaternion playerOffsetRotation;
    private float timer = 0;

    private void Update()
    {
        if (status == Status.Opening)
        {
            PlayStory();
        }
        else if (status == Status.Open && playerOffsetRotation != player.OffsetRotation)
        {
            CloseScene();
        }
    }

    private void PlayStory()
    {
        timer += Time.deltaTime;
        scrollingText.transform.localPosition = Vector3.Lerp(startPos, endPos, timer / 14.0f);
        if (timer > 7.0f)
        {
            GrowCrystal();
        }
        if (timer > 14.0f)
        {
            FinishStory();
        }
        InterpolateBlueScreenColor();
    }

    private void InterpolateBlueScreenColor()
    {
        Color color = blueScreen.color;
        if (color.a > 0.01f)
        {
            color.a -= 0.014f;
            blueScreen.color = color;
        }
    }

    private void FinishStory()
    {
        scrollingText.SetActive(false);
        crystal.gameObject.SetActive(false);
        status = Status.Open;
        instructions.SetInstruction(CollectiveInstructions.InstType.START);
    }

    private void GrowCrystal()
    {
        crystal.gameObject.SetActive(true);
        crystal.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (Mathf.Min(8.0f, timer) - 7.0f));
    }

    public override void CloseScene()
    {
        instructions.SetInstruction(CollectiveInstructions.InstType.NONE);
        healthBar.Show(true);
        status = Status.Closed;
    }

    public override void OpenScene()
    {
        ResetBlueScreen();
        ResetPlayerOffsetRotation();
        InitializeSceneElements();
        status = Status.Opening;
        timer = 0;
    }

    private void InitializeSceneElements()
    {
        musicManager.SetMusic(MusicManager.MUSIC.LEVEL);
        scrollingText.SetActive(true);
        crystal.gameObject.SetActive(false);
        scrollingText.transform.localPosition = startPos;
        musicManager.SetVoice(MusicManager.VOICE.STORY);
    }

    private void ResetPlayerOffsetRotation()
    {
        player.OffsetRotation = Quaternion.identity;
        playerOffsetRotation = player.OffsetRotation;
    }

    private void ResetBlueScreen()
    {
        Color color = blueScreen.color;
        color.a = 1;
        blueScreen.color = color;
    }

    public override void Teardown()
    {
        throw new System.NotImplementedException();
    }
}
