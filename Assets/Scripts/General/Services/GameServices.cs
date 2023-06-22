using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace General.Services
{
    public class GameServices : MonoBehaviour
    {
        public string _playerId;
        public PlayerData playerData;
        private TimeManager timeManager;

        private async void Awake()
        {
            timeManager = GetComponent<TimeManager>();
            if (playerData == null)
                playerData = FindObjectOfType<PlayerData>();
            await UnityServices.InitializeAsync();
            await SignInAnon();
            SetupEvents();
            await playerData.LoadData();await timeManager.StartTimer();
        }

        // private async void Start()
        // {
        //     await UnityServices.InitializeAsync();
        //     await timeManager.StartTimer();
        // }

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
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (this == null) return;
                }

                _playerId = AuthenticationService.Instance.PlayerId;
                Debug.Log($"Player id:{AuthenticationService.Instance.PlayerId}");

                await EconomyManager.Instance.RefreshEconomyConfiguration();
                if (this == null) return;
                await EconomyManager.Instance.RefreshCurrencyBalances();
                if (this == null) return;
                Debug.Log("Initialization and signin complete.");
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [ContextMenu("GET RANDOM NEW LOOTBOX")]
        public async Task GrantRandomCurrency()
        {
            try
            {
                // Call Cloud Code js script and wait for grant to complete.
                await CloudCodeManager.Instance.CallGrantRandomCurrencyEndpoint();
                if (this == null) return;

                await EconomyManager.Instance.RefreshCurrencyBalances();
            }
            catch (CloudCodeResultUnavailableException)
            {
                // Exception already handled by CloudCodeManager
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            // finally
            // {
            //     if (this != null)
            //     {
            //         sceneView.SetInteractable();
            //     }
            // }
        }
    }

    class CloudCodeResponse
    {
        public string welcomeMessage;
    }
}