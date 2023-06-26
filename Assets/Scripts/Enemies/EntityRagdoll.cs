using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EntityRagdoll : MonoBehaviour
    {
        [SerializeField] private GameObject _ragdollRig;
        [SerializeField] private GameObject _normalRig;


        public List<RagdollElement> _ragdollElements;

        public void ActivateRagdoll()
        {
            _normalRig.SetActive(false);
            _ragdollRig.SetActive(true);
        }

        public void DeactivateRagdoll()
        {
            _ragdollRig.SetActive(false);
            _normalRig.SetActive(true);
            foreach (var re in _ragdollElements)
                re.Reset();
        }

        [Serializable]
        public struct RagdollElement
        {
            public Transform ragdollElement;
            public Transform referenceElement;
            public Rigidbody rb;
            public RagdollElement(Rigidbody rb, Transform ragdollElement, Transform referenceElement)
            {
                this.ragdollElement = ragdollElement;
                this.referenceElement = referenceElement;
                this.rb = rb;
            }
            public void Reset()
            {
                ragdollElement.localPosition = referenceElement.localPosition;
                ragdollElement.localRotation = referenceElement.localRotation;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}