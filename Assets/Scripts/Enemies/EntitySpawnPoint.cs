using System.Collections;
using General;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EntitySpawnPoint : MonoBehaviour
    {
        [SerializeField] private bool _used;
        [SerializeField] private float _waitToCheckTime = 0.5f;
        private GameObject _instantiatedGo;
        [SerializeField] private float SpawnRange = 2f;


        public GameObject UseToInstantiate(ObjectPooling pool)
        {
            //TODO: CHANGE TO USE POOLING
            _instantiatedGo = pool.GetPooledElement();
            _instantiatedGo.transform.position = transform.position;

            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            int vertexIndex = Random.Range(0, triangulation.vertices.Length);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, SpawnRange, 0))
            {
                _instantiatedGo.GetComponent<NavMeshAgent>().Warp(hit.position);
                _instantiatedGo.GetComponent<NavMeshAgent>().enabled = true;
            }

            else
            {
                _instantiatedGo.GetComponent<NavMeshAgent>().Warp(transform.position);
            }

            // _instantiatedGo.GetComponent<NavMeshAgent>().nextPosition = transform.position;
            // Debug.LogError($"El {_instantiatedGo.name} con posicion {_instantiatedGo.transform.position} se setea a {transform.position} o {transform.localPosition} ? ");
            _instantiatedGo.GetComponent<BT.Entity>().ResetForNewUse();
            _used = true;
            StartCoroutine(WaitInstantiatedToLeave());
            return _instantiatedGo;
        }

        private IEnumerator WaitInstantiatedToLeave()
        {
            while (Vector3.Distance(_instantiatedGo.transform.position, transform.position) <= SpawnRange)
                yield return new WaitForSeconds(_waitToCheckTime);
            _used = false;
            yield return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, SpawnRange);
        }
    }
}