using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatChecker : MonoBehaviour {

	CircleCollider2D myCircle;
	Movement myMovement;

	// Use this for initialization
	void Start () {
		myCircle = gameObject.GetComponent<CircleCollider2D>();
		myMovement = gameObject.GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (myCircle.OverlapPoint(other.transform.position)) {
			float r1 = myMovement.radius;
			float r2 = other.gameObject.GetComponent<Movement>().radius;
			myMovement.setRadius(Mathf.Sqrt(r1 * r1 + r2 * r2));
			SpawnHandler.getInstance().removeRadius(r2);
			Destroy(other.gameObject);
		}
	}
		
}
