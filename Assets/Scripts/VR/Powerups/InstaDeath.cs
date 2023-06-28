using System.Collections.Generic;
using Enemies.BT;

namespace VR.Powerups
{
    public class InstaDeath : PowerUp
    {
        protected override void PerformPowerupAction()
        {
            List<Entity> currentEntities = new List<Entity>();
            if (_entitiesManager != null)
            {
                if (_entitiesManager.Entities.Count > 0)
                {
                    currentEntities.AddRange(_entitiesManager.Entities);

                    foreach (var e in currentEntities)
                    {
                        e.Die();
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}