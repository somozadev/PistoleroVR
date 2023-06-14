using System.Collections.Generic;
using UnityEngine;

namespace Hordes.Formations
{
    public abstract class FormationBase : MonoBehaviour
    {
        [SerializeField] [Range(0, 1)] protected float noise = 0;
        [SerializeField] protected float spread = 4;
        public abstract IEnumerable<Vector3> EvaluatePoints();

        public Vector3 GetNoise(Vector3 pos)
        {
            var pNoise = Mathf.PerlinNoise(pos.x * noise, pos.z * noise);

            return new Vector3(pNoise, 0, noise);
        }
    }
}