	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaController : Controller {

	private Script ai;
	private int typeID;

	public LuaController (int typeID)
	{
		this.typeID = typeID;
		Debug.Log ("" + SpawnHandler.AIScripts.Length);
		string script = SpawnHandler.AIScripts[typeID];
		ai = new Script();
		Debug.Log ("1.");
		ai.DoString(script);
		Debug.Log ("2.");
	}

	public void setup (Movement.Stats stats)
	{
		float[] data = stats.asArray();
		ai.Call(ai.Globals["construct"], data);
	}

	public void update (float radius, float energy, float speed, float angle, GameObject[] enemies, 
		out bool split, out float goalSpeed, out float goalAngle)
	{
		float[] self = new float[]{ radius, energy, speed, angle };
		float[][] others = new float[0][];
		DynValue res = ai.Call(ai.Globals["update"], self, others);
		if (res.IsNil()) {
			split = true;
			goalSpeed = goalAngle = 0;
		} else {
			split = false;
			var res_ = res.Table;
			goalSpeed = (float)(res_[0]);
			goalAngle = (float)(res_[1]);
		}

	}
}
