using System;
using System.Threading.Tasks;
using General;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Oculus.Platform;
using Oculus.Platform.Models;

namespace Multiplayer
{
    public class GameServices : MonoBehaviour
    {
        public string _playerId;
        public PlayerData playerData;

        
        
        
        
        private void Start()
        {
            Core.AsyncInitialize("6748617261832040").OnComplete(OnInitializationCallback);
        }

        private void OnInitializationCallback(Message<PlatformInitialize> msg)
        {
            if (msg.IsError)
            {
                Debug.LogErrorFormat("Oculus: Error during initialization. Error Message: {0}",
                    msg.GetError().Message);
            }
            else
            {
                Entitlements.IsUserEntitledToApplication().OnComplete(OnIsEntitledCallback);
            }
        }

        private void OnIsEntitledCallback(Message msg)
        {
            if (msg.IsError)
            {
                Debug.LogErrorFormat("Oculus: Error verifying the user is entitled to the application. Error Message: {0}",
                    msg.GetError().Message);
            }
            else
            {
                GetLoggedInUser();
            }
        }

        private void GetLoggedInUser()
        {
            Users.GetLoggedInUser().OnComplete(OnLoggedInUserCallback);
        }

        private void OnLoggedInUserCallback(Message<User> msg)
        {
            if (msg.IsError)
            {
                Debug.LogErrorFormat("Oculus: Error getting logged in user. Error Message: {0}",
                    msg.GetError().Message);
            }
            else
            {
                _playerId = msg.Data.ID.ToString(); // do not use msg.Data.OculusID;
                GetUserProof();
            }
        }

        private void GetUserProof()
        {
            Users.GetUserProof().OnComplete(OnUserProofCallback);
        }

        private void OnUserProofCallback(Message<UserProof> msg)
        {
            if (msg.IsError)
            {
                Debug.LogErrorFormat("Oculus: Error getting user proof. Error Message: {0}",
                    msg.GetError().Message);
            }
            else
            {
                string oculusNonce = msg.Data.Value;
                // Authentication can be performed here
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //
        //
        //
        // private async void Awake()
        // {
        //     if (playerData == null)
        //         playerData = FindObjectOfType<PlayerData>();
        //     await UnityServices.InitializeAsync();
        //     SetupEvents();
        //     await SignInAnon();
        //     await playerData.LoadData();
        // }
        //
        // private void SetupEvents()
        // {
        //     AuthenticationService.Instance.SignedIn += () => { _playerId = AuthenticationService.Instance.PlayerId; };
        //     AuthenticationService.Instance.SignInFailed += (err) => { Debug.Log(err.ToString()); };
        //     AuthenticationService.Instance.SignedOut += () => { _playerId = ""; };
        //     AuthenticationService.Instance.Expired += () => { Debug.Log("Session expired"); };
        // }
        //
        public async Task SignInAnon()
        {
            try
            {
                
                Debug.Log("Player signed in anon with " + _playerId);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
                //save user ID locally to check on new start if that user exists (to popup another login type)
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //
        // private async Task SignInWithOculusASync(string nonce, string userId)
        // {
        //     try
        //     {
        //         await AuthenticationService.Instance.SignInWithOculusAsync(nonce, userId);
        //         
        //     }
        //     catch (AuthenticationException e)
        //     {
        //         Debug.LogException(e);
        //     }
        //     catch (RequestFailedException e)
        //     {
        //         Debug.LogException(e);
        //     }
        // }
        //
        // private async Task LinkWithOculusAsync(string nonce, string userId)
        // {
        //     try
        //     {
        //         await AuthenticationService.Instance.LinkWithOculusAsync(nonce, userId);
        //         Debug.Log("Linked!");
        //     }
        //     catch (AuthenticationException e) when (e.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        //     {
        //         Debug.LogError("This user is already linked with another account. Log in instead.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.LogError("Link failed.");
        //         Debug.LogException(ex);
        //     }
        // }
    }
}