using BehaviourTree;

namespace Hordes
{
    public class CheckHordeIsGrouped : Node
    {
        public CheckHordeIsGrouped()
        {
            name = "CheckHordeIsGrouped";
        }


        public override NodeState Evaluate()
        {
            var horde = (Horde)GetData("currentHorde");
            if (horde.IsHordeGrouped())
            {
                state = NodeState.SUCCESS;
                return state;
            }
            horde.TryGroupHorde();
         

            state = NodeState.FAILURE;
            return state;
        }
    }
}