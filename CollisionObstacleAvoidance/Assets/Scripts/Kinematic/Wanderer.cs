using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Kinematic
{
    Wander myMoveType;
    CollisionAvoidance myCollisionAvoidanceType;
    LookWhereGoing myRotateType;

    public bool flee = false;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new Wander(this);

        myCollisionAvoidanceType = new CollisionAvoidance(this);

        myRotateType = new LookWhereGoing(this);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        steeringUpdate = new SteeringOutput();
        //check for collision
        steeringUpdate = myCollisionAvoidanceType.getSteering();
        //if a collision wasnt found wander
        if (steeringUpdate == null)
        {
            steeringUpdate = new SteeringOutput();
            steeringUpdate.linear = myMoveType.getSteering().linear;
        }
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.FixedUpdate();
    }
}
