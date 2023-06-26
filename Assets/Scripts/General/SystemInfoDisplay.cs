using TMPro;
using UnityEngine;

namespace General
{
    public class SystemInfoDisplay : MonoBehaviour
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

        private string GetString() => "Os: " + SystemInfo.operatingSystem + " / " + SystemInfo.processorFrequency + " MHz";
    }
}
