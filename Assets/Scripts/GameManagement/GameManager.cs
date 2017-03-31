using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;

public class GameManager : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Members:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    private static GameManager INSTANCE__ = null;

    public enum GameState
    {
        None,         // 0) Default state.
        SplashScreen, // 1) Display the dev logo.
        MainMenu,     // 2) Launch the gamplay.
        CharSelect,   // 3) Select each player.
        Game          // 4) Actual gameplay.
    };

    private GameState CurrentState { get; set; }

    // Player related members:
    private const int MAX_PLAYERS__ = 4;
    private List<Player> m_players = new List<Player>(MAX_PLAYERS__);
    public GameObject m_PlayerPrefab;
    bool m_LockPlayersInput = true;

    public int m_NumberOfRound = 4;

    // SplashScreen stage members:
    public GameObject m_SplashScreenObject;
    private SplashScreen m_SplashScreen;

    // Menu stage members:
    public GameObject m_MenuObject;
    private Menu m_Menu;

    //for animation
    public GameObject[] m_PlayerAnimationList;

    private bool m_PlayerReady = false;
    public GameObject[] m_PlayerList;
    


    ////////////////////////////////////////////////////////////////////////////////
    // Callbacks:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    void Awake()
    {
        Debug.Log("Awake GameManager");
        if (INSTANCE__ == null)
        {
            INSTANCE__ = this;
            DontDestroyOnLoad(INSTANCE__.gameObject);
            if (!InitGame())
            {
                Debug.LogError("Unable to init game!");
            }
        }
        else
        {
            Debug.LogError("GameManager already exists!");
        }
    }


    void Update()
    {
        switch (CurrentState)
        {
            case GameState.SplashScreen:
                if (m_SplashScreen.IsDone())
                {
                    SetCurrentState(GameState.MainMenu);
                }

                m_LockPlayersInput = true;
                break;

            case GameState.MainMenu: 
                    if(Input.GetKeyDown(KeyCode.Return))
                    {
                        m_PlayerList = new GameObject[4];
                        for (int i = 0; i < 4; i++)
                        {
                            GameObject player = PoolManager.instance.GetPoolObject("Player");
                             m_PlayerList[i] = player;
                            //player.GetComponent<Player>().SetPlayerNumber(i);
                        }
                        SetCurrentState(GameState.Game);
                    } 
                
                break;

            case GameState.CharSelect:	
				

            	break;

            case GameState.Game:
               

              
                break;

            default:
                Debug.LogError("Undefined game state!");
                SetCurrentState(GameState.SplashScreen);
                break;
        }
    }

    public void PerformRippleEffect(Vector3 wpos)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera)
        {
            Vector3 spos = camera.GetComponent<Camera>().WorldToViewportPoint(wpos);
            RippleEffect fx = camera.GetComponent<RippleEffect>();
            fx.Emit(spos.x, 1-spos.y);
        }
    }
    public void PerformScreenShake(float duration)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        if (camera)
        {
            CameraShake fx = camera.GetComponent<CameraShake>();
            fx.m_ShakeDuration = duration;
        }
    }
   
    void OnStateEnter(GameState state)
    {
        Debug.Log("OnStateEnter " + state);
        switch (state)
        {
            case GameState.SplashScreen:
                {
                    GameObject o = (GameObject)Instantiate(m_SplashScreenObject);
                    m_SplashScreen = o.GetComponent<SplashScreen>();
                }
                break;

            case GameState.MainMenu:
                {                  
                    GameObject o = (GameObject)Instantiate(m_MenuObject);
                    m_Menu = o.GetComponent<Menu>();
                }
                break;

            case GameState.Game:
                {   
                    SceneManager.LoadScene(1);
                }
                break;
        }
    }   
    void OnStateLeave(GameState state)
    {
        Debug.Log("OnStateLeave " + state);
        switch (state)
        {
            case GameState.SplashScreen:
                Destroy(m_SplashScreen.gameObject);
                m_SplashScreen = null;
                break;

            case GameState.MainMenu:
                Destroy(m_Menu.gameObject);
                m_Menu = null;
                break;

            case GameState.Game:
                ResetPlayers();             
                break;
        }


    }
    private bool JoinButtonWasPressedOnDevice(InputDevice input_device)
    {
        return (
            input_device.Action1.WasPressed ||
            input_device.Action2.WasPressed ||
            input_device.Action3.WasPressed ||
            input_device.Action4.WasPressed
        );
    }    
    private Player FindPlayerUsingDevice(InputDevice input_device)
    {
        for (int i = 0; i < m_players.Count; ++i)
        {
            Player player = m_players[i];
            if (player.Device == input_device)
            {
                return player;
            }
        }

        return null;
    }
    private Player CreatePlayer(InputDevice input_device)
    {
        if (m_players.Count < MAX_PLAYERS__)
        {
            Debug.Log("A new player join the game!");

            // instantiate player object
            GameObject player_object = (GameObject)Instantiate(m_PlayerPrefab);
            GameObject.DontDestroyOnLoad(player_object);

            // retrieve player component
            Player component = player_object.GetComponent<Player>();
            component.Device = input_device;          

            m_players.Add(component);

            return component;
        }

        return null;
    }    
    void CheckForPlayers()
    {
        // get current active device
        InputDevice active_device = InputManager.ActiveDevice;

        // check if device is active
        if (JoinButtonWasPressedOnDevice(active_device))
        {
            if (FindPlayerUsingDevice(active_device) == null)
            {
                CreatePlayer(active_device);
            }
        }
    }
    bool InitGame()
    {
        CurrentState = GameState.None;
        SetCurrentState(GameState.SplashScreen);
        return true;
    } 
    public static GameManager GetInstance()
    {
        return INSTANCE__;
    }    

    public void SetCurrentState(GameState state)
    {
        if (CurrentState == state)
        {
            return;
        }

        OnStateLeave(CurrentState);
        OnStateEnter(state);
        CurrentState = state;
    }
    public Player GetPlayerAt(int index)
    {
        return m_players[index];
    }
    public int GetCurrentPlayerCount()
    {
        return m_players.Count;
    }
    public static int GetMaxPlayers()
    {
        return MAX_PLAYERS__;
    }
    public bool isPlayersInputLocked()
    {
        return m_LockPlayersInput;
    }
    public int GetNumOfPlayersAlive()
    {
        int count = 0;
        for (int i = 0; i < GetCurrentPlayerCount(); ++i) {
            if (GetPlayerAt(i).CurrentState != Entity.State.Dead) {
                count++;
            }
        }

        return count;
    }
    public void IncrementPlayerScore()
    {
        for (int i = 0; i < GetCurrentPlayerCount(); ++i)
        {
            Player player = GetPlayerAt(i);
            if (player.CurrentState != Entity.State.Dead) {
               
            }
        }
    }
    public void ResetPlayers()
    {
        for (int i = 0; i < m_players.Count; ++i)
        {
            Player player = m_players[i];
            
        }

        GameObject[] obj = GameObject.FindGameObjectsWithTag("PowerUp");
        for (int i = 0; i < obj.Length; i++) {
            Destroy(obj[i]);
        }
    } 
    

}