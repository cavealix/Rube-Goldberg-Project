using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
	private Vector3 respawn; //location to respawn 
	public GameObject item; //item to get location of for respawn

	// Use this for initialization
	void Start () {
		respawn = item.transform.position;
	}

	void OnCollisionEnter (Collision col)
    {
		//if collision with floor, respawn at original location
        if(col.gameObject.name == "Ground")
        {
            item.transform.position = respawn;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
