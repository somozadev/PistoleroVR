using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class ShopInstance : PlayerRangeDetection
{
    [SerializeField] private ShopItem _item;

    [SerializeField] private Transform _instantiatePivot;
    [SerializeField] private Transform _initialPlacementPivot;
    [SerializeField] private TMP_Text _price;
    

    private void OnEnable()
    {
        if (_item != null)
        {
            Instantiate(_item.GetPrefabMesh, _initialPlacementPivot.position, transform.rotation);
            _price.text = _item.GetPrice.ToString() + "$";
        }
    }

    //when raycasts hit call this function
    public void OnRaycastHit()
    {
        //calls visual event to trigger onLookingUI
        //Glow element visually, trigger some animation... 
    }

    public void OnBuyItem()
    {
        
        //if player has enough money this
        
            //subtract from player currency the _item.GetPrice()
        
            Instantiate(_item.GetPrefab, _instantiatePivot.position, quaternion.identity);
            //maybe snap to players hand ? idk need test
        
        //else show visual error not enough money 
    }
}