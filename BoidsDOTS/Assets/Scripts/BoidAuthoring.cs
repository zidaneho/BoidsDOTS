using Unity.Entities;
using UnityEngine;

public class BoidAuthoring : MonoBehaviour
{
    public Vector3 startingVelocity;
    public float radius;
    public float viewAngle;
    public float maxSpeed;
    public float seperationDistance;
    public float rotationSpeed = 5;
    public float seperationBias = 0.3f;
    public float cohesionBias = 0.1f;
    public float alignmentBias = 1f;
    class Baker : Baker<BoidAuthoring>
    {
        public override void Bake(BoidAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            Vector3 alignedVelocity = authoring.transform.forward * authoring.startingVelocity.magnitude;
            AddComponent(entity, new BoidComponent()
            {
                Radius = authoring.radius,
                ViewAngle = authoring.viewAngle,
                MaxSpeed = authoring.maxSpeed,
                SeperationDistance= authoring.seperationDistance,
                Position = authoring.transform.position,
                Velocity = alignedVelocity,
                Orientation = authoring.transform.rotation,
                RotationSpeed = authoring.rotationSpeed,
                SeparationBias = authoring.seperationBias,
                CohesionBias = authoring.cohesionBias,
                AlignmentBias = authoring.alignmentBias,
            });
            AddComponent(entity, new BoidTag());
        }
    }
}