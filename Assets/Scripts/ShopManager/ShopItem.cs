using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItemsScriptableObject", order = 1)]
public class ShopItem : ScriptableObject
{
    [SerializeField] private bool unlocked;
    [SerializeField] private int id;
    [SerializeField] private int price;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefabMesh;
    
    public bool IsUnlocked => unlocked;
    public GameObject GetPrefab => prefab;
    public GameObject GetPrefabMesh => prefabMesh;
    public int GetPrice => price;
    public int GetId => id;
    
    public void Unlock() { this.unlocked = true;}
}