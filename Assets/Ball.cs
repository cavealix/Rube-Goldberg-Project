﻿using System;
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
	private GameObject trampoline;

	public List<GameObject> stars;

	// Use this for initialization
	void Start () {
		respawn = ball.transform.position;
		Debug.Log("Ball start position is " + respawn.ToString());
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
        	//respawn to original position and deactivate gravity
            ball.transform.position = respawn;
            ball.GetComponent<Rigidbody>().useGravity = true;
            //set velocity and angular velocity (spin) to 0
            ball.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
            ball.GetComponent<Rigidbody>().angularVelocity = new Vector3 (0, 0, 0);
            Debug.Log("The ball hit the floor and respawned");

            //Respawn Stars
            foreach(GameObject star in stars)
            {
            	star.GetComponent<Star>().Respawn();
            }
            //reset star array
            stars.Clear();
        }
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
    	//Collect Star
    	if (col.gameObject.tag == "Star")
    	{
    		stars.Add(col.gameObject);
    		col.gameObject.GetComponent<Star>().Collect();
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
    }

    void OnTriggerExit(Collider col)
    {
    	if(col.gameObject.tag == "WindArea")
    	{
    		inWind = false;
    	}
    }
}