using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Multiplayer
{
    public class LobbyManager : MonoBehaviour
    {
        private async void CreateLobby(string lobbyName, int maxPlayers)
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void AvailableLobbies()
        {
            try
            {
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
                Debug.Log(queryResponse.Results);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    Debug.Log(lobby.Name + " , " + lobby.MaxPlayers);
                }
            }
            catch (LobbyServiceException e)
            {
                Console.WriteLine(e); 
            }
        }
    }
}