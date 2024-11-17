using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateAfter(typeof(BoidBehaviorSystem))]
public partial struct BoidRotationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, boid)
                 in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BoidComponent>>().WithAll<BoidTag>())
        {
            if (math.lengthsq(boid.ValueRW.Velocity) < math.EPSILON)
                continue; 
            // Normalize the velocity to get the direction
            float3 direction = math.normalize(boid.ValueRW.Velocity);

            // Define the up vector (usually world up: y-axis)
            float3 up = math.up();
            
            quaternion targetRot = quaternion.LookRotation(direction, up);
            transform.ValueRW.Rotation = math.slerp(boid.ValueRW.Orientation,targetRot, boid.ValueRW.RotationSpeed * SystemAPI.Time.DeltaTime);
            boid.ValueRW.Orientation = transform.ValueRW.Rotation;
        }
    }
}