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

    [ContextMenu("ReturnButton")]
    public void ReturnButton()
    {
        GameManager.Instance.sceneController.LoadScene("E_StartScene", LoadSceneMode.Single);
        GameManager.Instance.players[0].PlayerMovement.ResetTo(Vector3.zero);
        GameManager.Instance.players[0].PlayerInteractionManager.EnableInteraction();
        GameManager.Instance.players[0].PlayerMovement.EnableMovement();
        GameManager.Instance.players[0].PlayerHealth.HitEffectVolume.weight = 0;
        AudioManager.Instance.PlayStartingTheme();
        _gameOverCanvas.gameObject.SetActive(false);
    }
    
}