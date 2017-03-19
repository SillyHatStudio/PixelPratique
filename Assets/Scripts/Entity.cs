using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
   
    public enum State
    {
        Sleeping, // When the entity is not activated
        Idle,     // When the entity is not moving
        Dead      // When the entity is dead
    };

    public State CurrentState { get; set; }
  
  
    protected virtual void Start()
    {       
        CurrentState = State.Sleeping;   
    }

   
  
}