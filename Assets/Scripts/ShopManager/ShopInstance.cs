using TMPro;
using UnityEngine;

public class ShopInstance : PlayerRangeDetection
{
    [SerializeField] private ShopItem _item;

    [SerializeField] private Transform _instantiatePivot;
    [SerializeField] private Transform _initialPlacementPivot;
    [SerializeField] private TMP_Text _price;

    public ShopItem ShopItem => _item;

    private void OnEnable()
    {
        if (_item != null)
        {
            GameObject gun = Instantiate(_item.GetPrefab, _initialPlacementPivot.position, transform.rotation,transform);
            gun.GetComponent<Rigidbody>().isKinematic = true;
            _price.text = _item.GetPrice.ToString() + "$";
        }
    }
}