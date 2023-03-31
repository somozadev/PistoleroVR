using Unity.Netcode;
using Unity.Netcode.Components;

namespace Multiplayer
{
    public class ClientNetworkTransform : NetworkTransform
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            CanCommitToTransform = IsOwner;
        }

        protected override void Update()
        {
            base.Update();
            if (NetworkManager.Singleton != null && (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost))
            {
                CanCommitToTransform = IsOwner;
                if (CanCommitToTransform)
                {
                    TryCommitTransformToServer(transform,NetworkManager.LocalTime.Time);
                }
            }
        }
    }
}