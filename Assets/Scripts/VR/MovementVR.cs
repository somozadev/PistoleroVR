using System;
using General;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using Unity.XR.CoreUtils;

public class MovementVR : LocomotionProvider
{
    public enum RotationType
    {
        Continuous,
        Snap
    }

    [Header("Control movement status")] 
    [SerializeField] private bool canMove;
    [SerializeField] private bool canRotate;
    [SerializeField] private bool isGrounded;
    [SerializeField] private RotationType rotationType;
    [SerializeField] private float turnAmount = 45f;
    [SerializeField] private bool readyToSnapTurn;

    [Space(20)] [Header("Movement Variables")]
    public float speed = 5;

    public float rotSpeed;
    public float drag = 0;
    public float groundDistance=0.1f;
    public LayerMask groundLayerMask;
    private RaycastHit _slopeHit;

    [Space(20)] [Header("Hands controllers InputActions references")] [SerializeField]
    private InputActionProperty _leftHandMoveAction;

    [SerializeField] private InputActionProperty _rightHandMoveAction;
    private Vector2 _lInput;
    private Vector2 _rInput;

    [Space(20)] [Header("Needed references")] [SerializeField]
    private Rigidbody _moveRb;

    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private Rigidbody _lookRb;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Transform _orientationTrf;
    [SerializeField] private Transform _raycastOrigin;


    public Transform CameraHolder => _cameraHolder;

    public void DisableMovement()
    {
        canMove = false;
        canRotate = false;
    }

    public void EnableMovement()
    {
        canMove = true;
        canRotate = true;
    }

    protected override void Awake()
    {
        _xrOrigin = GetComponentInParent<XROrigin>();
        // BeginLocomotion();
        // _leftHandMoveAction.EnableDirectAction();
        // _rightHandMoveAction.EnableDirectAction();
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
        transform.position = Vector3.zero;
    }

    private void OnDestroy()
    {
        OnDisable();
    }

    /// <summary>
    /// Unity calls <see cref="FixedUpdate"/> around 50 times per second, used for physics and avoid jittering.
    /// Saves input from both hands in <paramref key="inputL"/> and <paramref key="inputR"/> respectively. With it,
    /// calls <see cref="ComputeDesiredDirection"/> with left input to get <paramref key="translationInWorldSpace"/> the direction of the movement.
    /// Finally, calls <see cref="MoveRig"/> passing both the direction and the right input in order to move the character by physics.
    /// </summary>
    /// <param key="inputL"> Left hand controller input value </param>
    /// <param key="inputR"> Right hand controller input value </param>
    /// <param key="translationInWorldSpace"> Desired direction of the movement </param>
    protected void FixedUpdate()
    {
        _lInput = ReadInputLeftHand();
        _rInput = ReadInputRightHand();
        var translationInWorldSpace = ComputeDesiredDirection(_lInput);
        MoveRig(translationInWorldSpace, _rInput);
    }


    /// <summary>
    /// Sets the <see cref="_orientationTrf"/> rotation to a new quaternion based on the <see cref ="_lookRb"/> y euler angle multiplied by an offset,
    /// given by the extra rotation from the <see cref="ReadInputRightHand"/>.
    /// In other words, sets the orientation gameobject to the camera looking orientation on the Y axis. 
    /// </summary>
    ///<param key="localEulerAngles"> stores the vector3 of the localEulerAngles of the <see cref="_orientationTrf"/></param>
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
    /// Applies a force to the main rigidbody <see cref="_moveRb"/> on the given <paramref key="direction"/>
    /// </summary>
    /// <param key="direction"> Desired direction of the movement</param>
    private void Move(Vector3 direction)
    {
        // _moveRb.AddForce(direction, ForceMode.Force);
        if (isGrounded)
            _moveRb.velocity = direction * speed * Time.deltaTime;
        else
            _moveRb.velocity = new Vector3(direction.x, -40, direction.z) * speed * Time.deltaTime;
    }

    public void UpadteRotationType(RotationType type)
    {
        rotationType = type;
    }


    /// <summary>
    /// Rotates the camera parent object <see cref="_cameraHolder"/> in the Y axis with a given speed.
    /// </summary>
    /// <param key="rotSpeedFinal">Based on given rotSpeed value, calculares if it should be positive or negative based on the <paramref key="inputRight"/> x value</param>
    /// <param key="inputRight">The Vector2 of the current right hand input, calculated in <see cref="ReadInputRightHand()"/></param>
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

    private void RotateSnapTurn(Vector2 inputRight)
    {
        if (inputRight.x > 0)
        {
            if (readyToSnapTurn)
            {
                readyToSnapTurn = false;
                _xrOrigin.RotateAroundCameraUsingOriginUp(Mathf.Abs(turnAmount));
            }
        }
        else if (inputRight.x < 0)
        {
            if (readyToSnapTurn)
            {
                readyToSnapTurn = false;
                _xrOrigin.RotateAroundCameraUsingOriginUp(-turnAmount);
            }
        }
        else
        {
            readyToSnapTurn = true;
        }
    }


    /// <summary>
    /// Calculates the direction of the movement based on the <paramref key="inputRight"/> value and the <see cref="_orientationTrf"/> tranasform
    /// </summary>
    /// 
    /// <param key="input">The vector2 value of the current left hand input. Calculated in  <see cref="ReadInputLeftHand()"/></param>
    /// <param key="forwardDir">A vector3 based on the forward direction of the <see cref="_orientationTrf"/> and the <paramref key="input"/></param>
    /// <param key="moveDirection">The final vector3 direction, normalizing <paramref key="forwardDir"/> and multiplying it by a constant 1000, the <see cref="speed"/> and <see cref="Time.fixedDeltaTime"/></param>
    /// <returns></returns>
    private Vector3 ComputeDesiredDirection(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector3.zero;
        // return _moveRb.position;
        Vector3 forwardDir = _orientationTrf.forward * input.y + _orientationTrf.right * input.x;

        Vector3 moveDirection = Vector3.Normalize(forwardDir) * (speed * 1000 * Time.fixedDeltaTime);
        return moveDirection;
    }


    /// <summary>
    /// Applyes a <see cref="drag"/> value to the <see cref="_moveRb"/> drag
    /// </summary>
    private void Drag() => _moveRb.drag = drag;

    /// <summary>
    /// If the locomotion system allows it by <see cref="LocomotionProvider.CanBeginLocomotion"/> and <see cref="LocomotionProvider.BeginLocomotion"/>
    /// and the control bool <see cref="canMove"/> is true, <see cref="Move"/> will be called, passing in the direction <paramref key="translationInWorldSpace"/>
    /// and if the control bool <see cref="canRotate"/> is true, <see cref="Rotate"/> will be called, passing in the value of  <paramref key="inputRight"/>.
    ///
    /// As well as calling <see cref="SetOrientationWithCam"/> and <see cref="LocomotionProvider.EndLocomotion"/> in that order.
    /// </summary>
    /// <param key="translationInWorldSpace">Direction calculated in <see cref="ComputeDesiredDirection"/></param>
    /// <param key="inputRight">Input value of the right hand, calculated in <see cref="ReadInputRightHand"/></param>
    private void MoveRig(Vector3 translationInWorldSpace, Vector2 inputRight)
    {
        isGrounded = IsGrounded();
        if (CanBeginLocomotion() && BeginLocomotion())
        {
            if (canMove)
                Move(translationInWorldSpace);
            if (canRotate)
            {
                if (rotationType.Equals(RotationType.Continuous))
                    Rotate(inputRight);
                else
                    RotateSnapTurn(inputRight);
            }


            SetOrientationWithCam();
            EndLocomotion();
        }
    }


    private bool IsGrounded()
    {
        if (Physics.Raycast(_raycastOrigin.position, Vector3.down, groundDistance, groundLayerMask))
        {
            Debug.DrawRay(_raycastOrigin.position, Vector3.down, Color.green, 1f);
            return true;
        }
        return false;
    }


    public void ResetTo(Vector3 target)
    {
        _moveRb.MovePosition(target);
    }
}