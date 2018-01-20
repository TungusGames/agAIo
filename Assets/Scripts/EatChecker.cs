using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatChecker : MonoBehaviour {

	CircleCollider2D myCircle;
	Movement myMovement;

	// Use this for initialization
	void Start () {
		myCircle = gameObject.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log("Triggered!");
		if (myCircle.OverlapPoint(other.transform.position)) {
			Debug.Log("Eaten!");
		}
	}
		
}
