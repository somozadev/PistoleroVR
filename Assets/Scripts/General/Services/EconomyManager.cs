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
        [FormerlySerializedAs("name")] public string key;
        public float value;
       public float lastAddedValue;
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

                key = balance.CurrencyId;
                value = balance.Balance;
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