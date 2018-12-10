using UnityEngine;

/// <summary>
/// A class to orchestrate <see cref="AudioSource"/> music and voiceovers such that only 1
/// music track and 1 voice is playing.
/// Music loops when playing.
/// Voice does not loop.
/// </summary>
public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource startMusic;
    [SerializeField]
    private AudioSource levelMusic;
    [SerializeField]
    private AudioSource bossMusic;

    [SerializeField]
    private AudioSource backgroundStory;
    [SerializeField]
    private AudioSource tutorialStart;
    [SerializeField]
    private AudioSource tutorialRanged;
    [SerializeField]
    private AudioSource tutorialEnd;
    [SerializeField]
    private AudioSource preBoss;
    [SerializeField]
    private AudioSource gameEnd;

    private AudioSource currentMusic;
    private static AudioSource blank = new AudioSource();
    private AudioSource currentVoice = blank;

    public enum MUSIC
    {
        START,
        LEVEL,
        BOSS,
        LENGTH
    }

    public enum VOICE
    {
        STORY,
        TUTORIAL_START,
        TUTORIAL_RANGED,
        TUTORIAL_END,
        PRE_BOSS,
        GAME_END,
        LENGTH
    }

    /// <summary>
    /// Sets the music to be played.
    /// </summary>
    public void SetMusic(MUSIC musicType)
    {
        switch (musicType)
        {
            case MUSIC.START:
                SetMusic(startMusic);
                break;
            case MUSIC.LEVEL:
                SetMusic(levelMusic);
                break;
            case MUSIC.BOSS:
                SetMusic(bossMusic);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets the voice to be played.
    /// </summary>
    public void SetVoice(VOICE voiceType)
    {
        switch (voiceType)
        {
            case VOICE.STORY:
                SetVoice(backgroundStory);
                break;
            case VOICE.TUTORIAL_START:
                SetVoice(tutorialStart);
                break;
            case VOICE.TUTORIAL_RANGED:
                SetVoice(tutorialRanged);
                break;
            case VOICE.TUTORIAL_END:
                SetVoice(tutorialEnd);
                break;
            case VOICE.PRE_BOSS:
                SetVoice(preBoss);
                break;
            case VOICE.GAME_END:
                SetVoice(gameEnd);
                break;
            default:
                break;
        }
    }

    private void SetMusic(AudioSource music)
    {
        if (currentMusic == music)
        {
            return;
        }
        if (currentMusic)
        {
            currentMusic.Stop();
        }
        Debug.Log("Playing music " + music.name);
        music.Play();
        currentMusic = music;
    }

    private void SetVoice(AudioSource voice)
    {
        if (voice == currentVoice)
        {
            return;
        }
        if (currentVoice)
        {
            currentVoice.Stop();
        }
        Debug.Log("Playing voice " + voice.name);
        voice.Play();
        currentVoice = voice;
    }
}
