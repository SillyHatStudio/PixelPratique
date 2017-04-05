using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using InControl;
using System.Linq;
using Pixel.Game.Management;

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
    private GameObject m_CurrentPacman;

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
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    m_PlayerList = new GameObject[4];

                    int rand = Random.Range(0, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        GameObject player = PoolManager.instance.GetPoolObject("Player");
                        m_PlayerList[i] = player;                       

                        if(rand == i)
                        {
                            player.name = "pacman " + player.GetHashCode();
                            m_CurrentPacman = player;
                            player.GetComponent<PacmanPlayer>().enabled = true;
                            player.GetComponent<PacmanPlayer>().setPlayerType(EnumTypes.PlayerType.Pacman);
                            player.GetComponent<PacmanPlayer>().SetPlayerNumber(i);
                            m_PlayerList[i].GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = false;
                            player.GetComponent<SpriteRenderer>().color = Color.yellow;
                        }

                        else
                        {
                            player.name = "ghost "+player.GetHashCode();
                            player.GetComponent<GhostPlayer>().enabled = true;
                            player.GetComponent<GhostPlayer>().setPlayerType(EnumTypes.PlayerType.Ghost);
                            player.GetComponent<GhostPlayer>().SetPlayerNumber(i);
                            player.GetComponent<SpriteRenderer>().color = player.GetComponent<GhostPlayer>().playerColor;
                            m_PlayerList[i].GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = true;
                        }
                        SetPlayerPosition(player, i);
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

    public void ResetGame()
    {

        for (int i = 0; i < m_PlayerList.Length; i++)
        {
            SetPlayerPosition(m_PlayerList[i], i);
           // m_PlayerList[i].GetComponent<GhostPlayer>().enabled = false;
            //m_PlayerList[i].GetComponent<PacmanPlayer>().enabled = false;

        }
        ChoosePacman();
    }

    private void ChoosePacman()
    {
        //Lock the current pac man timer
        m_CurrentPacman.GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = true;


        var listPlayers = new List<GameObject>(m_PlayerList); //Tmp list

        var candidates = listPlayers.Where(p => p.GetComponent<GhostPlayer>().enabled && !p.GetComponent<PacmanPlayer>().enabled)
            .OrderByDescending(p => p.GetComponent<GhostPlayer>().PlayerTimer.remainingMins)
            .ThenByDescending(p => p.GetComponent<GhostPlayer>().PlayerTimer.remainingSecs)
            .ThenByDescending(p => p.GetComponent<GhostPlayer>().PlayerTimer.milliseconds).ToList();

        //Group the players by time, and check if there are more than 1 player with a time
        var playersWithSameTime = candidates.GroupBy(p => 
                                        new { p.GetComponent<GhostPlayer>().PlayerTimer.remainingMins,
                                            p.GetComponent<GhostPlayer>().PlayerTimer.remainingSecs,
                                            p.GetComponent<GhostPlayer>().PlayerTimer.milliseconds } )
            .Where(grp => grp.Count() > 1);

        var nextPacman = candidates.First();

        if (playersWithSameTime.Count() > 0)
        {
            Debug.Log("there are  players with same time");

            var finalCandidates = playersWithSameTime.SelectMany(g => g);
            int randIndex = Random.Range(0, finalCandidates.Count());
            nextPacman = finalCandidates.ElementAt(randIndex);
        }

        //Switch former pacman's scripts
        m_CurrentPacman.GetComponent<PacmanPlayer>().enabled = false;
        m_CurrentPacman.GetComponent<GhostPlayer>().enabled = true;

        m_CurrentPacman = nextPacman;


        //Set the new pacman
        nextPacman.name = "pacman "+nextPacman.GetHashCode();
        nextPacman.GetComponent<PacmanPlayer>().enabled = true;
        nextPacman.GetComponent<GhostPlayer>().enabled = false;
        nextPacman.GetComponent<PacmanPlayer>().setPlayerType(EnumTypes.PlayerType.Pacman);
        nextPacman.GetComponent<SpriteRenderer>().color = Color.yellow;
        nextPacman.GetComponent<PacmanPlayer>().SetPlayerNumber(listPlayers.IndexOf(nextPacman));
        nextPacman.GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = false;

        //Set the ghosts
        var ghostPlayers = listPlayers.Where(p => !p.GetComponent<PacmanPlayer>().enabled && p.GetComponent<GhostPlayer>().enabled);

        foreach(var ghost in ghostPlayers)
        {
            ghost.name = "ghost "+ghost.GetHashCode();
            ghost.GetComponent<GhostPlayer>().enabled = true;
            ghost.GetComponent<PacmanPlayer>().enabled = false;
            ghost.GetComponent<GhostPlayer>().setPlayerType(EnumTypes.PlayerType.Ghost);
            ghost.GetComponent<GhostPlayer>().SetPlayerNumber(listPlayers.IndexOf(ghost));
            ghost.GetComponent<SpriteRenderer>().color = ghost.GetComponent<GhostPlayer>().playerColor;
            ghost.GetComponent<GhostPlayer>().PlayerTimer.timerLocked = true;
        }

        Debug.Log("NextPacman is player " + nextPacman.GetHashCode());

        /* int rand = Random.Range(0, 4);      

         for (int i = 0; i < m_PlayerList.Length; i++)
         {
             if (rand == i)
             {
                 m_PlayerList[i].GetComponent<PacmanPlayer>().enabled = true;
                 m_PlayerList[i].GetComponent<PacmanPlayer>().setPlayerType(EnumTypes.PlayerType.Pacman);
                 m_PlayerList[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                 m_PlayerList[i].GetComponent<PacmanPlayer>().SetPlayerNumber(i);
                 m_PlayerList[i].GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = false;

             }

             else
             {
                 m_PlayerList[i].GetComponent<GhostPlayer>().enabled = true;
                 m_PlayerList[i].GetComponent<GhostPlayer>().setPlayerType(EnumTypes.PlayerType.Ghost);
                 m_PlayerList[i].GetComponent<SpriteRenderer>().color = Color.white;
                 m_PlayerList[i].GetComponent<GhostPlayer>().SetPlayerNumber(i);
                 m_PlayerList[i].GetComponent<PacmanPlayer>().PlayerTimer.timerLocked = true;
             }
         }*/
    }

    public void SetPlayerPosition(GameObject player, int pNumber)
    {

        switch (pNumber)
        {
            case 0:
                player.transform.position = new Vector2(-2.76f, 3.18f);
                break;

            case 1:
                player.transform.position = new Vector2(2.76f, 3.18f);
                break;

            case 2:
                player.transform.position = new Vector2(-2.76f, -3.5f);
                break;
            case 3:
                player.transform.position = new Vector2(2.76f, -3.5f);
                break;

            default:
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
            fx.Emit(spos.x, 1 - spos.y);
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
        for (int i = 0; i < GetCurrentPlayerCount(); ++i)
        {
            if (GetPlayerAt(i).CurrentState != Entity.State.Dead)
            {
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
            if (player.CurrentState != Entity.State.Dead)
            {

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
        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
    }


}