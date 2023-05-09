using Unity.Entities;
using Unity.Mathematics;

namespace Ecs.Enemies.Components
{
    public struct ESpawner : IComponentData
    {
        public Entity prefab;
        public float3 spawnPos;
        public float nextSpawnTime;
        public float spawnRate;

    }
}