	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaController : Controller {

	private Script ai;
	private int typeID;
	private GameObject gameObject;

	public LuaController (int typeID, GameObject obj)
	{
		gameObject = obj;
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
		float[][] others = new float[enemies.Length][];
		for (int i = 0; i < enemies.Length; i++) {
			others[i] = new float[4];
			var mvm = enemies[i].GetComponent<Movement>();
			others[i][0] = mvm.radius;
			others[i][1] = (mvm.getTypeID == typeID ? 1.0f : 0f);
			Vector3 d = gameObject.transform.position - mvm.transform.position;
			others[i][2] = Mathf.Sqrt(d.x * d.x, d.y * d.y);
			others[i][3] = Mathf.Atan2(d.y, d.x);
		}
		DynValue res = ai.Call(ai.Globals["update"], self, others);
		split = res.Boolean;
		if (split) {
			goalSpeed=0;goalAngle=0;
		} else {
			goalSpeed = (float)ai.Globals.Get("v_t").Number;
			goalAngle = (float)ai.Globals.Get("f_t").Number;
		}
	}

	public int getTypeID()
	{
		return typeID;
	}
}
