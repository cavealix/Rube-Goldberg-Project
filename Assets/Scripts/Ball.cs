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


	// Use this for initialization
	void Start () {
        //set respawn point
		respawn = ball.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

	//Hit Floor, Respawn ball and stars
	void OnCollisionEnter (Collision col)
    {
		//if collision with floor, respawn at original location
        if(col.gameObject.CompareTag("Ground"))
        {
            //Respawn Ball
        	Respawn();
            //Reset Game Logic
            GameLogic.GetComponent<GameLogic>().Reset();
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
    }

    //Wind
    private void FixedUpdate()
    {
        //Apply wind force
    	if(inWind)
    	{
	        ball.GetComponent<Rigidbody>().AddForce(WindArea.transform.up * WindArea.GetComponent<WindArea>().strength);
    	}
    }

    //Trigger effects of hitting various objects
    void OnTriggerEnter(Collider col)
    {
        //Set ball material
        if (col.gameObject.tag == "StartZone")
        {
            ball.GetComponent<Renderer>().material = m_Active;
        }

    	//Hit Goal (if not cheating)
    	if (col.gameObject.tag == "Goal" && Grabbed == false)
    	{
    		GameLogic.GetComponent<GameLogic>().HitGoal();
    	}

    	//Collect Star if not Grabbed (Prevent Cheating)
    	if (col.gameObject.tag == "Star" && Grabbed == false)
    	{
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
        	trampoline = col.gameObject;
        	reflect = Vector3.Reflect(ball.GetComponent<Rigidbody>().velocity, Vector3.up);
        	ball.GetComponent<Rigidbody>().velocity =  trampoline.GetComponent<Trampoline>().strength * reflect;
        }
    }

    //Wind and StartZone
    void OnTriggerExit(Collider col)
    {
        //Set wind bool
    	if(col.gameObject.tag == "WindArea")
    	{
    		inWind = false;
    	}

        //Set ball material
        if (col.gameObject.tag == "StartZone")
        {
            ball.GetComponent<Renderer>().material = m_Inactive;
        }
    }
}
