using System;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudServices
{
    public class CloudSaveManager : MonoBehaviour
    {
        public async void SaveToCloud(string userId, PlayerSaveData playerData)
        {
            var data = new Dictionary<string, object> { { userId, JsonUtility.ToJson(playerData) } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }

        public async Task<bool> CheckIfUserHasData(string userId)
        {
            bool userExists = false;
            List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();
            if (keys.Count <= 0)
                userExists = false;
            else
            {
                foreach (var key in keys)
                {
                    if (key == userId)
                        userExists = true;
                }
            }

            return userExists;
        }

        public async Task<PlayerSaveData> LoadFromCloud(string userId)
        {
            try
            {
                if (!await CheckIfUserHasData(userId))
                    SaveToCloud(userId, new PlayerSaveData(userId));
                Dictionary<string, string> data =
                    await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { userId });
                PlayerSaveData playerLoadedData = JsonUtility.FromJson<PlayerSaveData>(data[userId]);
                Debug.Log(
                    $"Player with authed id {userId} successfully retrieved it's saved data : {playerLoadedData.ToString()}");
                return playerLoadedData;
            }
            catch (Exception e)
            {
                return new PlayerSaveData();
            }
        }
    }
}