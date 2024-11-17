using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

public struct BoidComponent : IComponentData
{
    public float3 Position;
    public float3 Velocity;
    public quaternion Orientation;
    public float Radius;
    public float ViewAngle;
    public float SeperationDistance;
    public float MaxSpeed;
    public float RotationSpeed;

    public float CohesionBias;
    public float AlignmentBias;
    public float SeparationBias;

    public float Speed => math.length(Velocity);
}

public struct BoidTag : IComponentData
{
}

