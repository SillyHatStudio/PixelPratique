using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Crystal : MonoBehaviour {

    public int value = 100;
    private SpriteRenderer m_SpriteRenderer;
    private PolygonCollider2D m_Collider;

    public void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Collider = GetComponent<PolygonCollider2D>();
    }

    public void EnableCrystal(bool enable)
    {
        m_SpriteRenderer.enabled = enable;
        m_Collider.enabled = enable;
    }

}
