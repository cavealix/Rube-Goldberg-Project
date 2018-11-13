using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {
	private Vector3 respawn; //location to respawn 
	public GameObject item; //item to get location of for respawn

	// Use this for initialization
	void Start () {
		respawn = item.transform.position;
		Debug.Log("Ball start position is " + respawn.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter (Collision col)
    {
		//if collision with floor, respawn at original location
        if(col.gameObject.CompareTag("Ground"))
        {
        	//respawn to original position
            item.transform.position = respawn;
            //set velocity and angular velocity (spin) to 0
            item.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
            item.GetComponent<Rigidbody>().angularVelocity = new Vector3 (0, 0, 0);
            Debug.Log("The ball hit the floor and respawned");
        }
    }
}
