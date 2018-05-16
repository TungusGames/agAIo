using System;
using UnityEngine;

public class SimParameters
{
	public static readonly float CONTROLLER_UPDATE_RATE = 0.2f;
	public static readonly float MAX_TOTAL_RADIUS_SQUARED = 120f;
	public static readonly float MAP_WIDTH = 30f;
	public static readonly float MAP_HEIGHT = 30f;
	public static readonly float MIN_SPAWN_RADIUS = 0.5f;
	public static readonly float MAX_SPAWN_RADIUS = 1f;
	public static readonly float SPRITE_SCALAR = 11f;
	public static readonly float SIGHT_PER_RADIUS = 6f;
	public static readonly float MAP_RADIUS = 22f;
	public static System.Random rnd=new System.Random();
	public static readonly Color[] COLORS = {
		new Color((float) (rnd.NextDouble()/5+0.1),(float) (rnd.NextDouble()/5+0.6),(float) (rnd.NextDouble()/5+0.1)),
		new Color((float) (rnd.NextDouble()/5+0.6),(float) (rnd.NextDouble()/10),(float) (rnd.NextDouble()/3+0.2)),
		new Color((float) (rnd.NextDouble()/6+0.2),(float) (rnd.NextDouble()/6+0.4),(float) (rnd.NextDouble()/5+0.75)),
		new Color((float) (rnd.NextDouble()/5+0.75),(float) (rnd.NextDouble()/3+0.2),(float) (rnd.NextDouble()/5)),
		new Color((float) (rnd.NextDouble()/5+0.75),(float) (rnd.NextDouble()/5+0.75),(float) (rnd.NextDouble()/2))
	};
}

