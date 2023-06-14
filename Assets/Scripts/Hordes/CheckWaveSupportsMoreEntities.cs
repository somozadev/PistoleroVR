using BehaviourTree;

namespace Hordes
{
    public class CheckWaveSupportsMoreEntities : Node
    {
        private static WavesManager _wavesManager;

        public CheckWaveSupportsMoreEntities(WavesManager wavesManager)
        {
            name = "CheckWaveSupportsMoreEntities";
            _wavesManager = wavesManager;
        }

        public override NodeState Evaluate()
        {
            parent.SetData("wavesManager", _wavesManager);
            if (_wavesManager.CanSpawnNewEntity())
            {
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
    }
}