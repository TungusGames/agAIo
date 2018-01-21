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
		split = res.Boolean;
		if (split) {
			goalSpeed=0;goalAngle=0;
		} else {
			goalSpeed = (float)ai.Globals.Get("v_t").Number;
			goalAngle = (float)ai.Globals.Get("f_t").Number;
		}
	}

	public void getEvolve(out float[] weights) {
		DynValue res = ai.Call(ai.Globals["getEvolve"]);
		weights = new float[5];
		weights[0] = Mathf.Abs ((float)ai.Globals.Get ("d_a_max").Number);
		weights[1] = Mathf.Abs ((float)ai.Globals.Get ("d_v_max").Number);
		weights[2] = Mathf.Abs ((float)ai.Globals.Get("d_E_mul").Number);
		weights[3] = Mathf.Abs ((float)ai.Globals.Get("d_stat_mul").Number);
		weights[4] = Mathf.Abs ((float)ai.Globals.Get("d_split_cost_mul").Number);
	}
}