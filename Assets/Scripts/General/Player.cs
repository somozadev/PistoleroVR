using System;
using CloudServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace General
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private MovementVR _movementVR;
        [SerializeField] private RigVR _rigVR;
        [SerializeField] private CharacterCustomization _characterCustomization;
        [SerializeField] private ActionBasedController _leftHand;
        [SerializeField] private ActionBasedController _rightHand;
        [SerializeField] private PlayerSaveData _playerData;

        private  void Start()
        {
            GameManager.Instance.eventManager.OnAuthCompleted += GetPlayerDataFromCloud;
        }

        private async void GetPlayerDataFromCloud()
        {
            await GameManager.Instance.cloudSaveManager.CheckIfUserHasData(GameManager.Instance.gameServices._playerId);
            
            _playerData = await GameManager.Instance.cloudSaveManager.LoadFromCloud(GameManager.Instance.gameServices._playerId);
        }
        private void OnEnable()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.player = this;
        }
    }
}