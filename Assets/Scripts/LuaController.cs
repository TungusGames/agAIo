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
		string script = SpawnHandler.AIScripts[typeID];
		ai = new Script();
		ai.DoString(script);
	}

	void setup (Movement.Stats stats)
	{
		float[] data = stats.asArray();
		ai.Call(ai.Globals["construct"], data);
	}

	void update (float radius, float energy, float speed, float angle, GameObject[] enemies, 
		out bool split, out float goalSpeed, out float goalAngle)
	{
		float[] self = new float[]{ radius, energy, speed, angle };
		float[][] others = new float[0][];
		float[] res = ai.Call(ai.Globals["update"], self, others);
		if (res == null) {
			split = true;
			goalSpeed = goalAngle = 0;
		} else {
			split = false;
			goalSpeed = res[0];
			goalAngle = res[1];
		}

	}
}
