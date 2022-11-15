using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   
    [SerializeField][Tooltip("How fast the spider moves to it's left and right")] float _movementSpeed = 5f;
    [SerializeField]
    [Tooltip("How far ahead should the foot placement targets be advanced when moving")] float _footPlacementTargetAdcance = 1f;
    [SerializeField] Transform _footPlacementTargetsParent;
    float _currentInput = 0;
    Vector2 _placementTargetsInitialPos;
    // Start is called before the first frame update
    void Start()
    {
        _placementTargetsInitialPos = _footPlacementTargetsParent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _currentInput = Input.GetAxis("Horizontal"); 
        //Current unput will have a value between -1 and 1, negative values indicat a left movement and positi
      if(_currentInput != 0) //Only move if there is a control input
        {
            //Shift the footPlacement targets to left or right depending on walk direction and rate
            _footPlacementTargetsParent.localPosition =_placementTargetsInitialPos + (_currentInput * _footPlacementTargetAdcance * Vector2.right);  
            transform.position += _currentInput * _movementSpeed * Time.deltaTime * transform.right;
        }
     
    }
  
}

