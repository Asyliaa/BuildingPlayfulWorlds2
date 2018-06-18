using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject prefab;
	// Use this for initialization
	void Start () {
        Instantiate(prefab, new Vector3(5,1,5), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
