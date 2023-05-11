using System;
using General;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using Player = Unity.Services.Lobbies.Models.Player;

public class MovementVR : LocomotionProvider
{
    [Header("Control movement status")] [SerializeField]
    private bool canMove;

    [SerializeField] private bool canRotate;
    [Header("Movement Variables")] public float speed;
    public float rotSpeed;
    public float drag;

    [Header("Hands controllers InputActions references")] [SerializeField]
    private InputActionProperty _leftHandMoveAction;

    [SerializeField] private InputActionProperty _rightHandMoveAction;

    [Header("Needed references")] [SerializeField]
    private Rigidbody _moveRb;

    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private Rigidbody _lookRb;
    [SerializeField] private Transform _characterRig;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Transform _cameraVR;
    [SerializeField] private Transform _orientationTrf;
    [SerializeField] private Transform _pivotCamTrf;

    protected override void Awake()
    {
        _xrOrigin = GetComponentInParent<XROrigin>();
        BeginLocomotion();
        _leftHandMoveAction.EnableDirectAction();
        _rightHandMoveAction.EnableDirectAction();
    }

    private void Start()
    {
        if (GameManager.Instance == null) return;

        if (
            !GameManager.Instance.players.Contains(GetComponentInParent<General.Player>()))
            GameManager.Instance.players.Add(GetComponentInParent<General.Player>());
    }

    /// <summary>
    /// Unity reads the value on action performed by the left hand. provided by <see cref="InputActionProperty"/>
    /// </summary>
    protected virtual Vector2 ReadInputLeftHand()
    {
        return _leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
    }

    /// <summary>
    /// Unity reads the value on action performed by the right hand. provided by <see cref="InputActionProperty"/>
    /// </summary>
    protected virtual Vector2 ReadInputRightHand()
    {
        return _rightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
    }

    /// <summary>
    /// When the gameobject gets enabled, enables both hands <see cref="InputActionProperty"/> to perform actions. Calls <see cref="Drag()"/> to start Drag calculations to the rigidbody
    /// </summary>
    private void OnEnable()
    {
        Drag();
    }


    /// <summary>
    /// When the gameobject gets disabled, disables both hands <see cref="InputActionProperty"/> to perform actions.
    /// </summary>
    private void OnDisable()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.players.Contains(GetComponentInParent<General.Player>()))
            GameManager.Instance.players.Remove(GetComponentInParent<General.Player>());
        _leftHandMoveAction.DisableDirectAction();
        _rightHandMoveAction.DisableDirectAction();
        EndLocomotion();
    }

    private void OnDestroy()
    {
        OnDisable();
    }

    /// <summary>
    /// Unity calls <see cref="FixedUpdate"/> around 50 times per second, used for physics and avoid jittering.
    /// Saves input from both hands in <paramref name="inputL"/> and <paramref name="inputR"/> respectively. With it,
    /// calls <see cref="ComputeDesiredDirection"/> with left input to get <paramref name="translationInWorldSpace"/> the direction of the movement.
    /// Finally, calls <see cref="MoveRig"/> passing both the direction and the right input in order to move the character by physics.
    /// </summary>
    /// <param name="inputL"> Left hand controller input value </param>
    /// <param name="inputR"> Right hand controller input value </param>
    /// <param name="translationInWorldSpace"> Desired direction of the movement </param>
    protected void FixedUpdate()
    {
        var inputL = ReadInputLeftHand();
        var inputR = ReadInputRightHand();
        var translationInWorldSpace = ComputeDesiredDirection(inputL);
        MoveRig(translationInWorldSpace, inputR);
    }

    /// <summary>
    /// Unity calls <see cref="Update"/> on every frame.
    /// Calls <see cref="SpeedControl"/> to limit the maximum velocity of the rigidbody to <see cref="speed"/> value.
    /// </summary>
    private void Update()
    {
        SpeedControl();
    }

    /// <summary>
    /// Sets <see cref="_cameraHolder"/> position to follow <see cref="_pivotCamTrf"/> position, due the fact that camera is outside the
    /// gameobject in charge of movement (its rigidbody).
    /// </summary>
    protected virtual void MoveCam()
    {
        _cameraHolder.position = _pivotCamTrf.position;
    }

    /// <summary>
    /// Sets the <see cref="_orientationTrf"/> rotation to a new quaternion based on the <see cref ="_lookRb"/> y euler angle multiplied by an offset,
    /// given by the extra rotation from the <see cref="ReadInputRightHand"/>.
    /// In other words, sets the orientation gameobject to the camera looking orientation on the Y axis. 
    /// </summary>
    ///<param name="localEulerAngles"> stores the vector3 of the localEulerAngles of the <see cref="_orientationTrf"/></param>
    protected virtual void SetOrientationWithCam()
    {
        var localEulerAngles = _orientationTrf.localEulerAngles;
        var rotation = _orientationTrf.rotation;
        rotation = Quaternion.Euler(localEulerAngles.x,
            _lookRb.transform.localEulerAngles.y, localEulerAngles.z);
        rotation *= _cameraHolder.rotation;
        _orientationTrf.rotation = rotation;
    }

    /// <summary>
    /// Applies a force to the main rigidbody <see cref="_moveRb"/> on the given <paramref name="direction"/>
    /// </summary>
    /// <param name="direction"> Desired direction of the movement</param>
    private void Move(Vector3 direction) => _moveRb.AddForce(direction, ForceMode.Force);

    /// <summary>
    /// Rotates the camera parent object <see cref="_cameraHolder"/> in the Y axis with a given speed.
    /// </summary>
    /// <param name="rotSpeedFinal">Based on given rotSpeed value, calculares if it should be positive or negative based on the <paramref name="inputRight"/> x value</param>
    /// <param name="inputRight">The Vector2 of the current right hand input, calculated in <see cref="ReadInputRightHand()"/></param>
    private void Rotate(Vector2 inputRight)
    {
        var rotSpeedFinal = rotSpeed;
        if (inputRight.x < 0)
            rotSpeedFinal = -rotSpeedFinal;
        else if (inputRight.x > 0)
            rotSpeedFinal = Mathf.Abs(rotSpeedFinal);
        else
            return;
        _xrOrigin.RotateAroundCameraUsingOriginUp(Time.fixedDeltaTime * rotSpeedFinal);
    }

    /// <summary>
    /// Calculates the direction of the movement based on the <paramref name="inputRight"/> value and the <see cref="_orientationTrf"/> tranasform
    /// </summary>
    /// 
    /// <param name="input">The vector2 value of the current left hand input. Calculated in  <see cref="ReadInputLeftHand()"/></param>
    /// <param name="forwardDir">A vector3 based on the forward direction of the <see cref="_orientationTrf"/> and the <paramref name="input"/></param>
    /// <param name="moveDirection">The final vector3 direction, normalizing <paramref name="forwardDir"/> and multiplying it by a constant 1000, the <see cref="speed"/> and <see cref="Time.fixedDeltaTime"/></param>
    /// <returns></returns>
    private Vector3 ComputeDesiredDirection(Vector2 input)
    {
        if (input == Vector2.zero)
            return _moveRb.position;
        Vector3 forwardDir = _orientationTrf.forward * input.y + _orientationTrf.right * input.x;

        Vector3 moveDirection = Vector3.Normalize(forwardDir) * (speed * 1000 * Time.fixedDeltaTime);
        return moveDirection;
    }

    /// <summary>
    /// Measures the current <see cref="_moveRb"/> velocity magnitude ignoring it's y axis. If it's greater than the limit <see cref="speed"/>, limits the rb velocity to it.
    /// </summary>
    private void SpeedControl()
    {
        var velocity = _moveRb.velocity;
        Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            _moveRb.velocity = new Vector3(limitedVel.x, velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Applyes a <see cref="drag"/> value to the <see cref="_moveRb"/> drag
    /// </summary>
    private void Drag() => _moveRb.drag = drag;

    /// <summary>
    /// If the locomotion system allows it by <see cref="LocomotionProvider.CanBeginLocomotion"/> and <see cref="LocomotionProvider.BeginLocomotion"/>
    /// and the control bool <see cref="canMove"/> is true, <see cref="Move"/> will be called, passing in the direction <paramref name="translationInWorldSpace"/>
    /// and if the control bool <see cref="canRotate"/> is true, <see cref="Rotate"/> will be called, passing in the value of  <paramref name="inputRight"/>.
    ///
    /// As well as calling <see cref="SetOrientationWithCam"/>, <see cref="MoveCam"/> and <see cref="LocomotionProvider.EndLocomotion"/> in that order.
    /// </summary>
    /// <param name="translationInWorldSpace">Direction calculated in <see cref="ComputeDesiredDirection"/></param>
    /// <param name="inputRight">Input value of the right hand, calculated in <see cref="ReadInputRightHand"/></param>
    private void MoveRig(Vector3 translationInWorldSpace, Vector2 inputRight)
    {
        if (CanBeginLocomotion() && BeginLocomotion())
        {
            if (canMove)
                Move(translationInWorldSpace);
            if (canRotate)
                Rotate(inputRight);
            SetOrientationWithCam();
            MoveCam();
            EndLocomotion();
        }
    }
}