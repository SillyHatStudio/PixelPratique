using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Basic Wandering AI
 * */
[RequireComponent(typeof (NavMeshAgent), typeof(CharacterController))]
public class WanderingAI2D : MonoBehaviour {

    public float maxPickDestinationInterval; //Maximum time between 2 destination pick
    public float idleTime;
    public bool useWayPointsList;
    public List<GameObject> wayPointsObjects;

    private Vector3 m_CurrentDestination, m_LastDestination;
    private float m_LastDistanceToDestination;
    private NavMeshAgent m_NavAgent;
    private float m_PositionCheckInterval = 3; //Interval to check characters position
    private float m_MaxStuckTime = 4f; //Max time before considering the character is stuck if not on an destination point
    private bool m_Idle, m_IsWaiting, m_CurrentDestinationReached;

    void Awake()
    {
        m_Idle = false;
        m_LastDestination = new Vector2(transform.position.x, transform.position.y);
    }

	// Use this for initialization
	void Start () {

        if (useWayPointsList)
        {
            int randIndex = Random.Range(0, wayPointsObjects.Count - 1);
            m_CurrentDestination = wayPointsObjects[randIndex].transform.position;
            m_LastDistanceToDestination = (m_CurrentDestination - transform.position).magnitude;
        }

        StartCoroutine("CheckIfCharacterStuck");
	}
	
	// Update is called once per frame
	void Update () {

        //TODO
        if (m_Idle)
        {
            StartCoroutine("PickNextDestinationRandom()");
        }

        else
        {
            //Destination just reached
            if(m_CurrentDestination == m_LastDestination && !m_CurrentDestinationReached)
            {
                m_Idle = true;
                m_CurrentDestinationReached = true;
                m_IsWaiting = false;
                StartCoroutine("Wait", idleTime);

            }
        }
    }

    IEnumerator PickNextDestinationRandom()
    {
        //TODO
        if (useWayPointsList)
        {
            int randIndex = Random.Range(0, wayPointsObjects.Count - 1);
            m_CurrentDestination = wayPointsObjects[randIndex].transform.position;
        }


        yield return null;
    }


    IEnumerator CheckIfCharacterStuck()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_PositionCheckInterval);
            if (!m_Idle)
            {
                float currentDistanceToDestination = (m_CurrentDestination - transform.position).magnitude;
                Debug.Log("--- Checking if character is stuck");
                Debug.Log("Remaining distance to destination = " + currentDistanceToDestination);

                //If distance is the same as a few seconds ago, means we're stuck
                if(currentDistanceToDestination == m_LastDistanceToDestination)
                {
                    Debug.Log("Stuck !");
                    StartCoroutine("PickNextDestinationRandom()");
                }

                else{
                    m_LastDistanceToDestination = currentDistanceToDestination;
                }
            }
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        m_IsWaiting = true;
    }
}
