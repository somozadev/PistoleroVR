using System.Collections.Generic;
using Hordes;
using UnityEngine;
using UnityEngine.Serialization;

public class WavesManager : MonoBehaviour
{
    [SerializeField] private int _currentWave;
    [SerializeField] private float _wavesMultiplayer;
    [SerializeField] private float _hordesMultiplayer;
    [SerializeField] private float _enemiesMultiplayer;

    [SerializeField] private int _currentEntities;
    [SerializeField] private int _currentHordes;

    [SerializeField] private List<Horde> _hordes;
    [SerializeField] private List<GameObject> _entities;

    [SerializeField] private int _maxEntities;
    [SerializeField] private int _maxHordes;

    public delegate void NewWave();
    public event NewWave OnNewWave;

    private void TriggerNewWave()
    {
        OnNewWave?.Invoke();
    }

    public bool CanSpawnNewHorde() => _currentHordes < _maxHordes;
    public bool CanSpawnNewEntity() => _currentHordes < _maxHordes;
    
    public void AddEntitiy(GameObject entity)
    {
        if (_currentEntities >= _maxEntities)
            return;
        _currentEntities++;
        _entities.Add(entity);
    }
    public void AddHorde(Horde horde)
    {
        if (!CanSpawnNewHorde())
            return;
        _currentHordes++;
        _hordes.Add(horde);
    }
}