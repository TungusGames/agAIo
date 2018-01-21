using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Controller  {

	void setup (Movement.Stats stats);
	void update (float radius, float energy, float speed, float angle, GameObject[] enemies, 
		out bool split, out float goalSpeed, out float goalAngle);
	void getEvolve (out  float[] weights);
}
