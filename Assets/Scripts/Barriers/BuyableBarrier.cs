using System;
using UnityEngine;
using UnityEngine.Events;

public class BuyableBarrier : PlayerRangeDetection
{

    public int barrierPrice;

    private void OnValidate()
    {
        barrierPrice = GetComponentInParent<BuyableBarrierParent>().GetPrice();
    }

    protected override void OnTriggerEnter(Collider other) { }
    protected override void OnTriggerExit(Collider other) { }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag + " : has collided! ");
        if (other.gameObject.tag.Equals("Player"))
            _playerEnterRange?.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        Debug.Log(other.gameObject.tag + " : has exited! ");
        if (other.gameObject.tag.Equals("Player"))
            _playerLeaveRange?.Invoke();
    }

    private void OnEnable()
    {
        _playerEnterRange.AddListener(delegate { Test("enter"); });
        _playerLeaveRange.AddListener(delegate { Test("leave"); });
    }

    private void OnDestroy()
    {
        _playerEnterRange.RemoveListener(delegate { Test("enter"); });
        _playerLeaveRange.RemoveListener(delegate { Test("leave"); });
    }

    private void Test(string msg)
    {
        Debug.Log(msg); 
    }
}
