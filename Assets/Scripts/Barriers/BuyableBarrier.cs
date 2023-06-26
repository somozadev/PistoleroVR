using System;
using UnityEngine;
using UnityEngine.Events;

public class BuyableBarrier : PlayerRangeDetection
{
    public int barrierPrice;
    [SerializeField] private InteractType interactType;
    [SerializeField] private GameObject barrierElements;
    
    private void OnEnable()
    {
        barrierPrice = GetComponentInParent<BuyableBarrierParent>().GetPrice();
        if (barrierElements == null)
            barrierElements = transform.GetChild(0).gameObject;

        if (interactType.Equals(InteractType.Collider))
            GetComponent<Collider>().isTrigger = false;
        else
            GetComponent<Collider>().isTrigger = true;
    }

    public void Buy()
    {
        barrierElements.SetActive(false);
        enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (interactType.Equals(InteractType.Trigger)) { Enter(other,null);}
    }
    protected override void OnTriggerExit(Collider other)
    {       
        if (interactType.Equals(InteractType.Trigger)) { Exit(other,null);}
    }
    private void OnCollisionEnter(Collision other)
    {
        if (interactType.Equals(InteractType.Collider)) { Enter(null,other);}
    }
    private void OnCollisionExit(Collision other)
    {
        if (interactType.Equals(InteractType.Collider)) { Exit(null,other);}

    }
   
    
    
    private void Enter(Collider collider, Collision collision)
    {
        if (collider == null)
        {
            Debug.Log(collision.gameObject.tag + " : has collided! ");
            if (collision.gameObject.tag.Equals("Player"))
                _playerEnterRange?.Invoke();
        }

        else
        {
            Debug.Log(collider.gameObject.tag + " : has collided! ");
            if (collider.gameObject.tag.Equals("Player"))
                _playerEnterRange?.Invoke();
        }
    }
    private void Exit(Collider collider, Collision collision)
    {
        if (collider == null)
        {
            Debug.Log(collision.gameObject.tag + " : has exited! ");
            if (collision.gameObject.tag.Equals("Player"))
                _playerLeaveRange?.Invoke();
        }
        else
        {
            Debug.Log(collider.gameObject.tag + " : has exited! ");
            if (collider.gameObject.tag.Equals("Player"))
                _playerLeaveRange?.Invoke();
        }
    }

    

    private enum InteractType
    {
        Collider, 
        Trigger
    }
}