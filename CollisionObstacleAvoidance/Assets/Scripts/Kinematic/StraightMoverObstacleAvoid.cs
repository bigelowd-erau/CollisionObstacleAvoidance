using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMoverObstacleAvoid : Kinematic
{
    Straight myMoveType;
    //CollisionAvoidance myCollisionAvoidanceType;
    ObstacleAvoidance myObstacleAvoidanceType;
    LookWhereGoing myRotateType;

    public bool flee = false;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new Straight(this);

        //myCollisionAvoidanceType = new CollisionAvoidance(this);
        myObstacleAvoidanceType = new ObstacleAvoidance(this);

        myRotateType = new LookWhereGoing(this);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        steeringUpdate = new SteeringOutput();
        //check for collision
        //steeringUpdate = myCollisionAvoidanceType.getSteering();
        steeringUpdate = myObstacleAvoidanceType.getSteering();
        //if a collision wasnt found wander
        if (steeringUpdate == null)
        {
            steeringUpdate = new SteeringOutput();
            steeringUpdate.linear = myMoveType.getSteering().linear;
            //Debug.Log("Got stright");
        }
       // else
            //Debug.Log("Avoiding");
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.FixedUpdate();
    }
}
