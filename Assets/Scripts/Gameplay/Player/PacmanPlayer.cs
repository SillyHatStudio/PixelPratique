using Pixel.Game.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(TimerManager))]
public class PacmanPlayer : Player {

    [HideInInspector]
    public bool isDead;

    protected override void Awake()
    {
        base.Awake();
        this.playerType = EnumTypes.PlayerType.Pacman;
        isDead = false;
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<GhostPlayer>() != null)
        {
            isDead = true;
        }
    }
}
