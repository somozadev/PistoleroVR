using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private MovementVR _movementVR;
        [SerializeField] private RigVR _rigVR;
        [SerializeField] private CharacterCustomization _characterCustomization;
        [SerializeField] private ActionBasedController _leftHand;
        [SerializeField] private ActionBasedController _rightHand;
    
    }
}
