using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using General.Sound;
using Unity.Services.CloudSave;
using UnityEngine;

namespace General
{
    [Serializable]
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private PlayerIngameCanvas _ingameCanvas;


        public int _economy; //not saved economy currency but ingame economy (0 each new run)
        public int _kills;
        public int _runs;
        public bool[] _unlockedHats = new bool[4];
        public int _selectedHat;

        public void ResetEconomy()
        {
            _economy = 0;
        }




        [SerializeField] private bool _doubleXp;

        private void Awake()
        {
            _ingameCanvas = GetComponentInChildren<PlayerIngameCanvas>();
        }

        public void SetDoubleXP(bool doubleXp)
        {
            _doubleXp = doubleXp;
        }

        public void Buy(int price)
        {
            if (_economy - price < 0)
                _economy = 0;
            else
                _economy -= price;

            AudioManager.Instance.PlayOneShot("Buy");
            _ingameCanvas.UpdateEconomy(_economy);
        }

        public void Gain(int amount)
        {
            if (_doubleXp)
                amount *= 2;
            _economy += amount;
            _ingameCanvas.UpdateEconomy(_economy);
        }

        public void AddKill()
        {
            _kills++;
        }

        public async void AddRun()
        {
            _runs++;
            EventManager.OnPlayerDataUpdated();
            await SaveData();
        }

        public async void UnlockHat(int price, int id)
        {
            _unlockedHats[id] = true;
            await GameManager.Instance.gameServices.CallToSubstractEconomyToPlayer(price);
            await SaveData();
        }

        public async void SetSelectHat(int id)
        {
            _selectedHat = id;
            UpdateHatSelected();
            await SaveData();
            EventManager.OnPlayerDataLoaded();
        }

        public void UpdateHatSelected()
        {
            GetComponent<CharacterCustomization>().SetHatWithIndex(_selectedHat);
        }

        private async void OnApplicationQuit()
        {
            await SaveData();
        }

        private async void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                await SaveData();
        }

        public override string ToString()
        {
            return "Kills: " + _kills + "Runs: " + _runs + "UnlockedHats: " + _unlockedHats;
        }

        private async void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                await SaveData();
        }

        public async Task SaveData()
        {
            _unlockedHats ??= new bool[] { true, false, false, false };
            var data = new Dictionary<string, object>
                { { "Kills", _kills }, { "Runs", _runs }, { "Hats", _unlockedHats }, { "SelectedHat", _selectedHat } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }

        public async Task LoadData()
        {
            var data = await CloudSaveService.Instance.Data.LoadAllAsync();

            // Debug.Log("DATA: " + data);
            foreach (var (key, value) in data)
            {
                if (key == "Kills")
                    _kills = int.Parse(value);
                if (key == "Runs")
                    _runs = int.Parse(value);
                if (key == "Hats")
                    _unlockedHats = StringToBoolArray(value);
                if (key == "SelectedHat")
                    _selectedHat = int.Parse(value);
            }

            UpdateHatSelected();
            EventManager.OnPlayerDataLoaded();
        }

        private bool[] StringToBoolArray(string str)
        {
            var strArr = str.Trim('[', ']').Split(',');

            var boolArr = new bool[strArr.Length];
            for (var i = 0; i < strArr.Length; i++) boolArr[i] = bool.Parse(strArr[i]);

            return boolArr;
        }
    }
}