using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	private Vector3 respawn; //location to respawn 
	public GameObject ball; //item to get location of for respawn
	private Vector3 reflect;

	public float windForce = 1f;
	public bool inWind = false;
	private GameObject WindArea;

	// Use this for initialization
	void Start () {
		respawn = ball.transform.position;
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
        	//respawn to original position and deactivate gravity
            ball.transform.position = respawn;
            ball.GetComponent<Rigidbody>().useGravity = true;
            //set velocity and angular velocity (spin) to 0
            ball.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
            ball.GetComponent<Rigidbody>().angularVelocity = new Vector3 (0, 0, 0);
            Debug.Log("The ball hit the floor and respawned");
        }

        //Bounce
        if (col.gameObject.CompareTag("Trampoline"))
        {
        	Debug.Log ("Bounce!");
           //col.gameObject.GetComponent<Rigidbody>().AddForce (0, forceApplied, 0);
        	reflect = Vector3.Reflect(ball.GetComponent<Rigidbody>().velocity, col.contacts[0].normal);
        	ball.GetComponent<Rigidbody>().velocity = 2 * reflect;
        }
     
    }

    //wind zone
    /*void OnCollisionStay(Collision col)
    {
        Debug.Log ("Wind!");
        //col.gameObject.GetComponent<Rigidbody>().AddForce (0, forceApplied, 0);
        ball.GetComponent<Rigidbody>().AddForce(col.gameObject.transform.forward * windForce);
    } */

    private void FixedUpdate()
    {
    	if(inWind)
    	{
	        ball.GetComponent<Rigidbody>().AddForce(WindArea.transform.up * windForce);
    	}
    }

    void OnTriggerEnter(Collider col)
    {
    	if(col.gameObject.tag == "ForceField")
    	{
    		inWind = true;
    		WindArea = col.gameObject;
    	}
    }

    void OnTriggerExit(Collider col)
    {
    	if(col.gameObject.tag == "ForceField")
    	{
    		inWind = false;
    	}
    }
}
