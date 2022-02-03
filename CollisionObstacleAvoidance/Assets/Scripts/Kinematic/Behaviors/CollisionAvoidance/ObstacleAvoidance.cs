using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehavior
{
    public Kinematic character;
    private float velocity = 1.0f;

    //3 raycast one forward and two variable whiskers
    private float offsetDegrees = 30.0f;
    //whisker is name of two side raycast
    private float whiskerLength = 2.0f;
    //eyesight is name of main stright raycast
    private float eyesightDist = 3.0f;

    private Ray eyesight;
    private Ray[] whiskers = new Ray[2];

    public ObstacleAvoidance(Kinematic parentCharacter)
    {
        character = parentCharacter;
        eyesight = new Ray(character.transform.position, character.transform.position + character.transform.forward * eyesightDist);
        whiskers[0] = new Ray(character.transform.position, character.transform.position + Quaternion.Euler(0.0f, -offsetDegrees, 0.0f) * (character.transform.forward * whiskerLength));
        whiskers[1] = new Ray(character.transform.position, character.transform.position + Quaternion.Euler(0.0f, offsetDegrees, 0.0f) * (character.transform.forward * whiskerLength));
    }

    public override SteeringOutput getSteering()
    {
        Debug.DrawRay(eyesight.origin, eyesight.direction * eyesightDist, Color.green);
        Debug.DrawRay(whiskers[0].origin, whiskers[0].direction * whiskerLength, Color.yellow);
        Debug.DrawRay(whiskers[1].origin, whiskers[1].direction * whiskerLength, Color.red);
        return new SteeringOutput();
    }
}
