﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : FSMState
{

    private float jumpTimeCounter;//计时器
    private float jumpTime = 0.27f;//最大跳跃时间
    private bool isRun;
    private Vector2 velocity;



    public JumpState()
    {
        stateID = StateID.Jump;

        AddTransition(Transition.ReMove, StateID.Move);
        AddTransition(Transition.SlidePress, StateID.Slide);
    }

    public override void DoBeforeEntering(Player player)
    {
        player.slideJump = false;
        player.offJump = false;
        player.playerRigidbody.gravityScale = 0f;

        jumpTimeCounter = jumpTime;

        if (Mathf.Abs(player.playerRigidbody.velocity.x) > 0)
            isRun = true;

        Debug.Log("jump enter");
    }


    public override void InputHandle(Player player)
    {

    }

    public override void Update(Player player)
    {

        if (player.fsm.LastState is SlideState)
        {
            if (player.onLeftWall && player.forward == -1 || player.onRightWall && player.forward == 1)
            {
                player.StartCoroutine("SlideJump");
                if (player.slideJump)
                    player.fsm.PerformTransition(Transition.SlidePress, player);
            }

            else
            {
                player.StartCoroutine("OffJump");
                if (player.offJump)
                    player.fsm.PerformTransition(Transition.ReMove, player);
            }

        }


        else if (Input.GetKey(KeyCode.C) && jumpTimeCounter > 0)//不能用while,会在一帧中执行完
        {
            velocity.y = player.jumpCurve.Evaluate(jumpTimeCounter) * player.jumpSpeed;

            if (isRun)
                velocity.x = player.runCurve.Evaluate(player.timeCounter) * Input.GetAxisRaw("Horizontal") * player.moveSpeed;
            else
                velocity.x = player.moveBase * Input.GetAxisRaw("Horizontal") * player.moveSpeed;

            player.playerRigidbody.velocity = velocity;

            jumpTimeCounter -= Time.fixedDeltaTime;
        }

        else//如果不加else会只执行一次
            player.fsm.PerformTransition(Transition.ReMove, player);
    }




    public override void DoBeforeLeaving(Player player)
    {
        player.playerRigidbody.gravityScale = player.normalGravity;
        Debug.Log("jump finish");
    }

}