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

        public override void Damage()
        {
            _health -= _basicDamage;
            StartCoroutine(FlashDamage());
            if (_health <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private IEnumerator FlashDamage()
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            yield return new WaitForSeconds(.2f);
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}