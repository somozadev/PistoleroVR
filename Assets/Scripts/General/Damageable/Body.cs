using System.Collections;
using UnityEngine;
using VR;

namespace General.Damageable
{
    public class Body : Damageable
    {
        private void Awake()
        {
            _health = 3;
        }

        public override void Damage(BaseGun baseGun)
        {
            _health -= _basicDamage;
            
            //particle effect
            //sound effect
            
            if (_health <= 0)
            {
                //unable control from player *maybe change it's character to invisible and instance a ragdoll of him?
                gameObject.SetActive(false);
            }
        }

    }
}