using Unity.Entities;
using Unity.Mathematics;

public static class Util
{
    public static bool InRange(this BoidComponent boid, BoidComponent other, bool restrictView = true)
    {
        // Calculate the distance between the boids
        float3 difference = boid.Position - other.Position;
        float distanceSqr = math.lengthsq(difference);

        // Check if the other boid is within the radius range
        if (distanceSqr > boid.Radius * boid.Radius) return false;
        if (restrictView) return true;

        // Calculate the direction of the boid's flight (normalized velocity)
        float3 forwardDirection = math.normalize(boid.Velocity);

        // Calculate the direction to the other boid
        float3 directionToOther = math.normalize(difference);
        
        var dot = math.clamp(math.dot(forwardDirection, directionToOther), -1,1);

        // Calculate the angle between the boid's direction and the direction to the other boid
       
        float angle = math.degrees(math.acos(dot));

        // Check if the other boid is within the view angle
        return angle <= boid.ViewAngle / 2;
    }
}