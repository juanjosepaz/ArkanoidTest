using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb2D;

    [Header("Movement Values")]
    [SerializeField] private float movementSpeed;
    private bool canMove;
    private Vector3 moveDirection;

    private void Update()
    {
        HandleMovementInput();
    }

    private void FixedUpdate()
    {
        if (!canMove) { return; }

        rb2D.MovePosition(transform.position + movementSpeed * Time.deltaTime * moveDirection);
    }

    private void HandleMovementInput()
    {
        moveDirection = inputReader.MoveDirection;
    }

    public void SetPlayerCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

}
