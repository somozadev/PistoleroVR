using Unity.Netcode;
using UnityEngine;

public class MultiplayerMovementVR : MovementVR
{
    [SerializeField] private bool _enableInputActions;

    public bool EnableInputActions
    {
        get => _enableInputActions;
        set => _enableInputActions = value;
    }

    protected override Vector2 ReadInputLeftHand()
    {
        return !_enableInputActions ? Vector2.zero : base.ReadInputLeftHand();
    }

    protected override Vector2 ReadInputRightHand()
    {
        return !_enableInputActions ? Vector2.zero : base.ReadInputRightHand();
    }

    protected override void SetOrientationWithCam()
    {
        if (_enableInputActions)
            base.SetOrientationWithCam();
    }

    protected override void MoveCam()
    {
        if (_enableInputActions)
            base.MoveCam();
    }

}