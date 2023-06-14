using BehaviourTree;

namespace Hordes
{
    public class CheckHordeHasEnoughEntities : Node
    {
        
        public CheckHordeHasEnoughEntities()
        {
            name = "CheckHordeHasEnoughEntities";
        }

        public override NodeState Evaluate()
        {
            var horde = (Horde)GetData("currentHorde");
            if (horde.FilledPercentaje() >= 0.55f)
            {
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}