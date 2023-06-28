using System.Collections;
using General;
using UnityEngine;

namespace VR.Powerups
{
    public class Inmortal : PowerUp
    {
        [SerializeField] private float _inmortalTime = 10f;

        protected override void PerformPowerupAction()
        {
            GameManager.Instance.players[0].PlayerHealth.SetInmortal(true);
            visuals.SetActive(false);
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(WaitToBackToNormal());
        }

        private IEnumerator WaitToBackToNormal()
        {
            float elapsed_time = 0f;
            while (elapsed_time < _inmortalTime)
            {
                elapsed_time += Time.deltaTime;

                yield return null;
            }

            GameManager.Instance.players[0].PlayerHealth.SetInmortal(false);
            Destroy(gameObject);
        }
    }
}