using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct BoidBehaviorSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, boid, entity)
                 in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BoidComponent>>().WithEntityAccess())
        {
            var (centerPosition, centerVelocity) =
                GetCenterPosAndVel(ref state, transform.ValueRO, boid.ValueRO, entity);
            float3 displacement = float3.zero;
            
            foreach (var (transform2, boid2, entity2)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoidComponent>>().WithEntityAccess())
            {
                if (entity == entity2) continue;
                
                //Seperation
                var difference = transform.ValueRW.Position - transform2.ValueRO.Position;
                var distance = math.distance(transform.ValueRO.Position, transform2.ValueRO.Position);

                if (distance < boid.ValueRW.SeperationDistance)
                {
                    //We are adding instead of subtracting to push original away from neighbors
                    displacement += difference / (distance * distance);
                }
                //
                
            }
            
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            boid.ValueRW.Velocity += displacement * boid.ValueRW.SeparationBias * deltaTime + 
                                     (centerPosition - boid.ValueRW.Position) * boid.ValueRW.CohesionBias * deltaTime +
                                     (centerVelocity - boid.ValueRW.Velocity) * boid.ValueRW.AlignmentBias * deltaTime;
            
            // Apply the accumulated velocity to update the position
            if (math.length(boid.ValueRW.Velocity) > boid.ValueRW.MaxSpeed)
            {
                boid.ValueRW.Velocity = boid.ValueRW.MaxSpeed * math.normalize(boid.ValueRW.Velocity);
            }
            transform.ValueRW.Position += boid.ValueRW.Velocity * SystemAPI.Time.DeltaTime;
            // Sync the BoidComponent's position with the transform's position
            boid.ValueRW.Position = transform.ValueRW.Position;
        }
        
        
    }

    

    private (float3, float3) GetCenterPosAndVel(ref SystemState state, LocalTransform transform,
        BoidComponent boid, Entity entity)
    {
        int boidCount = 0;
        float3 totalPosition = float3.zero;
        float3 totalVelocity = float3.zero;
        
        foreach (var (transform2, boid2, entity2)
                 in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoidComponent>>().WithEntityAccess())
        {
            if (entity == entity2) continue;
            // if (boid.InRange(boid2.ValueRO,false))
            // {
            //     totalPosition += transform2.ValueRO.Position;
            // }
            bool withinRange = Math.Abs(math.length(transform2.ValueRO.Position - transform.Position))
                               < boid.Radius;
            if (withinRange)
            {
                totalPosition += transform2.ValueRO.Position;
                totalVelocity += boid2.ValueRO.Velocity;
                boidCount++;
            }
        }

        if (boidCount <= 0) return (float3.zero, float3.zero);
        return (totalPosition / boidCount, totalVelocity / boidCount);
    }

   

   
    
}