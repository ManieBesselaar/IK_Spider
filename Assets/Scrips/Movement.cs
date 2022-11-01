using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float _movementSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetAxis("Horizontal") != 0)
        {
            transform.position += Input.GetAxis("Horizontal") * _movementSpeed * Time.deltaTime * -transform.up;
        }  
    }
}
