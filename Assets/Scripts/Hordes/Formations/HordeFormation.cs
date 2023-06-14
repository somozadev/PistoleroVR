using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hordes.Formations
{
    [RequireComponent(typeof(FormationBase))]
    public class HordeFormation : MonoBehaviour
    {
        [SerializeField] private MovementType _movementType;
        private FormationBase _formation;
        private GameObject _unitPrefab;
        private float _unitSpeed = 2;
        private readonly List<GameObject> _spawnedUnits = new List<GameObject>();
        private List<Vector3> _points = new List<Vector3>();

        public void Init(GameObject unitPrefab, float unitSpeed)
        {
            _formation = GetComponent<FormationBase>();
            _unitPrefab = unitPrefab;
            _unitSpeed = unitSpeed;
        }

        private void Update()
        {
            SetFormation();
        }

        private void SetFormation()
        {
            _points = _formation.EvaluatePoints().ToList();
            SpawnIfNeeded();
            KillIfNeeded();
            Move(GameObject.Find("target").transform.position);
        }


        private void SpawnIfNeeded()
        {
            if (_points.Count <= _spawnedUnits.Count) return;
            var remainingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remainingPoints);
        }

        private void KillIfNeeded()
        {
            if (_points.Count < _spawnedUnits.Count)
            {
                Kill(_spawnedUnits.Count - _points.Count);
            }
        }


        private void Move(Vector3 target)
        {
            switch (_movementType)
            {
                case MovementType.FORMATION:
                    MoveUnits();
                    break;
                case MovementType.INDIVIDUAL:
                    MoveUnitsToTarget(target);
                    break;
                case MovementType.GROUP:
                    MoveHordeToTarget(target);
                    break;
            }
        }

        private void MoveUnits()
        {
            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position,
                    transform.position + _points[i], _unitSpeed * Time.deltaTime);
            }
        }
        private void MoveHordeToTarget(Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target,
                _unitSpeed * Time.deltaTime);
        }
        private void MoveUnitsToTarget(Vector3 target)
        {
            for (var i = 0; i < _spawnedUnits.Count; i++)
            {
                _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, target,
                    _unitSpeed * Time.deltaTime);
            }
        }

        private void Spawn(IEnumerable<Vector3> points)
        {
            foreach (var pos in points)
            {
                var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, transform);
                _spawnedUnits.Add(unit);
            }
        }

        private void Kill(int num)
        {
            for (var i = 0; i < num; i++)
            {
                var unit = _spawnedUnits.Last();
                _spawnedUnits.Remove(unit);
                Destroy(unit.gameObject);
            }
        }
    }

    enum MovementType
    {
        FORMATION,
        INDIVIDUAL,
        GROUP
    }
}