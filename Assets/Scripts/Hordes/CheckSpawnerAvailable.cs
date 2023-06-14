using System.Linq;
using BehaviourTree;
using General;
using Hordes.Spawners;

namespace Hordes
{
    public class CheckSpawnerAvailable : Node
    {
        private static HordeSpawnPoint[] _spawnPoints;
        
        public CheckSpawnerAvailable(HordeSpawnPoint[] spawnPoints)
        {
            name = "CheckSpawnerAvailable";
            _spawnPoints = spawnPoints.Shuffle();
        }

        public override NodeState Evaluate()
        {
            HordeSpawnPoint selectedSpawnPoint = _spawnPoints.FirstOrDefault(hordeSpawnPoint => hordeSpawnPoint.HasSpace());
            
            if(!ReferenceEquals(selectedSpawnPoint,null))
            {
                parent.parent.SetData("selectedSpawnPoint", selectedSpawnPoint);
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;

        }
    }
}