using System;
using General;
using General.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private TMP_Text _wavesText;
    [SerializeField] private TMP_Text _killsText;

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    public void OnDie(int number)
    {
        _killsText.text = $"kills {GetComponent<Player>().PlayerData._kills}";
        _wavesText.text = $"wave {number}";
        _gameOverCanvas.gameObject.SetActive(true);
    }
    public void MenuButton()
    {
        GameManager.Instance.sceneController.LoadScene("E_StartScene", LoadSceneMode.Single);
        AudioManager.Instance.PlayStartingTheme();
        gameObject.SetActive(false);
    }
    public void ReturnToStartScene()
    {
        //load start scene 
        //move player to start position
        _player.PlayerMovement.EnableMovement();
        _player.PlayerInteractionManager.EnableInteraction();
        _player.PlayerHealth.HitEffectVolume.weight = 0;
    }
}