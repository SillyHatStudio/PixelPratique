using UnityEngine;
using System.Collections;

public class PoolManager : MonoBehaviour 
{
	public static PoolManager instance = null;

	public class PoolInfo 
	{
		public string name;
		public GameObject[] pool;
		GameObject parent;
		public PoolInfo(string _name, GameObject _prefab, int _number, GameObject _parent)
		{
			name = _name;
			parent = _parent;
			CreatePool(_number, _prefab);
		}

		void CreatePool(int _number, GameObject _prefab)
		{
			pool = new GameObject[_number];
			for(int i = 0; i < _number; i++)
			{
				pool[i] = Instantiate(_prefab);
				pool[i].SetActive(false);			
				pool[i].transform.parent = parent.transform;
				pool[i].name = name;
			}
		}
	}
	PoolInfo[] m_PoolInfoList;

	[System.Serializable]
	public struct PoolPrefab
	{
		public GameObject prefab;
		public int number;
	}
	public PoolPrefab[] m_PoolPrefabList;

	void Awake()
    {
        Debug.Log("Awake GameManager");
		if (instance == null)
        {
			instance = this;
			DontDestroyOnLoad(instance.gameObject);
			name = "PoolManager";
            if (!InitGame())
            {
                Debug.LogError("Unable to create PoolManager!");
            }
        }
        else
        {
			Debug.LogError("PoolManager already exists!");
        } 
    }

	bool InitGame()
	{
		return true;
	}

	void Start()
	{
		m_PoolInfoList = new PoolInfo[m_PoolPrefabList.Length];
		for(int i = 0; i < m_PoolPrefabList.Length; i++) //Each list
		{
			m_PoolInfoList[i] = new PoolInfo(m_PoolPrefabList[i].prefab.name,
											 m_PoolPrefabList[i].prefab, 
											 m_PoolPrefabList[i].number,
											 gameObject);
		}
	}


	public GameObject GetPoolObject(string _name)
	{
		GameObject poolObject = null;
		for(int i = 0; i< m_PoolInfoList.Length; i++)
		{
			//if pool exist, search for the object
			if(m_PoolInfoList[i].name == _name) 
			{
				
				for(int j = 0; j< m_PoolInfoList[i].pool.Length; j++)
				{
					if(!m_PoolInfoList[i].pool[j].activeSelf)
					{
						poolObject = m_PoolInfoList[i].pool[j];
						poolObject.SetActive(true);
						return poolObject;
					}
				}
			}
		}

		if(poolObject == null)
		{
			Debug.LogError("No more " + _name + " object available. You might need more !!! ");
		}
		return poolObject;
	}






}
