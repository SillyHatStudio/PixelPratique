using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOverlay : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Members:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    public bool m_PerformFadeOut;

    public float m_TimeScale;
    private bool m_FadeOut;
    private Image m_Image;

    ////////////////////////////////////////////////////////////////////////////////
    // Callbacks:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
	void Start ()
    {
        // set initial alpha value
        m_Image = GetComponentInChildren<Image>();
        m_Image.color = new Color(
            m_Image.color.r,
            m_Image.color.g,
            m_Image.color.b,
            1.0f
        );

        m_FadeOut = false;
	}

    public void SetFadeOut()
    {
        // set initial alpha value
        m_Image = GetComponentInChildren<Image>();
        m_Image.color = new Color(
            m_Image.color.r,
            m_Image.color.g,
            m_Image.color.b,
            0.0f
        );

        m_PerformFadeOut = true;
        m_FadeOut = true;
    }

    /*----------------------------------------------------------------------------*/
    void Update()
    {
        if (m_Image.color.a > 0.05 && m_FadeOut == false)
        {
            m_Image.color = new Color(
                m_Image.color.r,
                m_Image.color.g,
                m_Image.color.b,
                Mathf.Lerp(m_Image.color.a, 0.0f, Time.deltaTime * (1 / m_TimeScale))
            );
        }
        else
        {
            if (m_PerformFadeOut)
            {
                m_Image.color = new Color(
                    m_Image.color.r,
                    m_Image.color.g,
                    m_Image.color.b,
                    Mathf.Lerp(m_Image.color.a, 1.0f, Time.deltaTime * (1 / m_TimeScale))
                );

                m_FadeOut = true;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    // Accessors:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    public bool IsDone()
    {
        return m_Image.color.a > 0.95 && m_FadeOut == true;
    }

    public void Reset()
    {
		m_FadeOut = false;
		m_PerformFadeOut = false;
		Color color = m_Image.color;
		color.a = 0;
		m_Image.color = color;
    }
}