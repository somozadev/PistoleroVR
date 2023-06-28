using System;
using UnityEngine;

namespace General
{
    public abstract class Utils

    {
        public static bool CheckTypes( Type type , GameObject handItemType, GameObject shopItemType)
        {
            if (handItemType.GetComponent(type) != null && shopItemType.GetComponent(type) != null)
                return true;
            return false;
        }
    }
}