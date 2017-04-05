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
        //Ghost collision
        if (col.gameObject.GetComponent<GhostPlayer>() && col.gameObject.GetComponent<GhostPlayer>().enabled == true && gameObject.GetComponent<PacmanPlayer>().enabled == true)
        {
            PlayerTimer.timerLocked = true;
            GameManager.GetInstance().ResetGame();
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        //Crystal collision
        if (col.gameObject.GetComponent<Crystal>() && gameObject.GetComponent<PacmanPlayer>().enabled == true)
        {
            playerScore += col.gameObject.GetComponent<Crystal>().value;
            col.gameObject.GetComponent<Crystal>().EnableCrystal(false);
        }
    }

    public override void setPlayerType(int pType)
    {
        this.playerType = EnumTypes.PlayerType.Ghost;
    }
}
