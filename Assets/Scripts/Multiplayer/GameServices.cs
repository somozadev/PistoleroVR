using System;
using UnityEngine;
using Unity.Services.Core;

namespace Multiplayer
{
    public class GameServices : MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();
            //shall signin with unityauth
        }
    }
}
