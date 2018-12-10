using UnityEngine;

public class TestInstructions : MonoBehaviour
{
    public ArcadeInstruction swingLeft;
    public ArcadeInstruction swingRight;
    public ArcadeInstruction swingForward;
    public ArcadeInstruction attack;

    private ArcadeInstruction currentInstruction;

    private void Start()
    {
        currentInstruction = attack;
    }

    private void Update()
    {
        if (Input.GetKey("left")) SetInstruction(swingLeft);
        else if (Input.GetKey("right")) SetInstruction(swingRight);
        else if (Input.GetKey("up")) SetInstruction(swingForward);
        else if (Input.GetKey("down")) SetInstruction(attack);
    }

    private void SetInstruction(ArcadeInstruction instruction)
    {
        if (instruction == currentInstruction) return;
        currentInstruction.SetActive(false);
        instruction.SetActive(true);
        currentInstruction = instruction;
    }
}
