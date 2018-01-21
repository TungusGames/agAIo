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

			/*AIScripts [i] = @"-- AI template

function construct(stats)
	v_max, r_max, E_mul, stat_mul,split_cost_mul = stats[1], stats[2], stats[3], stats[4], stats[5]
	
	return nil
end

function update(self, other)
	split=false
	r, E, v, f = self[1], self[2], self[3], self[4]
	
	-- other[i][] contains data about the i-th nerby AI, other[i][1] is its radius, other[i][2] is 1 if its the same species and 1 if its not, other[i][3] is its relative distance and other[i][4] is its direction from you
	
	
	-- set split to true if want to split, otherwise set v_t to target speed, f_t to target facing; always return split
	
	v_t,f_t=0,0
	
	return split
end";*/}
		myNumAITypes = AIScripts.Length;
	}



	// Update is called once per frame
	void Update () {
		while (myTotalRadiusSquared < SimParameters.MAX_TOTAL_RADIUS_SQUARED) {
			float newRadius = SimParameters.MIN_SPAWN_RADIUS + Random.value * (SimParameters.MAX_SPAWN_RADIUS - SimParameters.MIN_SPAWN_RADIUS);
			float newX = Random.value * (SimParameters.MAP_WIDTH) - SimParameters.MAP_WIDTH / 2;
			float newY = Random.value * (SimParameters.MAP_HEIGHT) - SimParameters.MAP_HEIGHT / 2;
			GameObject cell = Instantiate (prefab, new Vector3 (newX, newY, 0), Quaternion.identity);
			aliveCells.Add(cell);
			addRadius(newRadius);
			Movement mvt = cell.GetComponent<Movement> ();
			mvt.initAIWithTypeID(currentAIType);
			mvt.setRadiusWithoutReporting(newRadius);
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
		
}

