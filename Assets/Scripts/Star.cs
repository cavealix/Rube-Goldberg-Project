using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	public float turnSpeed = 100f;

	// Use this for initialization
	void Start () {
		//GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		//spin
		gameObject.transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
	}

	public void Collect()
	{
		GetComponent<AudioSource>().Play();
		
		gameObject.SetActive(false);
	}

	public void Respawn()
	{
		//Instantiate(gameObject);
		gameObject.SetActive(true);	
	}
}
