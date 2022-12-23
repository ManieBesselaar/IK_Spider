using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FootMover : MonoBehaviour
{
    [SerializeField] Transform _placementTarget;
    [SerializeField] float _stepSize = 2;  // Start is called before the first frame update


    #region Video4 Walk Phases
    //Track the step phases with an enum
    private enum StepPhase {
        PLANTED, //Foot is planted and waiting to take the next step
        LIFTING,//Moving towards highest point in stride
        MOVING_TO_PLANT//Moving the foot to its final position for this stride
    }
    // Set the current step phase to moving to plant. This wil cause it to try and reach the _placementTarget position at the start
    // of your scene
    StepPhase _stepPhase = StepPhase.MOVING_TO_PLANT;
    [SerializeField][Tooltip("How fast the foot should move in units per second.")] float _footSpeed = 2f;
    Vector2 _nextTarget = Vector2.zero; //Initialized to 0 vector to avoid potential null errors

    [SerializeField][Tooltip("How high the foot should lift on each stride")] float StepHeight = 1.2f; 

    [SerializeField] float DistanceTolerance = .2f; // How close to a point the gameobject (this ik target) should be to change state 

    #endregion


    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {

        /*
         If this IKTarget is further from the placement target than the stepsize 
        then start moving to the placement target's current position
         */
        if (Vector2.Distance(transform.position, _placementTarget.position) > _stepSize && _stepPhase == StepPhase.PLANTED)
        {
            _stepPhase = StepPhase.LIFTING;
            _nextTarget = FindLiftPosition();

        }
        if (Vector2.Distance(transform.position, _nextTarget) <= DistanceTolerance && _stepPhase == StepPhase.LIFTING)
        {
            _stepPhase = StepPhase.MOVING_TO_PLANT;
            _nextTarget = _placementTarget.position;
        }
        if (Vector2.Distance(transform.position, _nextTarget) <= DistanceTolerance && _stepPhase == StepPhase.MOVING_TO_PLANT)
        {
            _stepPhase = StepPhase.PLANTED;
            
        }
        MoveFoot();
    }

    private void MoveFoot()
    {
        
        transform.position=Vector2.MoveTowards(transform.position, _nextTarget,_footSpeed * Time.deltaTime);
    }
    Vector2 FindLiftPosition()
    {
        Vector2 differenceVector = _placementTarget.position - transform.position;
        Vector2 liftTarget = (differenceVector /2) + (Vector2.up * StepHeight);

        return (Vector2)transform.position + liftTarget ;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_nextTarget, .4f);
    }
  
}
