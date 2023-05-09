﻿using Ecs.Enemies.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Ecs.Enemies.Systems
{
    public partial struct ESpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Queries for all Spawner components. Uses RefRW because this system wants
            // to read from and write to the component. If the system only needed read-only
            // access, it would use RefRO instead.
            foreach (RefRW<ESpawner> spawner in SystemAPI.Query<RefRW<ESpawner>>())
            {
                ProcessSpawner(ref state, spawner);
            }
        }

        private void ProcessSpawner(ref SystemState state, RefRW<ESpawner> spawner) 
        {
            // If the next spawn time has passed.
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                // Spawns a new entity and positions it at the spawner.
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.prefab);
                // LocalPosition.FromPosition returns a Transform initialized with the given position.
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.spawnPos));

                // Resets the next spawn time.
                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }
    }
}