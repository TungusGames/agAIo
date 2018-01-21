using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using MoonSharp.Interpreter;

public class SpawnHandler : MonoBehaviour {

	public static string[] AIScripts;
	public static string[] AINames;
	private static SpawnHandler INSTANCE;
	public static SpawnHandler getInstance()
	{
		return INSTANCE;
	}

	public static readonly int NUM_LABELS = 5;

	private GameObject[] textLabels= new GameObject[NUM_LABELS];

	[SerializeField]
	public GameObject prefab;

	private int myNumAITypes;
	private int currentAIType = 0;

	private float myTotalRadiusSquared = 0;

	public HashSet<GameObject> aliveCells;

	public float[] massOfTypes;

	// Use this for initialization
	void Start () {
		aliveCells = new HashSet<GameObject>();
		INSTANCE = this;
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		myNumAITypes = texts.Length;
		AIScripts = new string[myNumAITypes];
		AINames = new string[myNumAITypes];
		massOfTypes = new float[myNumAITypes];
		InvokeRepeating("recalcTotal", 1f, 1f);
		for (int i = 0; i < myNumAITypes; i++) {
			AIScripts [i] = ((TextAsset)(texts [i])).text;
			AINames [i] = ((TextAsset)(texts [i])).name;
		}
		myNumAITypes = AIScripts.Length;
		textLabels[0] = GameObject.Find ("Text");
		textLabels[1] = GameObject.Find ("Text (1)");
		textLabels[2] = GameObject.Find ("Text (2)");
		textLabels[3] = GameObject.Find ("Text (3)");
		textLabels[4] = GameObject.Find ("Text (4)");
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
		for (int i = 0; i < massOfTypes.Length; i++) {
			massOfTypes[i] = 0;
		}
		foreach (GameObject obj in aliveCells) {
			float w = obj.GetComponent<Movement>().radius*obj.GetComponent<Movement>().radius;

			int t = obj.GetComponent<Movement>().getTypeID();
			if (t != -1)
				massOfTypes[t] += w;
			sum += w;
		} 	
		for (int i = 0; i < NUM_LABELS && i < myNumAITypes; ++i) {
			Debug.Log (massOfTypes [i] + " " + AINames [i]);
			textLabels [i].GetComponent<Text> ().text = massOfTypes [i] + AINames [i];
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
		if (type < NUM_LABELS)
						cell.GetComponent<SpriteRenderer> ().color = (SimParameters.COLORS) [type % (SimParameters.COLORS.Length)];
		mvt.init(type, r,  E, angle, speed, stats);
		return cell;
	}
}

