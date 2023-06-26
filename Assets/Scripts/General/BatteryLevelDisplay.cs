using TMPro;
using UnityEngine;

namespace General
{
    public class BatteryLevelDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _display;

        private void Awake()
        {
            _display.text = GetString();
        }

        private void OnEnable()
        {
            _display.text = GetString();
        }

        private string GetString() => "Battery: " + (SystemInfo.batteryLevel * 100).ToString("0.00") + " / " + SystemInfo.batteryStatus;
    }
}