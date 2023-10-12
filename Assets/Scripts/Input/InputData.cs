using Fusion;

public enum InputButton
{
    LEFT = 0,
    RIGHT = 1,
    RESPAWN = 2,
    JUMP = 3,
    INSTALL = 4,
    RECALL = 5,
    UP = 6,
    DOWN = 7,
}

/// <summary>
/// Fusion 입력 구조체
/// </summary>
public struct InputData : INetworkInput
{
    public NetworkButtons Buttons;

    public bool GetButton(InputButton button)
    {
        return Buttons.IsSet(button);
    }

    public NetworkButtons GetButtonPressed(NetworkButtons prev)
    {
        return Buttons.GetPressed(prev);
    }

    public bool AxisPressed()
    {
        return GetButton(InputButton.LEFT) || GetButton(InputButton.RIGHT);
    }
}