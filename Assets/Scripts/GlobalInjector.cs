using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalInjector : MonoBehaviour
{
    public List<GameObject> GlobalList;

	void Awake()
    {
        foreach (GameObject prefab in GlobalList)
        {
            Debug.Log("Injecting " + prefab.name);
            Instantiate(prefab);
        }

        Destroy(this.gameObject);
	}
}
