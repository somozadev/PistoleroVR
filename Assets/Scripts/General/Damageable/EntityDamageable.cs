using System;
using Enemies;
using UnityEngine;
using VR;
using Entity = Enemies.BT.Entity;

namespace General.Damageable
{
    [RequireComponent(typeof(Collider))]
    public class EntityDamageable : Damageable
    {
        [SerializeField] private EntityHealth _entityHealth;
        [SerializeField] private Entity _entity;

        private void Awake()
        {
            _entityHealth = GetComponentInParent<EntityHealth>();
            _entity = GetComponentInParent<Entity>();
        }

        public override void Damage(BaseGun baseGun)
        {
            _entity.HitParticle.Play();
            base.Damage(baseGun);
            _entity.TargetData.Gain(5);
            _entityHealth.Health -= baseGun.BulletDamage;
            _entity.SetGainAmount(10);
            //create hit particle and plug it here
            if (_entityHealth.Health < 0)
                _entity.Die();
        }
    }
}