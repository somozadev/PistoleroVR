namespace VR.Powerups
{
    public abstract class PowerUp : PlayerRangeDetection
    {
        protected virtual void Awake()
        {
            AddListenerPlayerEnter(PerformPowerupAction);
        }
        protected abstract void PerformPowerupAction();
    }
}