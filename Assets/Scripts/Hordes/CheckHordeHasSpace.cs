using BehaviourTree;

namespace Hordes
{
    public class CheckHordeHasSpace : Node
    {
        private bool _slotsAvailable;
        private Horde _horde;
        public CheckHordeHasSpace(Horde horde)
        {
            name = "CheckHordeHasSpace";
            _slotsAvailable = horde.HasSpace();
            _horde = horde;
        }

        public override NodeState Evaluate()
        {
            parent.parent.SetData("currentHorde", _horde);
            _slotsAvailable = _horde.HasSpace();
            if (_slotsAvailable)
            {
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;

        }
    }
}