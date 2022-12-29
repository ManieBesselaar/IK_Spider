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
   public enum StepPhase {
        PLANTED, //Foot is planted and waiting to take the next step
        LIFTING,//Moving towards highest point in stride
        MOVING_TO_PLANT//Moving the foot to its final position for this stride
    }
    // Set the current step phase to moving to plant. This wil cause it to try and reach the _placementTarget position at the start
    // of your scene
    StepPhase _stepPhase = StepPhase.MOVING_TO_PLANT;

   public StepPhase CurrentStepPhase { get => _stepPhase; private set { } } //Public variable to allow other objects to detect the step phase of this foot

    
    [SerializeField][Tooltip("How fast the foot should move in units per second.")] float _footSpeed = 2f;
    Vector2 _nextTarget = Vector2.zero; //Initialized to 0 vector to avoid potential null errors

    [SerializeField][Tooltip("How high the foot should lift on each stride")] float StepHeight = 1.2f; 

    [SerializeField] float DistanceTolerance = .2f; // How close to a point the gameobject (this ik target) should be to change state 


    #endregion

    [Header("Terrain detection variables")]
    #region Video5 terrain detection 
    [SerializeField]
    [Tooltip("The transform of the object from where the raycast for this foot should originate. This is used for terrain detection")]
    Transform _castPosition;
    [SerializeField][Tooltip ("Lenth of the ray to cast for ground detection.")] float _raycastDepth = 5;
    [SerializeField][Tooltip("Layer assigned to the ground")]LayerMask _terrainLayer;

    [SerializeField]
    [Tooltip("The footMover component of a foot opposite this one which needs to be on the ground for this foot to move")] 
    FootMover _opposingFoot;  //This helps make the walk cycle a little more realistic
    #endregion
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {

        /*
         If this IKTarget is further from the placement target than the stepsize 
        then start moving to the lift highest point of my stride.
         */
        if (Vector2.Distance(transform.position, _placementTarget.position) > _stepSize 
            && _stepPhase == StepPhase.PLANTED
           && _opposingFoot.CurrentStepPhase == StepPhase.PLANTED
            )
        {

            _stepPhase = StepPhase.LIFTING;
            _nextTarget = FindLiftPosition();  

        }
        /*
         If we have reached the highest point of our stride then check for the ground close to the
        _placementTarget and set that as our next target
         */
        if (Vector2.Distance(transform.position, _nextTarget) <= DistanceTolerance && _stepPhase == StepPhase.LIFTING)
        {
            _stepPhase = StepPhase.MOVING_TO_PLANT;
            _nextTarget = GetGroundPoint();
        }
        
        if (Vector2.Distance(transform.position, _nextTarget) <= DistanceTolerance && _stepPhase == StepPhase.MOVING_TO_PLANT)
        {
            //Our foot is planted and we will wait for a new step to start
            _stepPhase = StepPhase.PLANTED;
            
        }
        MoveFoot(); //Move the foot towards the location of _nextTarget. This gets called every frame.
    }
    /// <summary>
    /// This function moves the foot towards the desired location at a rate determined by the set footSpeed
    /// </summary>
    private void MoveFoot()
    {
        
        transform.position=Vector2.MoveTowards(transform.position, _nextTarget,_footSpeed * Time.deltaTime);
    }
    /// <summary>
    /// This determines the highest point in our stride halfway between the start 
    /// and end point of our stride.
    /// </summary>
    /// <returns>A Vector2 co-ordinate for the high point of our stride</returns>
    Vector2 FindLiftPosition()
    {
        Vector2 differenceVector = _placementTarget.position - transform.position;
        Vector2 liftTarget = (differenceVector /2) + (Vector2.up * StepHeight);

        return (Vector2)transform.position + liftTarget ;
    }
    private void OnDrawGizmos()
    {
        //Set gizmo colour to red and draw a sphere where the _nextTarget coordinate specifies
        //This is useful for debugging anomolies in the walk cycle
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_nextTarget, .4f);
    }
    /// <summary>
    /// This function does a ray cast from _castPosition to the _placementTarget position
    /// while filtering the results to only return a hit for the specified terrain layer
    /// </summary>
    /// <returns>Vector2d point where the cast hit the terrain layer</returns>
  Vector2 GetGroundPoint()
    {
        RaycastHit2D _terrainHit = Physics2D.Raycast(_castPosition.position, _placementTarget.position - _castPosition.position,
            _raycastDepth, _terrainLayer);

        //Draw a ray in the editor of the same direction and length as the actual cast ray to help debug
        // terrain detection anomolies
        Debug.DrawRay(_castPosition.position,( _placementTarget.position - _castPosition.position).normalized * _raycastDepth, 
            Color.white, 1f);

        Debug.Log("Terrain Hit result point " + _terrainHit.point);
       
        //If no hit was recorded move the feet to the default foot placement position relative to the character body.
        if (_terrainHit.collider != null)
        {
            return _terrainHit.point;
        }
        else
        {
            return _placementTarget.position;
        }
    }
}
