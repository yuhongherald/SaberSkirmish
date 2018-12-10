/// <summary>
/// A <see cref="BaseTutorialScene"/> with a right close ranged attacker.
/// </summary>
public class TutorialRightScene : BaseTutorialScene
{
    protected override Player.Position PLAYER_POSITION
    {
        get
        {
            return Player.Position.right;
        }
    }

    protected override CollectiveInstructions.InstType INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.SWING_RIGHT;
        }
    }

    protected override CollectiveInstructions.InstType ATTACK_INST_TYPE
    {
        get
        {
            return CollectiveInstructions.InstType.ATTACK_RIGHT;
        }
    }
}
