using Enemies.BT;
using UnityEngine;

namespace VR.Powerups
{
    public abstract class PowerUp : PlayerRangeDetection
    {
        protected EntitiesManager _entitiesManager;
        [SerializeField] protected GameObject visuals;
        
        

        public void Init(EntitiesManager entitiesManager)
        {
            _entitiesManager = entitiesManager;
        }
        protected virtual void Awake()
        {
            AddListenerPlayerEnter(PerformPowerupAction);
        }
        protected abstract void PerformPowerupAction();
    }
}