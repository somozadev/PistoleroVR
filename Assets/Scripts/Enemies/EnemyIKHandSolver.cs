using System.Collections;
using General;
using UnityEngine;

namespace Enemies
{
    public class EnemyIKHandSolver : MonoBehaviour
    {
        [SerializeField] private Transform target;
        public bool animating;

        private void Start()
        {
            target = GameManager.Instance.players[0].PlayerMovement.CameraHolder.transform;
        }

        private void LateUpdate()
        {
            if (animating) return;
            if (!target) return;

            transform.LookAt(target);
            transform.Rotate(90, 0, 0);
        }



        public IEnumerator AttackAnim()
        {
            animating = true;
            float elapsedTime = 0f;
            var localRotation = transform.localRotation;
            while (elapsedTime < .3f)
            {
                elapsedTime += Time.deltaTime;
                transform.localRotation = Quaternion.Slerp(transform.localRotation,
                    Quaternion.Euler(15, localRotation.eulerAngles.y, localRotation.eulerAngles.z),
                    elapsedTime / 0.3f);
                yield return null;
            }

            elapsedTime = 0f;
            while (elapsedTime < .3f)
            {
                elapsedTime += Time.deltaTime;
                localRotation = Quaternion.Slerp(localRotation,
                    Quaternion.Euler(90, localRotation.eulerAngles.y, localRotation.eulerAngles.z), elapsedTime / 0.3f);
                transform.localRotation = localRotation;
                yield return null;
            }

            animating = false;
        }
    }
}