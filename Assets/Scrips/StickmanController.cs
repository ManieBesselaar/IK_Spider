using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanController : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField]
    [Tooltip("The amount of force to ad to the rigidbody to move it left or right when the input is pressed")]
    float _walkForce = 10;
    [SerializeField]
    [Tooltip("The maximum horizontal speed of our stickman")]
    float _maxSpeed = 5;
    Animator _animator;

    [SerializeField]
    [Tooltip("Audioclips to play during animation.")]
    AudioClip[] _clips;

    AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
        _animator = GetComponent<Animator>();
        _rb= GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        float _inputAmmount = Input.GetAxis("Horizontal");//Get the horizontal input
       
        if (_inputAmmount != 0 && Mathf.Abs( _rb.velocity.x )< _maxSpeed)
        {
            
            Debug.Log("Input ammount " + _inputAmmount + " result force " + (transform.right * _walkForce * _inputAmmount * Time.fixedDeltaTime));
            _rb.AddForce(transform.right * _walkForce * _inputAmmount *Time.fixedDeltaTime);
            _animator.SetBool("IsWalking", true);
        }
       if(_rb.velocity.x < .1f)
        {
            _animator.SetBool("IsWalking", false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetTrigger("Scratch");
        }
    }

    //These audio functions can be much better and robust, but will do for a quick scene
    public void PlayStep()
    {
        _audioSource.clip = _clips[0];
        _audioSource.Play();
    }
    public void PlayMMM() {
        _audioSource.clip = _clips[1];
        _audioSource.Play();
    }

    public void PlayBlink()
    {
        _audioSource.clip = _clips[2];
        _audioSource.Play();
    }
    public void PlaySqueek()
    {
        _audioSource.clip = _clips[3];
        _audioSource.Play();
    }
}
