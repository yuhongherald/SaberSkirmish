using UnityEngine;

/// <summary>
/// A class to orchestrate <see cref="ArcadeInstruction"/> such that only 1 is playing.
/// </summary>
public class CollectiveInstructions : MonoBehaviour
{
    [SerializeField]
    private ArcadeInstruction startMessage;
    [SerializeField]
    private ArcadeInstruction instructions;
    [SerializeField]
    private ArcadeInstruction swingLeft;
    [SerializeField]
    private ArcadeInstruction swingRight;
    [SerializeField]
    private ArcadeInstruction swingForward;
    [SerializeField]
    private ArcadeInstruction attack;
    [SerializeField]
    private ArcadeInstruction attackLeft;
    [SerializeField]
    private ArcadeInstruction attackRight;
    [SerializeField]
    private ArcadeInstruction attackForward;
    [SerializeField]
    private ArcadeInstruction getReady;
    [SerializeField]
    private ArcadeInstruction level1;
    [SerializeField]
    private ArcadeInstruction level2;
    [SerializeField]
    private ArcadeInstruction bossStage;

    private static ArcadeInstruction blank = new ArcadeInstruction();
    private ArcadeInstruction currentInstruction = blank;

    public enum InstType
    {
        START,
        DEMONSTRATE,
        SWING_LEFT,
        SWING_RIGHT,
        SWING_FORWARD,
        ATTACK,
        ATTACK_LEFT,
        ATTACK_RIGHT,
        ATTACK_FORWARD,
        GET_READY,
        LEVEL_1,
        LEVEL_2,
        BOSS_STAGE,
        NONE,
        LENGTH
    }

    /// <summary>
    /// Sets the <see cref="ArcadeInstruction"/> to be animated.
    /// </summary>
    public void SetInstruction(InstType type)
    {
        switch (type)
        {
            case InstType.START:
                SetInstruction(startMessage);
                break;
            case InstType.DEMONSTRATE:
                SetInstruction(instructions);
                break;
            case InstType.SWING_LEFT:
                SetInstruction(swingLeft);
                break;
            case InstType.SWING_RIGHT:
                SetInstruction(swingRight);
                break;
            case InstType.SWING_FORWARD:
                SetInstruction(swingForward);
                break;
            case InstType.ATTACK:
                SetInstruction(attack);
                break;
            case InstType.ATTACK_LEFT:
                SetInstruction(attackLeft);
                break;
            case InstType.ATTACK_RIGHT:
                SetInstruction(attackRight);
                break;
            case InstType.ATTACK_FORWARD:
                SetInstruction(attackForward);
                break;
            case InstType.GET_READY:
                SetInstruction(getReady);
                break;
            case InstType.LEVEL_1:
                SetInstruction(level1);
                break;
            case InstType.LEVEL_2:
                SetInstruction(level2);
                break;
            case InstType.BOSS_STAGE:
                SetInstruction(bossStage);
                break;
            case InstType.NONE:
                SetInstruction(blank);
                break;
            default:
                break;
        }
    }

    private void SetInstruction(ArcadeInstruction instruction)
    {
        if (instruction == currentInstruction) return;
        if (currentInstruction != blank)
        {
            currentInstruction.SetActive(false);
        }
        if (instruction != blank)
        {
            Debug.Log("Setting instruction to " + instruction.name);
            instruction.SetActive(true);
        }
        currentInstruction = instruction;
    }
}
