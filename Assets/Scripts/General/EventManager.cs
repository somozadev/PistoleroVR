using UnityEngine;
using System.Collections.Generic;

namespace General
{
    public class EventManager : MonoBehaviour
    {
        public delegate void AuthCompleted();
        public delegate void AuthFailed();

        public event AuthCompleted OnAuthCompleted;
        public event AuthFailed OnAuthFailed;
        public void InvokeOnAuthCompleted(){OnAuthCompleted?.Invoke();}
        public void InvokeOnAuthFailed(){OnAuthFailed?.Invoke();}
        
    }
}