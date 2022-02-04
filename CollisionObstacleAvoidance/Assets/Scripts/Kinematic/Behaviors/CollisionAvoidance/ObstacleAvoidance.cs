using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehavior
{
    public Kinematic character;
    private float velocity = 1.0f;

    //3 raycast one forward and two variable whiskers
    private float offsetPercentage = .5f;
    //whisker is name of two side raycast
    private float whiskerLength = 1.5f;
    //eyesight is name of main stright raycast
    private float eyesightDist = 2f;
    //private float positionoffset
    private float maxAccel;

    private Ray eyesight;
    private Ray[] whiskers = new Ray[2];
    private int rotateDir;
    bool avoiding = false;
    private float accel;

    private LineRenderer lr;
    

    public ObstacleAvoidance(Kinematic parentCharacter)
    {
        character = parentCharacter;
        maxAccel = character.maxSpeed;
        lr = character.gameObject.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = 0.1f;
        lr.material.color = Color.green;
        lr.positionCount = 6;
    }

    public override SteeringOutput getSteering()
    {
        eyesight = new Ray(character.transform.position + character.transform.forward, character.transform.forward);
        whiskers[0] = new Ray(character.transform.position + character.transform.forward, character.transform.forward - offsetPercentage*character.transform.right);
        whiskers[1] = new Ray(character.transform.position + character.transform.forward, character.transform.forward + offsetPercentage*character.transform.right);
        
        Debug.DrawRay(eyesight.origin, eyesight.direction * eyesightDist, Color.green);
        lr.SetPosition(0, eyesight.origin);
        lr.SetPosition(1, eyesight.origin + eyesight.direction * eyesightDist);
        lr.SetPosition(2, whiskers[0].origin + whiskers[0].direction * whiskerLength);
        lr.SetPosition(3, eyesight.origin);
        lr.SetPosition(4, whiskers[1].origin + whiskers[1].direction * whiskerLength);
        lr.SetPosition(5, eyesight.origin + eyesight.direction * eyesightDist);
        Debug.DrawRay(whiskers[0].origin, whiskers[0].direction * whiskerLength, Color.yellow);
        Debug.DrawRay(whiskers[1].origin, whiskers[1].direction * whiskerLength, Color.red);
        RaycastHit eyeHit;
        RaycastHit[] whiskHit = new RaycastHit[2];
        bool eye = Physics.Raycast(eyesight, out eyeHit, eyesightDist);
        bool[] whisks = { Physics.Raycast(whiskers[0], out whiskHit[0], whiskerLength), Physics.Raycast(whiskers[1], out whiskHit[1], whiskerLength) };
 
        SteeringOutput result = new SteeringOutput();
        //if eyesight hits and either no whiskers hit or both whisker hit
        //turn in random direction
        if (!avoiding)
        {
            if (eye)
            {
                //Debug.Log(eyeHit.distance + " " + eyeHit.collider.name);
                accel = 0.5f * character.linearVelocity.magnitude * (eyesightDist - eyeHit.distance); //scale speed based on how far away
                avoiding = true;
                if (!(whisks[0] ^ whisks[1]))
                    rotateDir = Random.value > 0.5 ? -1 : 1;
                //if eye and left whisker turn right
                else if (whisks[0])
                    rotateDir = 1;
                //if eye and right whisker turn left
                else// if (whisks[1]) //this if is implied
                    rotateDir = -1;
            }
            else return null;
            ///
            ///
            //maybe add somethignehwere if one whisker collides move away and have logic if both are colliding cancel out
            ///
        }
        else if (!eye)
            avoiding = false;
        else
            accel = character.linearVelocity.magnitude * (eyesightDist - eyeHit.distance);
        //Debug.Log(avoiding + " " + eye);
        result.linear = 0.5f *rotateDir * character.transform.right - accel * character.transform.forward;
        Debug.DrawRay(character.transform.position, result.linear, Color.blue);
       // Debug.Log(rotateDir);
        //result.linear.Normalize();
        result.linear *= accel;
        result.angular = 0;
        //if middle hit and no others
        //return null if no collisions found

        //want to use adjustable wiskers so if wall caught widen search until no wall found
        return result;
    }
}
