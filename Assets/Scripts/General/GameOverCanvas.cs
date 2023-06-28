using General;
using TMPro;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private TMP_Text _wavesText;
    [SerializeField] private TMP_Text _killsText;


    public void OnDie(int number)
    {
        _killsText.text = $"kills {GetComponent<Player>().PlayerData._kills}";
        _wavesText.text = $"wave {number}";
        _gameOverCanvas.gameObject.SetActive(true);
    }

    public void ReturnToStartScene()
    {
        _gameOverCanvas.gameObject.SetActive(false);
    }
}