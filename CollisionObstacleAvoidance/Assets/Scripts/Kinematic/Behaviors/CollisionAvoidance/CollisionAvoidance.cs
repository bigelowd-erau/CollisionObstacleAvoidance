using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior
{
    private Kinematic character;

    private List<TargetCollision> targetCollisions = new List<TargetCollision>();

    [SerializeField] private float collisionRadius = 1.0f;

    [SerializeField] private float maxAcceleration = 1.0f;

    public CollisionAvoidance(Kinematic parentCharacter)
    {
        character = parentCharacter;
        List<Kinematic>targetKinematics = GameObject.FindObjectsOfType<Kinematic>().ToList();
        targetKinematics.Remove(parentCharacter);
        /*foreach (Kinematic k in targetKinematics)
        {
            if (k.transform.position == character.transform.position)
                targetKinematics.Remove(k);
        }*/
        Debug.Log("Found " + targetKinematics.Count + " kinematics");
        targetKinematics.Remove(character);
        Debug.Log("after remove " + targetKinematics.Count);
        foreach (Kinematic targetKinematic in targetKinematics)
            targetCollisions.Add(new TargetCollision(targetKinematic));
        Debug.Log("targetCollisions length is " + targetCollisions.Count);
    }

    public override SteeringOutput getSteering()
    {
        float shortestTime = Mathf.Infinity;

        TargetCollision curTarget;
        TargetCollision firstTargetToCollide = default;

        foreach (TargetCollision targetCollision in targetCollisions)
        {
            curTarget.target = targetCollision.target;
            curTarget.relativePos = curTarget.target.transform.position - character.transform.position;
            curTarget.distance = curTarget.relativePos.magnitude;
            curTarget.relativeVel = curTarget.target.GetVelocity() - character.GetVelocity();
            curTarget.relativeSpd = curTarget.relativeVel.magnitude;
            curTarget.timeToCollision = -Vector3.Dot(curTarget.relativePos, curTarget.relativeVel) / (curTarget.relativeSpd * curTarget.relativeSpd);

            curTarget.minSeperation = curTarget.distance - curTarget.relativeSpd * curTarget.timeToCollision;

            //if on collision path, and min seperation is smaller than collision radius, and it is the closest collision
            Debug.Log(character.name);
            Debug.Log("time to col: " + curTarget.timeToCollision);
            Debug.Log("minSep: " + curTarget.minSeperation);
            Debug.Log("shortest Time: " + shortestTime);
            Debug.Log("dist: " + curTarget.distance);

            if (curTarget.timeToCollision > 0 && curTarget.minSeperation < 2 * collisionRadius && curTarget.timeToCollision < shortestTime)
            {
                shortestTime = curTarget.timeToCollision;
                firstTargetToCollide.target = curTarget.target;
                firstTargetToCollide.minSeperation = curTarget.minSeperation;
                firstTargetToCollide.distance = curTarget.distance;
                firstTargetToCollide.relativePos = curTarget.relativePos;
                firstTargetToCollide.relativeVel = curTarget.relativeVel;
                Debug.Log("Made it to first");
            }
        }

        //if target was never assigned
        if (firstTargetToCollide.target == null)
        {
            Debug.Log("No collision");
            return null;
        }
        
        SteeringOutput result = new SteeringOutput();
        /*
        if (firstTargetToCollide.minSeperation <= 0 || firstTargetToCollide.distance < 2 * collisionRadius)
            result.linear = firstTargetToCollide.target.transform.position - character.transform.position;
        else
            result.linear = firstTargetToCollide.relativePos + (firstTargetToCollide.relativeVel * shortestTime);
        */
        float dotResult = Vector3.Dot(character.linearVelocity.normalized, firstTargetToCollide.target.linearVelocity.normalized);
        if (dotResult < -0.9)
        {
            result.linear = new Vector3(character.linearVelocity.z, 0.0f, character.linearVelocity.x);
        }
        else
        {
            result.linear -= firstTargetToCollide.target.linearVelocity;
        }
        result.linear.Normalize();

        result.linear *= maxAcceleration;
        //result.linear.Set(result.linear.x, 0.0f, result.linear.z);
        result.angular = 0;
        return result;
    }
}

public struct TargetCollision
{
    public Kinematic target;
    public float distance;
    public float minSeperation;
    public float relativeSpd;
    public float timeToCollision;
    public Vector3 relativePos;
    public Vector3 relativeVel;
    public TargetCollision(Kinematic target)
    {
        this.target = target;
        distance = 0.0f;
        minSeperation = 0.0f;
        relativeSpd = 0.0f;
        timeToCollision = 0.0f;
        relativePos = Vector3.zero;
        relativeVel = Vector3.zero;
    }
}