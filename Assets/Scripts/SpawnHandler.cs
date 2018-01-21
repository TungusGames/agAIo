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

	public HashSet<GameObject> aliveCells;

	// Use this for initialization
	void Start () {
		aliveCells = new HashSet<GameObject>();
		INSTANCE = this;
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		AIScripts = new string[texts.Length];
		InvokeRepeating("recalcTotal", 1f, 1f);
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
			addAI (currentAIType, newX, newY, newRadius);
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

	public void recalcTotal() {
		float sum = 0.0f;
		foreach (GameObject obj in aliveCells) {
			sum += obj.GetComponent<Movement>().radius*obj.GetComponent<Movement>().radius;
		}
		myTotalRadiusSquared = sum;
	}
	
	public GameObject addAI (int type, float x, float y, float r, float E = 0, float angle = 0, float speed = 0, Movement.Stats stats = null) { 	
		GameObject cell = Instantiate (prefab, new Vector3 (x, y, 0), Quaternion.identity);
		aliveCells.Add(cell);
		addRadius(r);
		Movement mvt = cell.GetComponent<Movement> ();
		if (stats == null) {
			stats = new Movement.Stats ();
		}
		cell.GetComponent<SpriteRenderer> ().color = (SimParameters.COLORS) [type % (SimParameters.COLORS.Length)];
		mvt.init(currentAIType, r,  E, angle, speed, stats);
		return cell;
	}
}

