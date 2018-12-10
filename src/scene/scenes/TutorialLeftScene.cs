/// <summary>
/// A <see cref="BaseTutorialScene"/> with a left close ranged attacker.
/// </summary>
public class TutorialLeftScene : BaseTutorialScene
{
    protected override Player.Position PLAYER_POSITION
    {
        get
        {
            return Player.Position.left;
        }
    }

    protected override CollectiveInstructions.InstType INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.SWING_LEFT;
        }
    }

    protected override CollectiveInstructions.InstType ATTACK_INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.ATTACK_LEFT;
        }
    }
}
