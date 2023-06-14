using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Hordes
{
    public class Horde : MonoBehaviour
    {
        [SerializeField] private int maxEntities = 10;
        [SerializeField] private int currentEntities = 0;

        [SerializeField] private List<GameObject> entities;
        [SerializeField] private float _hordeRange = 10f;
        [SerializeField] private float _hordePlayerDetectionRange = 18f;
        [Range(0, 1)] [SerializeField] private float _groupPercentage = 0.6f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private GameObject _target;

        private Dictionary<GameObject, Vector3> _randomPosInHordeEntityDict;
        public NavMeshAgent Agent => _agent;
        public GameObject Target => _target;

        private void Awake()
        {
            _randomPosInHordeEntityDict = new Dictionary<GameObject, Vector3>();
        }

        public void IncrementMaxEntities(int newMax)
        {
            maxEntities = newMax;
            currentEntities = 0;
            entities.Clear();
        }

        public void AddEntity(GameObject entity)
        {
            if (!HasSpace()) return;
            entities.Add(entity);
            _randomPosInHordeEntityDict.Add(entity, GetRandomInHordeArea());
            currentEntities++;
        }

        private Vector3 GetRandomInHordeArea()
        {
            Vector3 randomPos = UnityEngine.Random.insideUnitSphere * _hordeRange;
            randomPos = new Vector3(randomPos.x, transform.position.y, randomPos.z);
            randomPos = transform.TransformPoint(randomPos);

            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cylinder), randomPos, Quaternion.identity);
            
            return randomPos;
        }

        public bool IsHordeGrouped()
        {
            bool grouped = false;
            int groupedAmount = 0;
            foreach (var entity in entities)
                if (Vector3.Distance(entity.transform.position, transform.position) < _hordeRange)
                {
                    groupedAmount++;
                }

            if (GroupedPercentaje(groupedAmount) >= _groupPercentage)
                grouped = true;

            return grouped;
        }

        public void MoveGroupAlong()
        {
            foreach (var entity in entities)
            {
                entity.GetComponent<NavMeshAgent>()
                    .SetDestination(transform.position);
            }
        }


        private void ReAsignDestinations()
        {
            Dictionary<GameObject, Vector3> updatedDict = new Dictionary<GameObject, Vector3>();

            foreach (var kvp in _randomPosInHordeEntityDict)
            {
                var value = GetRandomInHordeArea();
                var key = kvp.Key;
                updatedDict[key] = value;
            }

            _randomPosInHordeEntityDict = updatedDict;
        }

        private bool DestinationsInRange()
        {
            bool inRange = true;
            foreach (var kvp in _randomPosInHordeEntityDict)
            {
                if (Vector3.Distance(kvp.Value, transform.position) > _hordeRange)
                    inRange = false;
            }

            return inRange;
        }

        public void TryGroupHorde()
        {
            Debug.Log("DestinationsInRange():  " + DestinationsInRange());
            if (!DestinationsInRange())
                ReAsignDestinations();
            foreach (var entity in entities)
            {
                if (Vector3.Distance(entity.transform.position, transform.position) > _hordeRange)
                {
                    entity.GetComponent<NavMeshAgent>().SetDestination(_randomPosInHordeEntityDict[entity]);
                }
            }
        }

        public float FilledPercentaje() => (currentEntities / maxEntities);
        public float GroupedPercentaje(int groupedAmount) => (groupedAmount / maxEntities);
        public bool HasSpace() => currentEntities < maxEntities;
        public bool HasEntities() => currentEntities <= 0;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _hordeRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _hordePlayerDetectionRange);
        }
    }
}