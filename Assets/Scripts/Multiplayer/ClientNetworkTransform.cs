// using Unity.Netcode;
// using Unity.Netcode.Components;
// using UnityEngine;
//
// namespace Multiplayer
// {
//     [DisallowMultipleComponent]
//     public class ClientNetworkTransform : NetworkTransform
//     {
//         public override void OnNetworkSpawn()
//         {
//             base.OnNetworkSpawn();
//             CanCommitToTransform = IsOwner;
//         }
//         protected override bool OnIsServerAuthoritative()
//         {
//             return false;
//         }
//         protected override void Update()
//         {
//             base.Update();
//             if (NetworkManager.Singleton != null && (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost))
//             {
//                 CanCommitToTransform = IsOwner;
//                 if (CanCommitToTransform)
//                 {
//                     TryCommitTransformToServer(transform,NetworkManager.LocalTime.Time);
//                 }
//             }
//         }
//     }
// }