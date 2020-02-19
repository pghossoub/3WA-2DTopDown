﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum State
    {
        IDLE,
        RUNNING,
        SPRINTING,
        ROLLING
    }

    #region Serialized
    #pragma warning disable CS0649
    [SerializeField]
    private float _speed;
    #pragma warning restore CS0649
    #endregion

    private Rigidbody2D _rb;
    private Animator _anim;
    private float _horizontal;
    private float _vertical;
    private State _state;
    private Vector2 _lastDirection;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        switch (_state)
        {
            case (State.IDLE):
                Move();
                break;

            case (State.RUNNING):
                Move();
                SaveLastDirection();
                Stop();

                //Roll();
                break;

            case (State.SPRINTING):
                //Move();
                //Roll();
                break;

            case (State.ROLLING):
                break;
        }
    }

    private static void Roll()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Roll");
        }
    }

    private void Move()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (_horizontal != 0 || _vertical != 0)
        {
            _state = State.RUNNING;
            _anim.SetBool(Animator.StringToHash("IsRunning"), true);
            _anim.SetFloat(Animator.StringToHash("VelocityX"), _horizontal);
            _anim.SetFloat(Animator.StringToHash("VelocityY"), _vertical);
        }
    }

    private void SaveLastDirection()
    {
        /*if (_horizontal != 0 || _vertical != 0)
        {
            _lastDirection = new Vector2(_horizontal, _vertical);
            _anim.SetFloat(Animator.StringToHash("LastX"), _lastDirection.x);
            _anim.SetFloat(Animator.StringToHash("LastY"), _lastDirection.y);
        }*/

        _lastDirection = new Vector2(_horizontal, _vertical);


        if (_lastDirection.sqrMagnitude > 0.5)
        {
            _lastDirection = NormalizeVector(_lastDirection);
            _anim.SetFloat(Animator.StringToHash("LastX"), _lastDirection.x);
            _anim.SetFloat(Animator.StringToHash("LastY"), _lastDirection.y);
        }

    }

    private void Stop()
    {
        if (_horizontal == 0 && _vertical == 0)
        {
            _state = State.IDLE;
            _anim.SetBool(Animator.StringToHash("IsRunning"), false);
        }
    }

    private Vector2 NormalizeVector(Vector2 vector)
    {
        if (vector.x > 0)
            vector.x = 1;
        if (vector.x < 0)
            vector.x = -1;
        if (vector.y > 0)
            vector.y = 1;
        if (vector.y < 0)
            vector.y = -1;

        return new Vector2(vector.x, vector.y);
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * _speed * Time.fixedDeltaTime,
            _vertical * _speed * Time.fixedDeltaTime);
    }

    void OnGUI()
    {
        GUI.Button(new Rect(10, 10, 120, 30), $"State: {_state}");
    }
}
