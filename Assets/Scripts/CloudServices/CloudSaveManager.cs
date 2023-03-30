using Unity.Services.CloudSave;
using UnityEngine;
using System.Collections.Generic;

namespace CloudServices
{
    public class CloudSaveManager : MonoBehaviour
    {
        public async void SaveToCloud(string userId, PlayerSaveData playerData)
        {
            playerData = new PlayerSaveData(userId, new bool[] { true, false, false, false, false });
            var data = new Dictionary<string, object> { { userId, JsonUtility.ToJson(playerData) } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);

        }
    }
}