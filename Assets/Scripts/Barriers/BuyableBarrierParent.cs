using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableBarrierParent : MonoBehaviour
{
    [SerializeField] private int unlockablePrice;

    public int GetPrice()
    {
        return unlockablePrice;
    }
}
