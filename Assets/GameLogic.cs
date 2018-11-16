using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	public GameObject ball;
	public List<GameObject> stars;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NextLevel()
	{
		//SteamVR_LoadLevel.Begin("Level_1");
	}

	public void CollectStar()
	{
/*		stars.Add(star);
    	star.GetComponent<Star>().Collect();*/
	}

	public void Reset()
	{

	}

	public void HitGoal()
	{

	}

	public bool isCheating()
	{
		return true;
	}
}
