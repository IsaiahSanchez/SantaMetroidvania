﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActions Inputs;

    private PlayerMain mainPlayer;
    private PlayerMovement myMovement;
    private FacingHandler playerFacing;

    private void Awake()
    {
        Inputs = new InputActions();   
    }

    private void OnEnable()
    {
        
        mainPlayer = GetComponent<PlayerMain>();
        myMovement = GetComponent<PlayerMovement>();
        playerFacing = GetComponent<FacingHandler>();

        Inputs.Enable();
        Inputs.Player.Jump.started += HandleStartJumping;
        Inputs.Player.Jump.canceled += HandleStopJumping;
        Inputs.Player.Jump.Enable();

        //Inputs.Player.Move.performed += HandleMovement;
        Inputs.Player.Move.Enable();

        Inputs.Player.Dash.performed += HandleDash;
        Inputs.Player.Dash.Enable();

        Inputs.Player.Throw.performed += HandleThrow;
        Inputs.Player.Throw.Enable();
    }

    private void OnDisable()
    {
        Inputs.Player.Jump.Disable();
        Inputs.Player.Move.Disable();
        Inputs.Player.Dash.Disable();
        Inputs.Player.Throw.Disable();
    }

    private void Update()
    {
        if (mainPlayer.isDead == false)
        {
            Vector2 aimDirection = Inputs.Player.Move.ReadValue<Vector2>();
            playerFacing.playerFaceOtherDirection(aimDirection);
            myMovement.setMovmentVector(aimDirection);
        }
    }

    private void HandleStartJumping(InputAction.CallbackContext context)
    {
        if (mainPlayer.isDead == false)
        {
            myMovement.handleStartJump();

        }
    }

    private void HandleStopJumping(InputAction.CallbackContext context)
    {
        if (mainPlayer.isDead == false)
        {
            myMovement.handleStopJump();

        }
    }


    //private void HandleMovement(InputAction.CallbackContext context)
    //{
    //    Vector2 temp = context.ReadValue<Vector2>();
    //    myMovement.setMovmentVector(temp);
    //}


    private void HandleDash(InputAction.CallbackContext context)
    {
        if (mainPlayer.isDead == false)
        {
            myMovement.handleDashInput();

        }
    }


    private void HandleThrow(InputAction.CallbackContext context)
    {
        if (mainPlayer.isDead == false)
        {
            mainPlayer.tryThrowSnowball();

        }
    }
}
