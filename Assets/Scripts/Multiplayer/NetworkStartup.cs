// using System;
// // using Unity.Netcode;
// using UnityEngine;
//
// public class NetworkStartup : MonoBehaviour
// {
//     [SerializeField] private bool _initializeAsHost;
//
//     private void Awake()
//     {
//         _initializeAsHost = GameManager.Instance.IsHost;
//     }
//
//     void Start()
//     {
//         if (_initializeAsHost) //SceneTransitionHandler.Instance.InitializeAsHost
//             NetworkManager.Singleton.StartHost();
//         else
//             NetworkManager.Singleton.StartClient();
//     }
// }
