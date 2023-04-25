using System;
using UnityEngine;

namespace General.UI_StartScene
{
    public class StartScene : MonoBehaviour
    {
        [SerializeField] private GameObject _scene;

        private void OnEnable()
        {
            EventManager.LoadingEnds += InitScene;
        }

        private void OnDisable()
        {
            EventManager.LoadingEnds -= InitScene;
        }

        private void InitScene() => _scene.SetActive(true);
    }
}