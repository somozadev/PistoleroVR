using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hordes.Formations
{
    public class FormationBox : FormationBase
    {
        [SerializeField] private int _unitWidth = 5;
        [SerializeField] private int _unitDepth = 5;
        [SerializeField] private bool _hollow = false;
        [SerializeField] private float _nthOffset = 0;

        [SerializeField] private Vector2 rndInSphere;

        private void Awake()
        {
            rndInSphere = Random.insideUnitCircle;
        }

        public override IEnumerable<Vector3> EvaluatePoints()
        {
            var middleOffset = new Vector3(_unitWidth * 0.5f, 0, _unitDepth * 0.5f);

            for (var x = 0; x < _unitWidth; x++)
            {
                for (var z = 0; z < _unitDepth; z++)
                {
                    if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1) continue;
                    var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);
                    
                    pos -= middleOffset;

                    pos += GetNoise(pos);
                    pos *= spread;

                    yield return pos;
                }
            }
        }


        private Vector3 AddRandom(Vector3 pos)
        {
            pos = new Vector3(pos.x * rndInSphere.x, pos.y, pos.z * rndInSphere.y);
            return pos;
        }
    }
}