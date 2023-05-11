using UnityEngine;

namespace General
{
    public class SceneActivateDeactivate : MonoBehaviour
    {
        [SerializeField] private GameObject scene;

        private void OnEnable()
        {
            EventManager.LoadingStarts += HideScene;
            EventManager.LoadingEnds += ShowScene;
        }

        private void OnDisable()
        {
            EventManager.LoadingStarts -= HideScene;
            EventManager.LoadingEnds -= ShowScene;
        }

        private void ShowScene() => scene.SetActive(true);
        private void HideScene() => scene.SetActive(false);
    }
}