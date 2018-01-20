using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
public class SpawnHandler : MonoBehaviour {

	public static string[] AIScripts;

	// Use this for initialization
	void Start () {
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		AIScripts = new string[texts.Length];
		for (int i = 0; i < texts.Length; i++) {
			AIScripts [i] = ((TextAsset)(texts [i])).text;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
