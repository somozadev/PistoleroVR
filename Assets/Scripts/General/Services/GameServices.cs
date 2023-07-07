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
            // await Task.Delay(1000);
            timeManager = GetComponent<TimeManager>();
            if (playerData == null)
                playerData = FindObjectOfType<PlayerData>();
            await UnityServices.InitializeAsync();
            await SignInAnon();
            SetupEvents();
            await playerData.LoadData();
            await timeManager.StartTimer();
         }

        private void SetupEvents()
        {
            AuthenticationService.Instance.SignedIn += () => { _playerId = AuthenticationService.Instance.PlayerId; };
            AuthenticationService.Instance.SignInFailed += (err) => { Debug.Log(err.ToString()); };
            AuthenticationService.Instance.SignedOut += () => { _playerId = ""; };
            AuthenticationService.Instance.Expired += () => { };
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
                GameManager.Instance.players[0].GetComponent<PlayerMenuCanvas>().playerIdText.text = "playerId: "+ _playerId;
                // Debug.Log($"Player id:{AuthenticationService.Instance.PlayerId}");
                await EconomyManager.Instance.RefreshEconomyConfiguration();
                if (this == null) return;
                await EconomyManager.Instance.RefreshCurrencyBalances();
                if (this == null) return;
                // Debug.Log("Initialization and signin complete.");
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
        }

        public async Task CallToSubstractEconomyToPlayer(int amount)
        {
            try
            {
                await CloudCodeManager.Instance.CallSubstractMoneyFromPlayerEndpoint(amount);
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
        }

        public async Task CallToAddEconomyToPlayer(int amount)
        {
            try
            {
                await CloudCodeManager.Instance.CallAddMoneyFromPlayerEndpoint(amount);
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
        }
    }
}