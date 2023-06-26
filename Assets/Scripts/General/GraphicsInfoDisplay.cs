using TMPro;
using UnityEngine;

namespace General
{
    public class GraphicsInfoDisplay : MonoBehaviour
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

        private string GetString() => "Graphics: " + SystemInfo.graphicsDeviceType + " / " + SystemInfo.graphicsMemorySize + " Mb";
    }
}
