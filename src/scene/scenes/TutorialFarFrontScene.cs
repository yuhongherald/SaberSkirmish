/// <summary>
/// A <see cref="BaseTutorialScene"/> with a front far ranged attacker.
/// </summary>
public class TutorialFarFrontScene : BaseTutorialScene
{
    protected override Player.Position PLAYER_POSITION
    {
        get
        {
            return Player.Position.farFront;
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
