using System;
using System.Linq;
using UnityEngine;

namespace General
{
    public class GetCameraReferenceToCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;

        private void OnValidate()
        {
            if (canvas == null)
                canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            canvas.worldCamera = GameManager.Instance.players.First().GetComponentInChildren<Camera>();
        }
    }
}