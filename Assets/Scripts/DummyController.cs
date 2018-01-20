using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : Controller {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setup (Movement.Stats stats)
	{

	}

	public void update (float radius, float energy, float speed, float angle, GameObject[] enemies, 
		out bool split, out float goalSpeed, out float goalAngle)
	{
		split = false;
		goalSpeed = 1;
		goalAngle = Random.value*360;
	}
}
