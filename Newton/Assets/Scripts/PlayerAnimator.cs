using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private float maxSpeed = 6f;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude / maxSpeed);
    }
}
