﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : FSMState
{

    private Player player;

    public MoveState(Player player)
    {
        stateID = StateID.Move;
        this.player = player;
        AddTransition(Transition.JumpPress, StateID.Jump);
        AddTransition(Transition.DashPress, StateID.Dash);
        AddTransition(Transition.SlidePress, StateID.Slide);
    }

    public override void DoBeforeEntering()
    {

        player.playerRigidbody.gravityScale = player.normalGravity;

        Debug.Log("move enter");

        if (player.fsm.LastState is SlideState && Input.GetKey(KeyCode.UpArrow))
        {
            player.StartCoroutine("SlideMove");
        }
    }

    public override void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            player.buffer_counter = 0;
        }

        if (player.buffer_counter < player.buffer_max)
        {
            player.buffer_counter++;
            if (Input.GetKey(KeyCode.C) && player.onGround || player.onWall)
                player.fsm.PerformTransition(Transition.JumpPress);
        }



        if (player.coyote_counter < player.coyote_max) 
        {
            if (Input.GetKeyDown(KeyCode.C) && player.coyote_counter < player.coyote_max)
            {
                player.fsm.PerformTransition(Transition.JumpPress);
            }
            player.coyote_counter++;
        }

        if (Input.GetKeyDown(KeyCode.X) && player.canDash)
            player.fsm.PerformTransition(Transition.DashPress);
        if (Input.GetKeyDown(KeyCode.Z) && player.slideTime > 0 && player.onWall)
            player.fsm.PerformTransition(Transition.SlidePress);

    }

    public override void Act()
    {
        Vector2 velocity = player.playerRigidbody.velocity;
        if (player.onLeftWall && Input.GetKey(KeyCode.LeftArrow) || player.onRightWall && Input.GetKey(KeyCode.RightArrow))
        {
            player.playerRigidbody.velocity = new Vector2(0, -1);
        }
        else
        {
            player.playerRigidbody.velocity = new Vector2(player.moveBase * Input.GetAxisRaw("Horizontal") * player.moveSpeed, velocity.y);
        }



    }

    public override void DoBeforeLeaving()
    {
        Debug.Log("move finish");
    }
}
