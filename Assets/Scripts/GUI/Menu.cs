using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Members:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    public FadeOverlay m_FadeOverlay;
    public GameObject m_Button0;
    public GameObject m_Button1;
    public GameObject m_Arrow;  

    public bool m_StartMenuOpen = true;

    const int BUTTON_COUNT__ = 2;
    int m_ButtonIndex;
    int m_SelectIndex;

    bool m_PlayerReady;


   
	void Start ()
    {
        m_Arrow.SetActive(false);

        m_ButtonIndex = 0;
        m_SelectIndex = -1;
        m_PlayerReady = false;
	}
   
    void Update()
    {
        if (GameManager.GetInstance().GetCurrentPlayerCount() > 0)
        {
            if (m_PlayerReady == false)
            {
                m_PlayerReady = true;
                return;
            }
        }
        else
        {
            return;
        }

		
     

        if (m_PlayerReady && m_FadeOverlay.m_PerformFadeOut == false)
        {
            Player active = GameManager.GetInstance().GetPlayerAt(0);
            if (active.Device.DPadUp.WasPressed)
            {                
                if (m_ButtonIndex != 0) {
                    m_ButtonIndex--;
                }
                else {
                    m_ButtonIndex = BUTTON_COUNT__ - 1;
                }
            }
            if (active.Device.DPadDown.WasPressed)
            {                
                if (m_ButtonIndex < BUTTON_COUNT__-1) {
                    m_ButtonIndex++;
                }
                else {
                    m_ButtonIndex = 0;
                }
            }

            switch (m_ButtonIndex)
            {
                case 0:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_Button0.transform.position.y
                    );
                    break;

                case 1:
                    m_Arrow.transform.position = new Vector3(
                        m_Arrow.transform.position.x,
                        m_Button1.transform.position.y
                    );
                    break;

                default:
                    break;
            }

            if (active.Device.Action1.WasPressed)
            {                
                m_FadeOverlay.m_PerformFadeOut = true;				
                m_SelectIndex = m_ButtonIndex;
            }

            m_Arrow.SetActive(true);
        }
    }
   
    public int GetSelectIndex()
    {
        return m_SelectIndex;
    }

    public bool IsDone()
    {
        return m_FadeOverlay.IsDone();
    }


}