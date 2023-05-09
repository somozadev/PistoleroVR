using Ecs.Enemies.Components;
using Unity.Entities;
using UnityEngine;

namespace Ecs.Enemies.Authoring
{
    class ESpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public float SpawnRate;
    }

    class SpawnerBaker : Baker<ESpawnerAuthoring>
    {
        public override void Bake(ESpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ESpawner
            {
                // By default, each authoring GameObject turns into an Entity.
                // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
                prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                spawnPos = authoring.transform.position,
                nextSpawnTime = 0.0f,
                spawnRate = authoring.SpawnRate
            });
        }
    }
}