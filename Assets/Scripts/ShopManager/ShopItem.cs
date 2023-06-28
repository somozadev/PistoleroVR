using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItemsScriptableObject", order = 1)]
public class ShopItem : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int price;
    [SerializeField] private GameObject prefab;
    
    public GameObject GetPrefab => prefab;
    public int GetPrice => price;
    
    
}