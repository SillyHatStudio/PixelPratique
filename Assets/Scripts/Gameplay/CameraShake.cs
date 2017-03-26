using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    // Members:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform m_CamTransform;

    // How long the object should shake for.
    public float m_ShakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float m_ShakeAmount = 0.2f;
    public float m_DecreaseFactor = 1.0f;

    Vector3 m_OriginalPos;

    ////////////////////////////////////////////////////////////////////////////////
    // Callbacks:
    ////////////////////////////////////////////////////////////////////////////////
    /*----------------------------------------------------------------------------*/
    void Awake()
    {
        if (m_CamTransform == null)
        {
            m_CamTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    /*----------------------------------------------------------------------------*/
    void OnEnable()
    {
        m_OriginalPos = m_CamTransform.localPosition;
    }

    /*----------------------------------------------------------------------------*/
    void Update()
    {
        if (m_ShakeDuration > 0)
        {
            Vector3 rand = m_OriginalPos + Random.insideUnitSphere * m_ShakeAmount;
            m_CamTransform.localPosition = new Vector3(rand.x, rand.y, m_OriginalPos.z);
            m_ShakeDuration -= Time.deltaTime * m_DecreaseFactor;
        }
        else
        {
            m_ShakeDuration = 0f;
            m_CamTransform.localPosition = m_OriginalPos;
        }
    }
}