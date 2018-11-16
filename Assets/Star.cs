using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Collect()
	{
		//Destroy(gameObject);
		gameObject.SetActive(false);
	}

	public void Respawn()
	{
		//Instantiate(gameObject);
		gameObject.SetActive(true);	
	}
}
