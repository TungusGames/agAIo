using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


	public class Stats {
		public float maxSpeed;
		public float maxRot;
		public float energyMul;
		public float statGainMul;
		public float splitCostMul;
		public Stats(float maxSpeed, float maxRot, float energyMul, float statGainMul, float splitCostMul)
		{maxSpeed = maxSpeed; maxRot = maxRot; energyMul = energyMul; statGainMul = statGainMul; splitCostMul = splitCostMul;}
		public Stats() 
		{maxSpeed = 5; maxRot = 20; energyMul = statGainMul = splitCostMul = 1;}
		public float[] asArray()
		{
			return new float[]{ maxSpeed, maxRot, energyMul, statGainMul, splitCostMul };
		}
	}

	[SerializeField]
	private Stats myStats = new Stats();

	[SerializeField]
	bool debugByPlayer = false;

	private Controller myController;

	private float energy = 0;
	public float radius = 0.1f;
	private float angle = 0;
	private float speed = 1;

	float inputSpeed, inputAngle;
	bool wantSplit;

	// Use this for initialization
	void Start () {
		//myController = new DummyController(); 
		myController = null;
		setRadius(radius);
	}
		
	// Update is called once per frame
	void Update () {
		if (myController == null) {
			if (debugByPlayer)
				myController = new PlayerController();
			else
				myController = new LuaController (0);
			myController.setup (myStats);
			InvokeRepeating ("askController", 0f, SimParameters.CONTROLLER_UPDATE_RATE);
		}
		speed = inputSpeed*myStats.maxSpeed;
		float dAngle = inputAngle - angle;
		dAngle %= 360;
		if (dAngle < -180)
			dAngle += 360;
		if (dAngle > 180)
			dAngle -= 360;
		if (dAngle > 0)
			angle = Mathf.Min(inputAngle, angle + Time.deltaTime*myStats.maxRot);
		if (dAngle < 0)
			angle = Mathf.Max(inputAngle, angle - Time.deltaTime * myStats.maxRot);
		transform.Translate(Mathf.Cos(angle) * speed * Time.deltaTime, Mathf.Sin(angle) * speed * Time.deltaTime, 0);
	}

	void askController() {
		myController.update(radius, energy, speed, angle, new GameObject[0], out wantSplit, out inputSpeed, out inputAngle);
		if (wantSplit)
			;
			//Split();
	}
		
	public void setRadius(float newR) {
		transform.localScale = new Vector3(newR, newR, newR) * SimParameters.SPRITE_SCALAR;
		SpawnHandler.getInstance().changeRadius(radius, newR);
		radius = newR;
	}

	public void split() {
		
		float dir = Random.value * 180;
	
	}

}
