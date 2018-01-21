using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
public class SpawnHandler : MonoBehaviour {

	public static string[] AIScripts;
	private static SpawnHandler INSTANCE;
	public static SpawnHandler getInstance()
	{
		return INSTANCE;
	}


	[SerializeField]
	public GameObject prefab;

	private int myNumAITypes;
	private int currentAIType = 0;

	private float myTotalRadiusSquared = 0;

	public static GameObject newAI (float x, float y, float E, float r, float f, float v, float a_max, float E_mul, float stat_mul, float split_cost_mul); //returns the new AI

	// Use this for initialization
	void Start () {
		INSTANCE = this;
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		AIScripts = new string[texts.Length];
		for (int i = 0; i < texts.Length; i++) {
			AIScripts [i] = ((TextAsset)(texts [i])).text;
		}
		myNumAITypes = AIScripts.Length;
	}

	// Update is called once per frame
	void Update () {
		while (myTotalRadiusSquared < SimParameters.MAX_TOTAL_RADIUS_SQUARED) {
			float newRadius = SimParameters.MIN_SPAWN_RADIUS + Random.value * (SimParameters.MAX_SPAWN_RADIUS - SimParameters.MIN_SPAWN_RADIUS);
			float newX = Random.value * (SimParameters.MAP_WIDTH) - SimParameters.MAP_WIDTH / 2;
			float newY = Random.value * (SimParameters.MAP_HEIGHT) - SimParameters.MAP_HEIGHT / 2;
			GameObject cell = Instantiate (prefab, new Vector3 (newX, newY, 0), Quaternion.identity);
			addRadius(newRadius);
			cell.GetComponent<Movement> ().setRadius(newRadius);
			currentAIType = (currentAIType + 1) % myNumAITypes; // cycling through AI types
		}
	}

	public void addRadius(float r) {
		myTotalRadiusSquared += r * r;
	}
	public void removeRadius(float r) {
		myTotalRadiusSquared -= r * r;
	}
	public void changeRadius(float old, float newR) {
		removeRadius(old);
		addRadius(newR);
	}

}

