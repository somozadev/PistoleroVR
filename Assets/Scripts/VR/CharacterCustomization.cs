using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] private Transform _pivotTransform; 
    [SerializeField] private GameObject[] _hats;
    public GameObject currentHat; 
    
    
    
    
    
    
    public void SetHatWithIndex(int index)
    {
        if (index >= _hats.Length - 1) return;
        if (currentHat != null)
        {
            currentHat.SetActive(false);
            currentHat = _hats[index];
            currentHat.SetActive(true);
        }
    }
    
}
