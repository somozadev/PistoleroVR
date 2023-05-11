using System;
using System.Threading.Tasks;
using General;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace Multiplayer
{
    public class GameServices : MonoBehaviour
    {
        public string _playerId;
        public PlayerData playerData;

        private async void Awake()
        {
            if (playerData == null)
                playerData = FindObjectOfType<PlayerData>();
            await UnityServices.InitializeAsync();
            SetupEvents();
            await SignInAnon();
            await playerData.LoadData();
        }

        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                _playerId = AuthenticationService.Instance.PlayerId;
            };
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