using UnityEngine;
using System.Collections;
using InControl;
using Pixel.Game.Management;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player : Entity
{
    public InputDevice Device { get; set; }

    private EnumTypes.PlayerEnum m_PlayerNumber;
    private KeyCode m_ControlKeyUp, m_ControlKeyDown, m_ControlKeyLeft, m_ControlKeyRight;
    private Rigidbody2D m_Rigidbody;

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
                break;

            case 1:
                m_PlayerNumber = EnumTypes.PlayerEnum.P2;
                m_ControlKeyUp = KeyCode.UpArrow;
                m_ControlKeyDown = KeyCode.DownArrow;
                m_ControlKeyLeft = KeyCode.LeftArrow;
                m_ControlKeyRight = KeyCode.RightArrow;
                break;

            case 2:
                m_PlayerNumber = EnumTypes.PlayerEnum.P3;
                m_ControlKeyUp = KeyCode.Y;
                m_ControlKeyDown = KeyCode.H;
                m_ControlKeyLeft = KeyCode.G;
                m_ControlKeyRight = KeyCode.J;
                break;

            case 3:
                m_PlayerNumber = EnumTypes.PlayerEnum.P4;
                m_ControlKeyUp = KeyCode.O;
                m_ControlKeyDown = KeyCode.L;
                m_ControlKeyLeft = KeyCode.K;
                m_ControlKeyRight = KeyCode.Semicolon;
                break;
        }
    }

    void Awake()
    {
        m_PlayerNumber = EnumTypes.PlayerEnum.Unassigned;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        SetPlayerNumber(0);
    }

    protected override void Start()
    {
        base.Start();

    }

    void Update()
    {
        if (m_PlayerNumber != EnumTypes.PlayerEnum.Unassigned)
        {
            if (Input.GetKey(m_ControlKeyUp))
            {
                //m_Rigidbody.AddForce(Vector2.up * 100f * Time.deltaTime);
                m_Rigidbody.velocity = new Vector2(0, 100f * Time.deltaTime);
            }

            else if (Input.GetKey(m_ControlKeyDown))
            {
                m_Rigidbody.velocity = new Vector2(0, -100f * Time.deltaTime);
            }

            else if (Input.GetKey(m_ControlKeyLeft))
            {
                m_Rigidbody.velocity = new Vector2(-100f * Time.deltaTime, 0);
            }

            else if (Input.GetKey(m_ControlKeyRight))
            {
                m_Rigidbody.velocity = new Vector2(100f * Time.deltaTime, 0);
            }

            else
            {
                m_Rigidbody.velocity = Vector2.zero;
            }
        }
    }
}