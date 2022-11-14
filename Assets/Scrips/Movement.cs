using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
   
    [SerializeField][Tooltip("How fast the spider moves to it's left and right")] float _movementSpeed = 5f;
    float _currentInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentInput = Input.GetAxis("Horizontal"); 
        //Current unput will have a value between -1 and 1, negative values indicat a left movement and positi
      if(_currentInput != 0) //Only move if there is a control input
        {
            
            transform.position += _currentInput * _movementSpeed * Time.deltaTime * transform.right;
        }
     
    }
  
}

