using Pixel.Game.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(TimerManager))]
public class GhostPlayer : Player {

    protected override void Awake()
    {
        base.Awake();
        this.playerType = EnumTypes.PlayerType.Ghost;
    }

    protected override void Update()
    {
        base.Update();        
    }


    public override void setPlayerType(int pType)
    {
        this.playerType = EnumTypes.PlayerType.Ghost;
    }
}
