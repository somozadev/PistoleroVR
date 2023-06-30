using General.Sound;
using UnityEngine;
using VR;

namespace General.Damageable
{
    [RequireComponent(typeof(Rigidbody))]
    public class Damageable : MonoBehaviour
    {
        public virtual void Damage(BaseGun baseGun)
        {
            AudioManager.Instance.Play("Hitmarker");
        }
    }
}