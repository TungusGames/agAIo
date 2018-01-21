using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

	public void setup (Movement.Stats stats)
	{

	}

	public void update (float radius, float energy, float speed, float angle, GameObject[] enemies, 
		out bool split, out float goalSpeed, out float goalAngle)
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		goalSpeed = Mathf.Sqrt(x * x + y * y);
		goalAngle = Mathf.Atan2(y, x)*Mathf.Rad2Deg;
		split = false;
	}
}
