using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR;

namespace General.Damageable
{
    public abstract class Damageable : MonoBehaviour
    {
        public int _health;
        protected int _basicDamage = 1;
        protected int _critDamage = 3;
        public abstract void Damage();
    }
}