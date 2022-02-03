using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight : SteeringBehavior
{
    public Kinematic character;
    private float velocity = 1.0f;

    public Straight(Kinematic parentCharacter)
    {
        character = parentCharacter;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        //result.linear = target.transform.position - character.transform.position;
        result.linear = character.transform.forward * velocity;

        result.angular = 0;
        return result;
    }
}
