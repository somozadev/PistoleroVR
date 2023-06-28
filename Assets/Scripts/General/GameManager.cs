using System.Collections.Generic;
using General.Services;
using UnityEngine;

namespace General
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager instance;

        public static GameManager Instance
        {
            get { return instance; }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                LoadChildren();
            }
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        [SerializeField] private List<GameObject> children;
        
        public List<Player> players;
        public ObjectPoolingManager objectPoolingManager;

        public SceneController sceneController;

        //todo -> player script with Instance here and references for this 2 there. Olso make controllers to lock player movement or head rotation or lineInteractors (needed 4 loading scene )
        public GameServices gameServices;
        public bool IsHost;

        private void LoadChildren()
        {
            foreach (var child in children)
            {
                child.SetActive(true);
            }   
        }
        
        
    }
}