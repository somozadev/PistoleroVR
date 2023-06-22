using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

namespace General
{
    public class TimeManager : MonoBehaviour
    {
        public bool rewardClaimed;

        private bool allThingsLoaded = false;

        private DateTime lastDateTime;
        [SerializeField] private float _timePassedNeeded = 24f;
        private ICloudSaveDataClient data;

        public string remainingTime;

        private void OnEnable()
        {
            EventManager.RewardClaimed += RewardClaimed;
        }

        private void OnDisable()
        {
            EventManager.RewardClaimed -= RewardClaimed;
        }

        private void RewardClaimed()
        {
            rewardClaimed = true;
            lastDateTime = DateTime.Now;
        }

        public async Task StartTimer()
        {
            data = CloudSaveService.Instance.Data;
            string rewardClaimedString = await LoadFromCloud(data, "RewardClaimed");
            rewardClaimed = string.IsNullOrEmpty(rewardClaimedString) ||
                            !bool.TryParse(rewardClaimedString, out var parsedRewardClaimed) || parsedRewardClaimed;

            string lastDateTimeString = await LoadFromCloud(data, "LastDateTime");
            lastDateTime = !string.IsNullOrEmpty(lastDateTimeString)
                ? DateTime.Parse(lastDateTimeString)
                : DateTime.Now;

            Debug.Log($"<color=green>REWARD CLAIMED IS {rewardClaimed}</color>");


            EventManager.OnTimerStarted();
            allThingsLoaded = true;
        }

        private async void OnApplicationQuit()
        {
            if (!rewardClaimed)
                await SaveToCloud(data, "LastDateTime", DateTime.Now.ToString());
            await SaveToCloud(data, "RewardClaimed", rewardClaimed.ToString());
        }

        private async void Update()
        {
            if (!allThingsLoaded) return;


            if (!rewardClaimed)
                return;

            //cuando rewardClaimed se ponga a true, a las 15:00 del 22/06/23 guardar ese date en lastDateTime. contar 24 horas y mostrar el tiempo restante en la variable remainingTime. 
            //si el usuario cierra la app en ese tiempo, si le rewardClaimed es true, guardar el valor de lastDateTime
            var timePassed = DateTime.Now - lastDateTime;
            var hoursPassed = (float)timePassed.TotalHours;
            if (hoursPassed >= _timePassedNeeded)
            {
                rewardClaimed = false;
                await SaveToCloud(data, "LastDateTime", DateTime.Now.ToString());
                await SaveToCloud(data, "RewardClaimed", rewardClaimed.ToString());
                EventManager.OnTimerEnded();
                //update Text here to now can reward! 
            }
            else
            {
                remainingTime =
                    TimeSpan.FromHours(_timePassedNeeded).Subtract(timePassed)
                        .ToString(@"hh\:mm\:ss"); //update text to remaining time
                rewardClaimed = true;
            }
        }

        private async Task<string> LoadFromCloud(ICloudSaveDataClient data, string key)
        {
            var query = await data.LoadAsync(new HashSet<string> { key });
            query.TryGetValue(key, out var value);
            return value;
        }


        private async Task SaveToCloud(ICloudSaveDataClient data, string key, string value)
        {
            var tosavedata = new Dictionary<string, object> { { key, value } };
            await data.ForceSaveAsync(tosavedata);
        }
    }
}