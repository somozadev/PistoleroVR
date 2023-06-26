using General;
using UnityEngine;

namespace Enemies
{
    public class EnemyIKHandSolver : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Start()
        {
            target = GameManager.Instance.players[0].PlayerMovement.CameraHolder.transform;
        }

        private void LateUpdate()
        {
            if (!target) return;

            transform.LookAt(target);
            transform.Rotate(90, 0, 0);
        }
    }
}