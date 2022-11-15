using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Foot : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A transform indicating the default foot position when planted relative to the head bone." +
        "Must be childed to the head bone of the spider.")]
    Transform _footPlacementTarget;
    [SerializeField]
    [Tooltip("How fast the foot target should move to it's next point")] 
    float _footMovementSpeed;
    [SerializeField]
    [Tooltip("How far the current foot target should be from the footPlacementTarget before a new step cycle is started")] 
    float _stepSize;
    [SerializeField]
    [Tooltip ("The minimum distance that a foot target should be to its target to be considered to be on target.")] 
    float _postitionTolerance = .1f; //This avoids the problem of the foot target never
                                     //reaching it's destination due to a small difference in precision. Just get close enough
    [SerializeField][Tooltip("How high the foot should be lifted with each step.")] float _stepHeight = .5f;
    [SerializeField]
    [Tooltip("The transform of the spider body to cast the ray from, specifically the parent of the footPlacementTargets")]
    Transform _bodyTransform;
    [SerializeField]
    [Tooltip("How far down the terrain detection ray should be cast")] float _raycastDepth = 2;
    [SerializeField] LayerMask _terrainLayer;
    [SerializeField]
    [Tooltip("A contact filter to filter out hits that are not terrain")]
    ContactFilter2D _terraingFilter;
    Vector3 _nextTarget; //Where the foot target is heading to.
    Vector3 halfway_point = Vector3.zero; // Used to store the position for the halfway target, for drawing debug gizmos
    Vector3 _plantingPoint = Vector3.zero;
    public bool isPlanted = false; // True is the opposing foot is in a planted state. Helps prevent having all feet in the air at once
    [SerializeField][Tooltip("The foot that should be planted while this foot is in the air")] Foot _opposingFoot;
    [SerializeField][Tooltip("Set true to have leg work without an opposing foot")] bool isSoloLeg = false;

    // Start is called before the first frame update
    enum StepState
    {
        planted,
        lifting,
        moving_to_plant
    }
    StepState _state = StepState.moving_to_plant; //Keep track of which phase of the walk cycle you are in.
    void Start()
    {
        _nextTarget = _footPlacementTarget.position; // start planting your foot so you are ready to move.

        
    }

    // Update is called once per frame
    void Update()
    {
        if (_state == StepState.planted && ( _opposingFoot.isPlanted || isSoloLeg))
        {

            if (Vector3.Distance(transform.position, _footPlacementTarget.position) > _stepSize) //If this foot is at its plant position
            {
                _plantingPoint = GetTerrain(_footPlacementTarget.position);
                _nextTarget = CalculateLiftPos(); // Calculate where the foot should be at the top of the lift
                halfway_point =_nextTarget; // Assign for debugging. TODO: remove before release
                _state = StepState.lifting;
               
                isPlanted = false; //Started lifting the foot and it is no longer on the ground.
            }
        }
        if(_state == StepState.lifting)
        {
            /*If the foot is lifting and is close enough to the top of the step cycle 
              */  
            if (Vector3.Distance(transform.position, _nextTarget) < _postitionTolerance) 
            {
                //Set the target to the final placement position and change the state accordingly

                _nextTarget = _plantingPoint;
                _state = StepState.moving_to_plant;
                Debug.Log("State changed " + _state);
            }
        }
        if(_state == StepState.moving_to_plant)
        {/*
          if this foot is moving towards the plant position and is close enough 
            then change the state to planted and set isPlanted to true so that the opposing leg can start moving if needed. 
          */
            if (Vector3.Distance(transform.position, _nextTarget) < _postitionTolerance)
            {
                
                _state = StepState.planted;
                isPlanted = true;
                Debug.Log("State changed " + _state);
            }
        }
        //Move this foot target to the assigned target position with the assigned speed.
        transform.position = Vector3.MoveTowards(transform.position, _nextTarget, _footMovementSpeed * Time.deltaTime);

    }
    /// <summary>
    /// Calculate the position where the foot will be at the top of it's arc in the step cycle.
    /// </summary>
    /// <returns>Returns a vector3 posittion for the top of the step cycle</returns>
    private Vector3 CalculateLiftPos()
    {
        //Get a straight line vector between the current position of this foot and the position of it's _footPlacementTarget's
 //position , then divide by 2 and add to the current position to get a point halfway to your destination.
        Vector3 halfwayPoint = transform.position +( _plantingPoint - transform.position ) /2;
        
        /* 
         Calculate how much the halfway point needs to shift to be at the step height perpendicular to the line between the current and target
        positions
         */
        Vector3 liftVector = transform.up * _stepHeight;
        
      //  _debug_halfway_point= liftVector + halfwayPoint;
        //Add the liftvector to the halfway point to get the final position for the lift point
        return liftVector + halfwayPoint;
    }
    //TODO: Remove this code in production when debugging the walk cycle is no longer neccesary
    private void OnDrawGizmos()
    {
        //Draw a blue sphere where the calculated halfway point is for debugging
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(halfway_point, .1f);

        //Draw a yellow sphere at the point where the foot is currently heading
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_nextTarget,.3f);
    }
   Vector3 GetTerrain( Vector3 targetedPosition)
    {
       
        RaycastHit2D _terrainHit = Physics2D.Raycast(_bodyTransform.position, _footPlacementTarget.position - _bodyTransform.position, _raycastDepth);
        Debug.DrawRay(_bodyTransform.position, _footPlacementTarget.position - _bodyTransform.position, Color.white, 1f);
       Debug.Log("Terrain Hit result point " + _terrainHit.point + " layer name is " + _terrainLayer.ToString());
       
        if (_terrainHit.point == Vector2.zero) return targetedPosition; // If the hit does not register any hits then just return the targeted pos
        
        return _terrainHit.point;
    }
}
