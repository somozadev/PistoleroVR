using Entity = Enemies.BT.Entity;
using Enemies;
using UnityEngine;
using VR;

namespace General.Damageable
{
    [RequireComponent(typeof(Collider))]
    public class EntityCritDamageable : Damageable
    {
        [SerializeField] private float _critValue = 1.5f;
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
            _entity.SetGainAmount(25);
            _entityHealth.Health -= (baseGun.BulletDamage * _critValue);
            //create hit particle and plug it here

            if (_entityHealth.Health < 0)
                _entity.Die();
        }
    }
}