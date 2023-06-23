using System;
using General;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR.Poke
{
    public class HatsInventory : MonoBehaviour
    {
        [SerializeField] private HatInventory[] hats;
        [SerializeField] private XRBaseInteractable[] _hats = new XRBaseInteractable[4];
        [SerializeField] private GameObject selectedHat;

        [SerializeField] private HatInventory _selectedHatOnPlayer;


        private void OnEnable()
        {
            LoadHats();
            EventManager.PlayerDataLoaded += LoadHats;
        }

        private void OnDisable()
        {
            EventManager.PlayerDataLoaded -= LoadHats;
        }

        private void Start()
        {
        //     for (int i = 0; i < hats.Length; i++)
        //         _hats[i] = hats[i].GetComponent<XRBaseInteractable>();
            foreach (var hat in _hats)
            {
                hat.selectEntered.AddListener(SelectHat);
            }

            LoadHats();
        }


        public void LoadHats()
        {
            var uHats = GameManager.Instance.players[0].PlayerData._unlockedHats;
            for (int i = 0; i < uHats.Length; i++)
            {
                if (!uHats[i])
                    hats[i].SetUnavailable();
                else
                    hats[i].SetAvailable();
            }

            UpdateSelectedVisualsDraw(_hats[GameManager.Instance.players[0].PlayerData._selectedHat]);
        }

        private int GetSelectedHatId(HatInventory hat)
        {
            int id = 0;
            for (int i = 0; i < hats.Length; i++)
            {
                if (hat == hats[i])
                    id = i;
            }

            return id;
        }

        private void SelectHat(BaseInteractionEventArgs args)
        {
            Debug.Log("<color=blue> WORKING </color>");
            if (args.interactorObject is XRRayInteractor)
            {
                var hat = args.interactableObject;
                if (!hat.transform.GetComponent<HatInventory>().ImAvailable())
                    return;
                UpdateSelectedVisualsDraw(hat);

                _selectedHatOnPlayer = hat.transform.GetComponent<HatInventory>();
                int id = GetSelectedHatId(hat.transform.GetComponent<HatInventory>());
                GameManager.Instance.players[0].PlayerData.SetSelectHat(id);
                GameManager.Instance.players[0].PlayerData.UpdateHatSelected();
            }
        }

        private void UpdateSelectedVisualsDraw(IXRInteractable hat)
        {
            selectedHat.GetComponent<MeshFilter>().mesh = hat.transform.GetComponent<MeshFilter>().mesh;
            selectedHat.GetComponent<MeshRenderer>().materials =
                hat.transform.GetComponent<MeshRenderer>().materials;
        }
    }
}