using TMPro;
using UnityEngine;

namespace General
{
    public class PlayerIngameCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _hpText;
        [SerializeField] private TMP_Text _economyText;


        [SerializeField] private PlayerHealth _health;

        public void EnableCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }
        public void DisableCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
        
        
        public void UpdateHp(int value)
        {
            _hpText.text =  $"<color=red> {value} </color>";
        }

        public void UpdateEconomy(int value)
        {
            _economyText.text = $"<color=yellow> {value} $</color>";
        }
    }
}