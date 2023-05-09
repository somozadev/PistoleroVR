using System;
using System.Collections;
using System.Collections.Generic;
using General;
using Multiplayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    private void Start() => onGameManagerLoaded?.Invoke();

    #endregion

    public UnityEvent onGameManagerLoaded;
    public List<Player> players;
    public ObjectPoolingManager objectPoolingManager;
    public SceneController sceneController;
    //todo -> player script with instance here and references for this 2 there. Olso make controllers to lock player movement or head rotation or lineInteractors (needed 4 loading scene )
   public GameServices gameServices;
   public bool IsHost;
}

