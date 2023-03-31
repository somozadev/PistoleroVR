using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using General;

namespace Multiplayer
{
    public class GameServices : MonoBehaviour
    {
        public string _playerId;

        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
        }

        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                GameManager.Instance.eventManager.InvokeOnAuthCompleted();
                _playerId = AuthenticationService.Instance.PlayerId;
            };
            AuthenticationService.Instance.SignInFailed += (err) =>
            {
                GameManager.Instance.eventManager.InvokeOnAuthFailed();
                Debug.Log(err.ToString());
            };
            AuthenticationService.Instance.SignedOut += () => { _playerId = ""; };
            AuthenticationService.Instance.Expired += () => { Debug.Log("Session expired"); };
        }

        public async Task SignInAnon()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                //save user ID locally to check on new start if that user exists (to popup another login type)
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}