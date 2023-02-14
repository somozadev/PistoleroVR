using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class MovementVR : LocomotionProvider
{
    [SerializeField] private InputActionProperty _leftHandMoveAction;
    public float speed;
    private Vector2 _inputAxis;
    [SerializeField] private Transform _targetTransform;
    private Vector3 _verticalVelocity;

    private Vector2 ReadInputLeftHand()
    {
        return _leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
    }
    
    private void OnEnable()
    {
        _leftHandMoveAction.EnableDirectAction();
    }

    private void OnDisable()
    {
        _leftHandMoveAction.DisableDirectAction();
    }
    protected void Update()
    {
        var xrOrigin = system.xrOrigin;
        if (xrOrigin == null)
            return;

        var input = ReadInputLeftHand();
        var translationInWorldSpace = ComputeDesiredMove(input);
        MoveRig(translationInWorldSpace);

    }

    // public void FixedUpdate()
    // {
    //     Quaternion headY = Quaternion.Euler(0, _xrOrigin.Camera.transform.eulerAngles.y, 0);
    //     Vector3 direction = headY * new Vector3(_inputAxis.x, 0, _inputAxis.y);
    //     Move(direction * (Time.fixedDeltaTime * speed));   
    //     
    // }

    private void Move(Vector3 direction)
    {
        _targetTransform.Translate(direction);
        
    }
    protected virtual Vector3 ComputeDesiredMove(Vector2 input)
        {
            if (input == Vector2.zero)
                return Vector3.zero;

            var xrOrigin = system.xrOrigin;
            if (xrOrigin == null)
                return Vector3.zero;
            var inputMove = Vector3.ClampMagnitude(new Vector3(input.x, 0f, input.y), 1f);
            var originTransform = xrOrigin.Origin.transform;
            var originUp = originTransform.up;
            
            var forwardSourceTransform = xrOrigin.Camera.transform;
            var inputForwardInWorldSpace = forwardSourceTransform.forward;
            if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(inputForwardInWorldSpace, originUp)), 1f))
            {
                inputForwardInWorldSpace = -forwardSourceTransform.up;
            }

            var inputForwardProjectedInWorldSpace = Vector3.ProjectOnPlane(inputForwardInWorldSpace, originUp);
            var forwardRotation = Quaternion.FromToRotation(originTransform.forward, inputForwardProjectedInWorldSpace);

            var translationInRigSpace = forwardRotation * inputMove * (speed * Time.fixedDeltaTime);
            var translationInWorldSpace = originTransform.TransformDirection(translationInRigSpace);
            return translationInWorldSpace;
        }
    
    protected virtual void MoveRig(Vector3 translationInWorldSpace)
    {
        var xrOrigin = system.xrOrigin;
        if (xrOrigin == null)
            return;

        var motion = translationInWorldSpace;

        if (_targetTransform != null)
        {
            // Step vertical velocity from gravity
            if (IsGrounded())
            {
                _verticalVelocity = Vector3.zero;
            }
            else
            {
                _verticalVelocity += Physics.gravity * Time.fixedDeltaTime;
            }

            motion += _verticalVelocity * Time.fixedDeltaTime;

            if (CanBeginLocomotion() && BeginLocomotion())
            {
                Move(motion);
                EndLocomotion();
            }
        }
        else
        {
            if (CanBeginLocomotion() && BeginLocomotion())
            {
                xrOrigin.Origin.transform.position += motion;

                EndLocomotion();
            }
        }
    }

    private bool IsGrounded()
    {
        return true;
    }
}

