using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {
	private Vector3 respawn; //location to respawn 
	public GameObject ball; //item to get location of for respawn
	private Vector3 reflect;

	//Wind and Trampoline variables
	public float windForce = 1f;
	public bool inWind = false;
	private GameObject WindArea;
	private GameObject trampoline;

	//Start Zone 
	public GameObject StartZone;
	public bool inStartZone = false;

	//GameLogic
	public GameObject GameLogic;

    //Grabbed indicator to help detect cheating
    public bool Grabbed = false;

	//Ball Materials
	public Material m_Active;
	public Material m_Inactive;

	//Collected Stars
	//public List<GameObject> stars;

	// Use this for initialization
	void Start () {
		respawn = ball.transform.position;
		//Debug.Log("Ball start position is " + respawn.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		inStartZone = GameLogic.GetComponent<GameLogic>().checkStartZone();
		if (inStartZone)
			ball.GetComponent<Renderer>().material = m_Active;
		else
			gameObject.GetComponent<Renderer>().material = m_Inactive;
	}

	//Check if ball in start zone to prevent cheating
	/*bool checkStarZone()
	{
		return StartZone.GetComponent<Collider>().bounds.Contains(gameObject.transform.position);
	}*/

	//Hit Floor, Respawn ball and stars
	void OnCollisionEnter (Collision col)
    {
		//if collision with floor, respawn at original location
        if(col.gameObject.CompareTag("Ground"))
        {
        	Respawn();
        }
    }

    public void Respawn()
    {
        //respawn to original position and deactivate gravity
        ball.transform.position = respawn;
        ball.GetComponent<Rigidbody>().useGravity = false;
        //set velocity and angular velocity (spin) to 0
        ball.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
        ball.GetComponent<Rigidbody>().angularVelocity = new Vector3 (0, 0, 0);
        //Debug.Log("The ball hit the floor and respawned");
    }

    //Apply Wind Force
    private void FixedUpdate()
    {
    	if(inWind)
    	{
	        ball.GetComponent<Rigidbody>().AddForce(WindArea.transform.up * WindArea.GetComponent<WindArea>().strength);
    	}
    }

    //Trigger effects of hitting various objects
    void OnTriggerEnter(Collider col)
    {
    	//Hit Goal
    	if (col.gameObject.tag == "Goal")
    	{
    		GameLogic.GetComponent<GameLogic>().HitGoal();
    	}

    	//Collect Star
    	if (col.gameObject.tag == "Star")
    	{
            Debug.Log("Hit Star");
    		GameLogic.GetComponent<GameLogic>().CollectStar(col.gameObject);
    	}

    	//Enter Wind Area
    	if(col.gameObject.tag == "WindArea")
    	{
    		inWind = true;
    		WindArea = col.gameObject;
    	}

    	//Bounce
        if (col.gameObject.CompareTag("Trampoline"))
        {
        	Debug.Log ("Bounce!");
        	trampoline = col.gameObject;
            //col.gameObject.GetComponent<Rigidbody>().AddForce (0, forceApplied, 0);
        	//reflect = Vector3.Reflect(ball.GetComponent<Rigidbody>().velocity, col.contacts[0].normal);
        	reflect = Vector3.Reflect(ball.GetComponent<Rigidbody>().velocity, Vector3.up);
        	ball.GetComponent<Rigidbody>().velocity =  trampoline.GetComponent<Trampoline>().strength * reflect;
        }

        //Prevent carrying outside of startZone
        if (col.gameObject.CompareTag("StartZone"))
        {
            Debug.Log("Hit Start Zone");
            if (Grabbed == true)
            {
                Respawn();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
    	if(col.gameObject.tag == "WindArea")
    	{
    		inWind = false;
    	}
    }
}
