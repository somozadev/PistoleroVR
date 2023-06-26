using System;
using System.Collections;
using System.Threading.Tasks;
using General;
using General.Sound;
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
        [SerializeField] private EntityRagdoll _ragdoll;
        [SerializeField] private EntityHealth _health;
        [SerializeField] private EntitySounder _sounder;

        [SerializeField] private EntitiesManager _entitiesManager;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float _attackRange;
        [SerializeField] private int _attackDamage = 15;

        private PlayerHealth _targetHealth;
        private PlayerData _targetData;
        private int _gainAmount = 15;
        public float AttackRange => _attackRange;
        public EntityHealth EntityHealth => _health;

        public void SetGainAmount(int value)
        {
            _gainAmount = value;
        }

        public void AssignWavesManager(EntitiesManager entitiesManager)
        {
            _entitiesManager = entitiesManager;
        }

        private void Start()
        {
            _health = GetComponent<EntityHealth>();
            _ragdoll = GetComponent<EntityRagdoll>();
            _sounder = GetComponent<EntitySounder>();
            agent = GetComponent<NavMeshAgent>();

            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.players[0].PlayerMovement.transform != null)
                    target = GameManager.Instance.players[0].PlayerMovement.transform;
            }
            else
            {
                target = GetComponentInParent<WavesManager>().target;
            }

            _targetHealth = target.GetComponentInParent<PlayerHealth>();
            _targetData = target.GetComponentInParent<PlayerData>();

            if (_entitiesManager == null)
                _entitiesManager = FindObjectOfType<EntitiesManager>();
            _tree.Init(target, agent, _entitiesManager);
        }

        private void LateUpdate()
        {
            updateTickTimer += Time.deltaTime;
            if (updateTickTimer < updateTickInterval) return;
            _tree.Tick();
            updateTickTimer = 0f;
        }

        public async void AnimateAttack()
        {
            await Task.Delay(1000);
            Debug.Log("<color='cyan'> ANIMATION ATTACK TRIGGERED </color>");
        }

        public void Attack()
        {
            _targetHealth.Damage(_attackDamage);
            _sounder.PlayAttackSound();
            Debug.Log("<color='green'> ATTACK TRIGGERED </color>");
        }

        public void Die()
        {
            _targetData.Gain(_gainAmount);
            _sounder.PlayDeadSound();
            _entitiesManager.Entities.Remove(this);
            _entitiesManager.CheckIfNoMoreEntities();
            _tree.StopTree();
            agent.ResetPath();
            _ragdoll.ActivateRagdoll();
            _sounder.StopSounding();
            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            float elapsedTime = 0f;
            //maybe trigger some visuals (fade through ground / fade material smth)
            while (elapsedTime < _health.DieTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
            _ragdoll.DeactivateRagdoll();
        }

        public void ResetForNewUse()
        {
            //set hp
            _sounder.StopSounding();
            updateTickTimer = 0f;
            _tree.StartTree();
        }

        private void OnEnable()
        {
            _health.ResetHealth();
            _sounder.StartSounding();
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