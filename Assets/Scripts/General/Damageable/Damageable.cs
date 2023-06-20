using System.Collections;
using System.Collections.Generic;
using General.Sound;
using UnityEngine;
using VR;

namespace General.Damageable
{
    public abstract class Damageable : MonoBehaviour
    {
        public int _health;
        protected int _basicDamage = 1;
        protected int _critDamage = 3;

        public virtual void Damage(BaseGun baseGun)
        {
            AudioManager.Instance.Play("Hitmarker");
        }
    }
}