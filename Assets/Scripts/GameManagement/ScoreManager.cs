using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Score Management (to be completed)
 * */
public class ScoreManager : MonoBehaviour {

    public int scoreToWin;
    private int m_CurrentScore;

    void Awake()
    {
        m_CurrentScore = 0;
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    //Method fired when gaining/losing points/life/whatever (to rename ofc)
    void SomeEventToWinPoints(bool condition, int scoreAmount)
    {
        if (condition)
        {
            m_CurrentScore += scoreAmount;

            if (IsGameWon())
            {
                Victory();
            }
        }

        else
        {
            m_CurrentScore -= scoreAmount;
        }
    }

    public bool IsGameWon()
    {
        return m_CurrentScore == scoreToWin;
    }

    public void Victory()
    {

    }

    public void GameOver()
    {

    }
}
