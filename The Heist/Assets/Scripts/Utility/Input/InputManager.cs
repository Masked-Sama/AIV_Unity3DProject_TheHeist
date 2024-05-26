using UnityEngine.InputSystem;

public static class InputManager
{
    private static Inputs input;

    static InputManager()
    {
        input = new Inputs();
        input.Player.Enable();
    }

    public static Inputs.PlayerActions Player
    {
        get { return input.Player; }
    }

}
