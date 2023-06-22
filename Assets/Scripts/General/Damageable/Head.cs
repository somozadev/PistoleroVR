using System;
using VR;

namespace General.Damageable
{
    public class Head : Damageable
    {
        private void Awake()
        {
            _health = 3; 
        }

        public override void Damage(BaseGun baseGun)
        {
            _health -= _critDamage;
            //particle effect 
            //unable control from player *maybe change it's character to invisible and Instance a ragdoll of him?
            //sound effect
            gameObject.SetActive(false);
        }

    }
}