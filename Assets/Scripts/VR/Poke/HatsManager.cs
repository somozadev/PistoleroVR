using System;
using General;
using UnityEngine;

namespace VR.Poke
{
    public class HatsManager : MonoBehaviour
    {
        [SerializeField] private GameObject hatsShop;
        [SerializeField] private GameObject hatsShopButton;
        [SerializeField] private GameObject hatsInventory;
        [SerializeField] private GameObject currentHatDisplay;

        private void OnEnable()
        {

            // EventManager.PlayerDataLoaded += ActivateSystems;
            EventManager.LoadingEnds += ActivateSystems;
        }

        private void OnDisable()
        {
            // EventManager.PlayerDataLoaded -= ActivateSystems;
            EventManager.LoadingEnds -= ActivateSystems;

        }
        

        private void ActivateSystems()
        {
            hatsShop.SetActive(true);
            hatsShopButton.SetActive(true);
            hatsInventory.SetActive(true);
            currentHatDisplay.SetActive(true);
        }
    }
}