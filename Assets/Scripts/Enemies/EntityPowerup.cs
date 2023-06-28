using System.Collections.Generic;
using Enemies.BT;
using UnityEngine;
using VR.Powerups;
using Random = UnityEngine.Random;

public class EntityPowerup : MonoBehaviour
{
    [SerializeField] private DoublePoints _x2Prefab;
    [SerializeField] private MaxAmmo _maxammoPrefab;
    [SerializeField] private InstaDeath _intsadeathPrefab;
    [SerializeField] private Inmortal _inmortalPrefab;

    [SerializeField] private float _probability = 0.005f;
    [SerializeField] private List<PowerUp> _powerUps;

    public float Probability => _probability;



    public void DropPowerUp()
    {
        int rnd = Random.Range(0, _powerUps.Count);
        PowerUp powerUp = Instantiate(_powerUps[rnd], transform.position, Quaternion.identity);
        powerUp.Init(GetComponentInParent<Entity>().EntitiesManager);
    }
}