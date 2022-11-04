using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class MoveScript : NetworkBehaviour
{
    private CharacterController controller;
    public float speed;

    Vector3 input;
    Vector3 move;

    AccesTitan accesTitanScript;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        accesTitanScript = GetComponent<AccesTitan>();
    }

    void Update()
    {
        if (!base.IsOwner)
            return;

        if (!accesTitanScript.inTitan)
        {
            HandleInput();
            GroundedMovement();
            controller.Move(move * Time.deltaTime);
        }
            
    }

    void HandleInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        input = transform.TransformDirection( input );
        input = Vector3.ClampMagnitude( input, 1f );
    }
        
    void GroundedMovement()
    {
        if ( input.x != 0 )
        {
            move.x += input.x * speed;
        }
        else
        {
            move.x = 0;
        }
        if ( input.z != 0 )
        {
            move.z += input.z * speed;
        }
        else
        {
            move.z = 0;
        }
 
        move = Vector3.ClampMagnitude( move, speed );
    }



}
