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


    protected override void OnCollisionEnter2D(Collision2D col)
    {       
        if (col.gameObject.GetComponent<GhostPlayer>() && col.gameObject.GetComponent<GhostPlayer>().enabled == true && gameObject.GetComponent<PacmanPlayer>().enabled == true)
        {
            PlayerTimer.timerLocked = true;
            GameManager.GetInstance().ResetGame();
        }
    }

    public override void setPlayerType(int pType)
    {
        this.playerType = EnumTypes.PlayerType.Ghost;
    }
}
