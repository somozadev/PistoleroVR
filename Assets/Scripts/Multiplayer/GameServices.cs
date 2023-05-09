using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.Events;

namespace Multiplayer
{
    public class GameServices : MonoBehaviour
    {
        public string _playerId;
        public UnityEvent playerSignedIn;
        private void Awake()
        {
            GameManager.Instance.onGameManagerLoaded.AddListener(async () =>
            {   Debug.Log("Waiting to init unity services");
                await UnityServices.InitializeAsync();
                SetupEvents();
                await SignInAnon();
            });
        }

        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () => { _playerId = AuthenticationService.Instance.PlayerId; };
            AuthenticationService.Instance.SignInFailed += (err) => { Debug.Log(err.ToString()); };
            AuthenticationService.Instance.SignedOut += () => { _playerId = ""; };
            AuthenticationService.Instance.Expired += () => { Debug.Log("Session expired"); };
        }

        public async Task SignInAnon()
        {
            try
            {
                Debug.Log("Player signed in anon with " + _playerId);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                playerSignedIn?.Invoke();
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