using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemies.BT
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private EntityBT _tree = new EntityBT();
        [SerializeField] private float updateTickInterval = 0.5f;
        [SerializeField] private float updateTickTimer = 0f;

        [SerializeField] private Transform target;
        [SerializeField] private EntitiesManager _entitiesManager;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float _attackRange;

        public float AttackRange => _attackRange;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.players[0].PlayerMovement.transform != null)
                    target = GameManager.Instance.players[0].PlayerMovement.transform;
            }
            else
            {
                target = GetComponentInParent<WavesManager>().target;
            }

            _entitiesManager = GetComponentInParent<EntitiesManager>();
            agent = GetComponent<NavMeshAgent>();
            _tree.Init(target, agent, _entitiesManager);
        }

        private void LateUpdate()
        {
            updateTickTimer += Time.deltaTime;
            if (updateTickTimer < updateTickInterval) return;
            _tree.Tick();
            updateTickTimer = 0f;
        }

        public void AnimateAttack()
        {
            Debug.Log("<color='cyan'> ANIMATION ATTACK TRIGGERED </color>");
        }

        public void Attack()
        {
            Debug.Log("<color='green'> ATTACK TRIGGERED </color>");
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _entitiesManager.JoinRange);
        }
    }
}