using UnityEngine;

namespace Enemies
{
    public class EnemyIKHandSolver : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        private void Start()
        {
            target = GameManager.Instance.players[0].PlayerMovement.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(target);
            transform.Rotate( 90, 0, 0 );
        }
    }
}