using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : SteeringBehavior
{
    public Kinematic character;
    private Vector3 targetPosition;
    float maxAcceleration = 1f;

    private const float wanderTargetRadius = 2.0f;
    private const float wanderTargetOffset = 3.0f;
    private const float wanderTargetThreshhold = 1.5f;

    public Wander(Kinematic parentCharacter)
    {
        character = parentCharacter;
    }

    private void GenerateTargetPosition()
    {
        float randomRotation = Random.value * 2 * Mathf.PI;
        targetPosition = new Vector3(Mathf.Cos(randomRotation), 0, Mathf.Sin(randomRotation)) * wanderTargetRadius;
        targetPosition += character.transform.forward * wanderTargetOffset;
        targetPosition = character.transform.position + targetPosition;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        if (targetPosition == Vector3.zero|| (character.transform.position - targetPosition).magnitude < wanderTargetThreshhold)
            GenerateTargetPosition();
        if (targetPosition == Vector3.positiveInfinity)
        {
            return null;
        }

        //result.linear = target.transform.position - character.transform.position;
        result.linear = targetPosition - character.transform.position;

        // give full acceleration along this direction
        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }
}
