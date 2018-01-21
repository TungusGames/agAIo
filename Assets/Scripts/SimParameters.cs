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

	public static readonly Color[] COLORS = {
		new Color(0.337f,0.584f,0.196f),
		new Color(0.612f,0.204f,0.298f),
		new Color(0.149f,0.443f,0.345f),
		new Color(0.667f,0.373f,0.224f),
		new Color(0.667f,0.557f,0.224f)
	};
}

