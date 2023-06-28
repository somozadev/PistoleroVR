using System.Collections;
using General;
using UnityEngine;

namespace VR.Powerups
{
    public class DoublePoints : PowerUp
    {
        [SerializeField] private float _doublePointsTime = 10f;


        protected override void PerformPowerupAction()
        {
            GameManager.Instance.players[0].PlayerData.SetDoubleXP(true);
            visuals.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(WaitToBackToNormal());
        }

        private IEnumerator WaitToBackToNormal()
        {
            float elapsed_time = 0f;
            while (elapsed_time < _doublePointsTime)
            {
                elapsed_time += Time.deltaTime;

                yield return null;
            }

            GameManager.Instance.players[0].PlayerData.SetDoubleXP(false);
            Destroy(gameObject);
        }
    }
}