using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
public class SpawnHandler : MonoBehaviour {

	public static string[] AIScripts;

	[SerializeField]
	public GameObject prefab;

	private int myNumAITypes;
	private int currentAIType = 0;

	private float myTotalRadiusSquared = 0;

	// Use this for initialization
	void Start () {
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		myNumAITypes = texts.Length;
		AIScripts = new string[myNumAITypes];
		for (int i = 0; i < myNumAITypes; i++) {
			AIScripts [i] = ((TextAsset)(texts [i])).text;
		}
	}

	// Update is called once per frame
	void Update () {
		while (myTotalRadiusSquared < SimParameters.MAX_TOTAL_RADIUS_SQUARED) {
			float newRadius = SimParameters.MIN_SPAWN_RADIUS + Random.value * (SimParameters.MAX_SPAWN_RADIUS - SimParameters.MIN_SPAWN_RADIUS);
			float newX = Random.value * (SimParameters.MAP_WIDTH) - SimParameters.MAP_WIDTH;
			float newY = Random.value * (SimParameters.MAP_HEIGHT) - SimParameters.MAP_HEIGHT;
			GameObject cell = Instantiate (prefab, new Vector3 (newX, newY, 0), Quaternion.identity);
			cell.transform.localScale = (new Vector3(newRadius, newRadius, newRadius)) * SimParameters.SPRITE_SCALAR;
			cell.AddComponent(typeof(Movement));
			addRadius(newRadius);
			currentAIType = (currentAIType + 1) % myNumAITypes; // cycling through AI types
		}
	}

	public void addRadius(float r) {
		myTotalRadiusSquared += r * r;
	}
	public void removeRadius(float r) {
		myTotalRadiusSquared -= r * r;
	}

}

