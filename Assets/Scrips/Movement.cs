using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Body Movement Parameters")]
    [SerializeField][Tooltip("How fast the spider moves to it's left and right")] float _movementSpeed = 5f;

    [SerializeField][Tooltip("This determines the max walk speed for our spider")] float _maximumCrawlVelocity = 20;
    float _currentInput = 0;


    [Header("Foot placement parameters")]
    [SerializeField]
    [Tooltip("How far ahead should the foot placement targets be advanced when moving")] float _footPlacementTargetAdcance = 1f;
    [SerializeField] Transform _footPlacementTargetsParent;
    Vector2 _placementTargetsInitialPos;

    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _placementTargetsInitialPos = _footPlacementTargetsParent.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        _currentInput = Input.GetAxis("Horizontal");
        //Current input will have a value between -1 and 1, negative values indicate a left movement and positive right
        if (_currentInput != 0) //Only move if there is a control input
        {
            //Shift the footPlacement targets to left or right depending on walk direction and rate


            _footPlacementTargetsParent.localPosition = _placementTargetsInitialPos +
                (_currentInput * _footPlacementTargetAdcance * Vector2.right);

            if (_rb.velocity.sqrMagnitude < _maximumCrawlVelocity)
            {
                _rb.AddForce(transform.right * _currentInput * _movementSpeed);
            }

           // Debug.Log("Body velocity " + _rb.velocity.sqrMagnitude);
        }

    }
    private void Reset()
    {
            
    }

}



/*
 * Keep this for later use
[SerializeField]
[Tooltip("How far ahead should the foot placement targets be advanced when moving")] float _footPlacementTargetAdcance = 1f;
[SerializeField] Transform _footPlacementTargetsParent;
Vector2 _placementTargetsInitialPos;
 void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _placementTargetsInitialPos = _footPlacementTargetsParent.localPosition;
    }


//Shift the footPlacement targets to left or right depending on walk direction and rate


            _footPlacementTargetsParent.localPosition =_placementTargetsInitialPos + (_currentInput * _footPlacementTargetAdcance * Vector2.right);
          
*/