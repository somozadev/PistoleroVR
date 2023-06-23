using System;
using UnityEngine;

namespace VR.Poke
{
    public class HatInventory : MonoBehaviour
    {
        public Material[] initialMats;
        public Material unavailableMat;
        private Renderer _renderer;
        private bool _available;

        public void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            initialMats = _renderer.materials;
        }

        public bool ImAvailable() => _available;

        public void SetAvailable()
        {
            if (initialMats == null)
                initialMats = _renderer.materials;
            _renderer = GetComponent<MeshRenderer>();
            _renderer.materials = initialMats;
            _available = true;
        }

        public void SetUnavailable()
        {
            if (initialMats == null)
                initialMats = _renderer.materials;
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material = unavailableMat;
            _available = false;
        }
    }
}