using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyIKFootSolver : MonoBehaviour
    {
        [SerializeField] LayerMask terrainLayer;
        [SerializeField] Transform body;
        [SerializeField] EnemyIKFootSolver otherFoot;
        [SerializeField] private float speed = 1;
        [SerializeField] private float stepDistance = 4;
        [SerializeField] private float stepLength = 4;
        [SerializeField] private float stepHeight = 1;
        [SerializeField] private Vector3 footOffset;
        private float _footSpacing;
        private Vector3 _oldPosition, _currentPosition, _newPosition;
        private Vector3 _oldNormal, _currentNormal, _newNormal;
        private float _lerp;

        private void Start()
        {
            _footSpacing = transform.localPosition.x;
            _currentPosition = _newPosition = _oldPosition = transform.position;
            _currentNormal = _newNormal = _oldNormal = transform.up;
            _lerp = 1;
        }
        
        void Update()
        {
            transform.position = _currentPosition;
            transform.up = _currentNormal;

            Ray ray = new Ray(body.position + (body.right * _footSpacing), Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                if (Vector3.Distance(_newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && _lerp >= 1)
                {
                    _lerp = 0;
                    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(_newPosition).z
                        ? 1
                        : -1;
                    _newPosition = info.point + (body.forward * (stepLength * direction)) + footOffset;
                    _newNormal = info.normal;
                }
            }

            if (_lerp < 1)
            {
                Vector3 tempPosition = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
                tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * stepHeight;

                _currentPosition = tempPosition;
                _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
                _lerp += Time.deltaTime * speed;
            }
            else
            {
                _oldPosition = _newPosition;
                _oldNormal = _newNormal;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_newPosition, 0.05f);
        }


        private bool IsMoving()
        {
            return _lerp < 1;
        }
    }
}