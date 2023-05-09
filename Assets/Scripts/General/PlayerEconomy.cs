using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEconomy : MonoBehaviour
{
   private int _currentEconomy;

   private async void Start()
   {
      GameManager.Instance.gameServices.playerSignedIn.AddListener(await CallServerToGetPlayerEconomy());
   }

   private void OnEnable()
   {
      
   }

   private void OnDisable()
   {
      throw new NotImplementedException();
   }

   private async Task<UnityAction> CallServerToGetPlayerEconomy()
   {
      var data = new Dictionary<string, object>{ {"playerEconomy", _currentEconomy} };
      await CloudSaveService.Instance.Data.ForceSaveAsync(data);
      return null;
   }
   [ContextMenu("LoadData")]
   private async Task LoadPlayerSaveData()
   {
      Dictionary<string, string> data = await CloudSaveService.Instance.Data.LoadAllAsync();

      foreach ((string key, string value) in data)
      {
         Debug.Log(key + ":" + value );
      }
   }
   private async Task SavePlayerSaveData()
   {
      
   }
}
