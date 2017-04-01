using UnityEngine;
using System.Collections;
using InControl;
using Pixel.Game.Management;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(TimerManager))]
public class Player : Entity
{
    public InputDevice Device { get; set; }
    public float Speed;
    private float m_BaseSpeed = 100f;
    private EnumTypes.PlayerEnum m_PlayerNumber;
    private KeyCode m_ControlKeyUp, m_ControlKeyDown, m_ControlKeyLeft, m_ControlKeyRight;
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Direction = Vector2.zero;
    protected int playerType;
    protected Color playerColor;

    public TimerManager PlayerTimer;

    protected virtual void Awake()
    {
        m_PlayerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_Rigidbody = GetComponent<Rigidbody2D>();    
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void OnCollisionEnter2D(Collision2D col) { } //To be inherited by ghosts / pacman

    protected virtual void Update()
    {
        if (m_PlayerNumber != EnumTypes.PlayerEnum.Unassigned)
        {
            m_Direction = Vector2.zero;
            if (Input.GetKey(m_ControlKeyUp))
            {
                m_Direction += new Vector2(0, m_BaseSpeed * Speed * Time.deltaTime);               
            }
            if (Input.GetKey(m_ControlKeyDown))
            {
                m_Direction += new Vector2(0, -m_BaseSpeed * Speed * Time.deltaTime);
            }
            if (Input.GetKey(m_ControlKeyLeft))
            {
                m_Direction += new Vector2(-m_BaseSpeed * Speed * Time.deltaTime, 0);
            }
            if (Input.GetKey(m_ControlKeyRight))
            {
                m_Direction += new Vector2(m_BaseSpeed * Speed * Time.deltaTime, 0);
            }

            m_Rigidbody.velocity = m_Direction;

            if (!Input.GetKey(m_ControlKeyUp) && !Input.GetKey(m_ControlKeyDown) && !Input.GetKey(m_ControlKeyLeft) && !Input.GetKey(m_ControlKeyRight))
            {
                m_Direction = Vector2.zero;
            }
            
        }
    }

    public void SetPlayerNumber(int number)
    {
        switch (number)
        {
            case 0:
                m_PlayerNumber = EnumTypes.PlayerEnum.P1;
                m_ControlKeyUp = KeyCode.W;
                m_ControlKeyDown = KeyCode.S;
                m_ControlKeyLeft = KeyCode.A;
                m_ControlKeyRight = KeyCode.D;
                playerColor = Color.red;
                break;

            case 1:
                m_PlayerNumber = EnumTypes.PlayerEnum.P2;
                m_ControlKeyUp = KeyCode.UpArrow;
                m_ControlKeyDown = KeyCode.DownArrow;
                m_ControlKeyLeft = KeyCode.LeftArrow;
                m_ControlKeyRight = KeyCode.RightArrow;
                playerColor = Color.blue;
                break;

            case 2:
                m_PlayerNumber = EnumTypes.PlayerEnum.P3;
                m_ControlKeyUp = KeyCode.Y;
                m_ControlKeyDown = KeyCode.H;
                m_ControlKeyLeft = KeyCode.G;
                m_ControlKeyRight = KeyCode.J;
                playerColor = Color.white;
                break;

            case 3:
                m_PlayerNumber = EnumTypes.PlayerEnum.P4;
                m_ControlKeyUp = KeyCode.O;
                m_ControlKeyDown = KeyCode.L;
                m_ControlKeyLeft = KeyCode.K;
                m_ControlKeyRight = KeyCode.Semicolon;
                playerColor = Color.yellow;
                break;
        }
    }

    public int GetPlayerNumber()
    {
        return (int)m_PlayerNumber;
    }

    public virtual void setPlayerType(int pType)
    {
        playerType = pType;
    }

}