using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
public class SpawnHandler : MonoBehaviour {

	string[] AIScripts;

	// Use this for initialization
	void Start () {
		Object[] texts = Resources.LoadAll("AIs/", typeof(TextAsset));
		AIScripts = new string[texts.Length];
		for (int i = 0; i < texts.Length; i++) {
			AIScripts [i] = ((TextAsset)(texts [i])).text;
			Debug.Log (AIScripts [i]);
		}

		string script = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(5)";

		DynValue res = Script.RunString(script);
		Debug.Log(""+res.Number);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
