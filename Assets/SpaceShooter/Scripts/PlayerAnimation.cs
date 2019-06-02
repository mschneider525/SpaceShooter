using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator = null;
    private string _playerDesignation = null;

    private string _horizontalAxis = "";

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerDesignation = this.transform.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerDesignation == "Player1(Clone)")
        {
            _horizontalAxis = "Horizontal";
        }
        else if (_playerDesignation == "Player2(Clone)")
        {
            _horizontalAxis = "Horizontal2";
        }
        else //if (_playerDesignation == "Player(Clone)")
        {
            _horizontalAxis = "Horizontal";
        }

        if (Input.GetAxis(_horizontalAxis) < 0.0f) //Turn Left
        {
            _animator.SetBool("TurnLeft", true);
            _animator.SetBool("TurnRight", false);
        }
        if (Input.GetAxis(_horizontalAxis) > 0.0f) //Turn Right
        {
            _animator.SetBool("TurnRight", true);
            _animator.SetBool("TurnLeft", false);
        }
        if (Input.GetAxis(_horizontalAxis) == 0.0f) //Idle
        {
            _animator.SetBool("TurnRight", false);
            _animator.SetBool("TurnLeft", false);
        }
    }
}
