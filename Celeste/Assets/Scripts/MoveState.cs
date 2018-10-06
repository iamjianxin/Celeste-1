﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IBaseState
{


    private Player player;
    public MoveState(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.playerRigidbody.gravityScale = player.normalGravity;
        Debug.Log("move enter");

    }

    public void Update () 
    {

        Vector2 velocity = player.playerRigidbody.velocity;
        float h = Input.GetAxisRaw("Horizontal");
        if (player.onWall)
        {
            if (player.forward == -1)
            {
                if (h < 0)
                    player.playerRigidbody.gravityScale = 1;
                else
                    player.playerRigidbody.velocity = new Vector2(h * player.moveSpeed, velocity.y);

            }
            if (player.forward == 1)
            {
                if (h > 0)
                    player.playerRigidbody.gravityScale = 1;
                else
                    player.playerRigidbody.velocity = new Vector2(h * player.moveSpeed, velocity.y);

            }


        }

        else
        {
            player.playerRigidbody.velocity = new Vector2(h * player.moveSpeed, velocity.y);

        }


        if (Input.GetKeyDown(KeyCode.C) && (player.onGround || player.onWall))
            player.SetPlayerState(new JumpState(player));
        else if (player.canDash && Input.GetKeyDown(KeyCode.X))
            player.SetPlayerState(new DashState(player));
        else if (player.onWall && Input.GetKey(KeyCode.Z))
            player.SetPlayerState(new SlideState(player));

    }

    public void Finish()
    {
        Debug.Log("move finish");
    }


}
