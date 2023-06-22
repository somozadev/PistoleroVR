using System;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace General.Services
{
    public class EconomyManager : MonoBehaviour
    {
        public string name;
        public float value;
        [FormerlySerializedAs("_lastAddedValue")] public float lastAddedValue;
        public static EconomyManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public async Task RefreshEconomyConfiguration()
        {
            // Calling SyncConfigurationAsync(), will update the cached configuration list (the lists of Currency,
            // Inventory Item, and Purchase definitions) with any definitions that have been published or changed by
            // Economy or overriden by Game Overrides since the last time the player's configuration was cached. It also
            // ensures that other services like Cloud Code are working with the same configuration that has been cached.
            await EconomyService.Instance.Configuration.SyncConfigurationAsync();
        }

        public async Task RefreshCurrencyBalances()
        {
            GetBalancesResult balanceResult = null;

            try
            {
                balanceResult = await GetEconomyBalances();
            }
            catch (EconomyRateLimitedException e)
            {
                balanceResult = await Utils.RetryEconomyFunction(GetEconomyBalances, e.RetryAfter);
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
            }

            if (this == null) return;

            SetBalances(balanceResult);
        }

        private void SetBalances(GetBalancesResult getBalancesResult)
        {
            if (getBalancesResult is null) return;

            var currenciesString = new StringBuilder();

            foreach (var balance in getBalancesResult.Balances)
            {
                if (balance.Balance > 0)
                {
                    currenciesString.Append($", {balance.CurrencyId}:{balance.Balance}");
                }

                name = balance.CurrencyId;
                value = balance.Balance;
            }

            if (currenciesString.Length > 0)
            {
                Debug.Log($"Currency balances updated. Value(s): {currenciesString.Remove(0, 2)}");
            }
            else
            {
                Debug.Log("Currency balances updated -- none found.");
            }
            EventManager.OnEconomyUpdated();
        }

        public static Task<GetBalancesResult> GetEconomyBalances()
        {
            var options = new GetBalancesOptions { ItemsPerFetch = 100 };
            return EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
        }

        void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}