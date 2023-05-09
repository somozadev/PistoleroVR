using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using VR;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private Vector2 placementArea = new Vector2(-10f, 10f);

        public override void OnNetworkSpawn()
        {
            DisableClientInput();
        }

        private void Start()
        {
            if (IsClient && IsOwner)
            {
                transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), transform.position.y,
                    Random.Range(placementArea.x, placementArea.y));
            }
        }

        public void OnItemSelected(SelectEnterEventArgs eventArgs)
        {
            if (IsClient && IsOwner)
            {
                NetworkObject networkObject = eventArgs.interactorObject.transform.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    RequestItemOwnershipServerRpc(OwnerClientId, networkObject);
                }
            }
        }

        [ServerRpc]
        public void RequestItemOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
        {
            if (networkObjectReference.TryGet(out NetworkObject networkObject))
            {
                networkObject.ChangeOwnership(newOwnerClientId);
            }
            else
            {
                Debug.Log($"Unable to change ownership for clientId {newOwnerClientId}");
            }
        }
        private void DisableClientInput()
        {
            if (IsClient && !IsOwner)
            {
                var multiplayerMovementVR = GetComponentInChildren<MultiplayerMovementVR>();
                var clientControllers = GetComponentsInChildren<ActionBasedController>();
                var clientHead = GetComponentInChildren<TrackedPoseDriver>();
                var clientCamera = GetComponentInChildren<Camera>();
                
                
                clientCamera.enabled = false;
                multiplayerMovementVR.EnableInputActions = false;
                clientHead.enabled = false;
                foreach (var clientController in clientControllers)
                {
                    clientController.enableInputActions = false;
                    clientController.enableInputTracking = false;
                    clientController.GetComponent<LineVisualRendererManager>().DisableIfNotOwner();
                }
            }
        }
    }
}