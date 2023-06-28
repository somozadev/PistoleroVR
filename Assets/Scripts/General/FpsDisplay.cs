using TMPro;
using UnityEngine;

namespace General
{
    public class FpsDisplay : MonoBehaviour
    {
        [SerializeField] private float pollingTime = 1f;
        private float time;
        private int frameCount;
        public TMP_Text display;

        private void Update()
        {
            time += Time.unscaledDeltaTime;
            frameCount++;
            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                display.text = frameRate.ToString() + " fps";

                time -= pollingTime;
                frameCount = 0;
            }
            
        }
    }
}