using TMPro;
using UnityEngine;

namespace General.UI_StartScene
{
    public class UIController_StartScene_Stats : MonoBehaviour
    {
        [SerializeField] private TMP_Text _runsCounter;
        [SerializeField] private TMP_Text _killsCounter;

        private void OnEnable()
        {
            EventManager.PlayerDataLoaded += RefreshStats;
            EventManager.PlayerDataUpdated += RefreshStats;

            if (GameManager.Instance.players[0].PlayerData != null)
                RefreshStats();
        }

        private void OnDisable()
        {
            EventManager.PlayerDataLoaded += RefreshStats;
            EventManager.PlayerDataUpdated -= RefreshStats;
        }

        private void RefreshStats()
        {
            _killsCounter.text = $"{GameManager.Instance.players[0].PlayerData._kills}";
            _runsCounter.text = $"{GameManager.Instance.players[0].PlayerData._runs}";
        }
    }
}