using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float turnSpeed = 360; // full turn in a second
    private Vector3 input;

    private void Update()
    {
        GatherInput();
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void Look()
    {
        if (input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        if (input == Vector3.zero)
        {
            if (playerAnim.GetBool("Static_b"))
            {
                playerAnim.SetBool("Static_b", false);
                playerAnim.SetFloat("Speed_f", 0);
            }
            return;
        } 

        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime); // * input.normalized.magnitude
        playerAnim.SetBool("Static_b", true);
        playerAnim.SetFloat("Speed_f", 0.3f);
    }
}


