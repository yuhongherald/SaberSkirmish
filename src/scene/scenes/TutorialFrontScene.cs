/// <summary>
/// A <see cref="BaseTutorialScene"/> with a front close ranged attacker.
/// </summary>
public class TutorialFrontScene : BaseTutorialScene
{
    protected override Player.Position PLAYER_POSITION
    {
        get
        {
            return Player.Position.front;
        }
    }

    protected override CollectiveInstructions.InstType INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.SWING_FORWARD;
        }
    }

    protected override CollectiveInstructions.InstType ATTACK_INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.ATTACK_FORWARD;
        }
    }

}
