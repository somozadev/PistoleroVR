using TMPro;
using UnityEngine;

public class ShopInstance : PlayerRangeDetection
{
    [SerializeField] private ShopItem _item;

    [SerializeField] private Transform _initialPlacementPivot;
    [SerializeField] private TMP_Text _price;

    public float Range => range;
    public ShopItem ShopItem => _item;

    private void Awake()
    {
        RefillGun();
    }

    public void RefillGun()
    {
        if (_item != null)
        {
            GameObject gun = Instantiate(_item.GetPrefab, _initialPlacementPivot.position,
                _initialPlacementPivot.rotation, _initialPlacementPivot);
            gun.GetComponent<Rigidbody>().isKinematic = true;
            _price.text = _item.GetPrice + "$";
        }
    }
    
}