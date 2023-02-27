using System;

namespace General.Damageable
{
    public class Head : Damageable
    {
        private void Awake()
        {
            _health = 3; 
        }

        public override void Damage()
        {
            _health -= _critDamage;
            //particle effect 
            //unable control from player *maybe change it's character to invisible and instance a ragdoll of him?
            //sound effect
            gameObject.SetActive(false);
        }

    }
}