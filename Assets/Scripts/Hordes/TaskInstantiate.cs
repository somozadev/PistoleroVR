using BehaviourTree;
using Hordes.Spawners;
using UnityEngine;

namespace Hordes
{
    public class TaskInstantiate : Node
    {
        private GameObject _instantiatedEnemy = null;
        private float _elapsedTime = 0f;
        private float _waitTime;

        public TaskInstantiate(float waitTime)
        {
            name = "TaskInstantiate";
            _waitTime = waitTime;
        }

        public override NodeState Evaluate()
        {
            var spt = (HordeSpawnPoint)GetData("selectedSpawnPoint");
            var horde = (Horde)GetData("currentHorde");
            var wavesManager = (WavesManager)GetData("wavesManager");
            if (ReferenceEquals(spt, null))
            {
                state = NodeState.FAILURE;
                return state;
            }

            if (_elapsedTime <= _waitTime)
            {
                _elapsedTime += Time.deltaTime;
                return state;
            }

            _instantiatedEnemy = spt.InstantiateNewEntity(horde);
            wavesManager.AddEntitiy(_instantiatedEnemy);
            horde.AddEntity(_instantiatedEnemy);

            _elapsedTime = 0;
            
            state = NodeState.SUCCESS;
            return state;
        }
    }
}