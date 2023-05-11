using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

namespace General
{
    [Serializable]
    public class PlayerData : MonoBehaviour
    {
        public int _economy;
        public int _kills;
        public int _runs;
        public bool[] _unlockedHats;


        public void Buy(int price)
        {
            if (_economy - price < 0)
                _economy = 0;
            else
                _economy -= price;
        }

        [ContextMenu("SaveData")]
        public async Task SaveData()
        {
            var data = new Dictionary<string, object>
                { { "Economy", _economy }, { "Kills", _kills }, { "Runs", _runs }, { "Hats", _unlockedHats } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        }

        public override string ToString()
        {
            return "Economy: " + _economy + "Kills: " + _kills + "Runs: " + _runs + "UnlockedHats: " + _unlockedHats;
        }

        [ContextMenu("LoadData")]
        public async Task LoadData()
        {
            var data = await CloudSaveService.Instance.Data.LoadAllAsync();

            Debug.Log("DATA: " + data);
            foreach (var (key, value) in data)
            {
                if (key == "Economy")
                    _economy = int.Parse(value);
                if (key == "Kills")
                    _kills = int.Parse(value);
                if (key == "Runs")
                    _runs = int.Parse(value);
                if (key == "Hats")
                    _unlockedHats = StringToBoolArray(value);
            }
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