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
			split();
	}
		
	public void setRadius(float newR) {
		transform.localScale = new Vector3(newR, newR, newR) * SimParameters.SPRITE_SCALAR;
		SpawnHandler.getInstance().changeRadius(radius, newR);
		radius = newR;
	}

	public void split() {
		float[] stats = { myStats.maxSpeed, myStats.energyMul, myStats.statGainMul, myStats.splitCostMul };
		float[] weights = new float[4];
		float[] stats1 = new float[4];
		float[] stats2 = new float[4];
		myController.getEvolve (weights);
		int stat_max=weights [2]
		int sum=0;
		for (int i = 0; i < 4; i++)
			sum += weights [i];
		if (sum > stat_max) {
			for (int i = 0; i < 4; i++)
				weights[i]=weights[i]*stat_max/sum;
		}

		for(int i=0;i<4;i++){
			stats1[i]= stats[i] + weights[i]/2 + Gaussian.getGaussian(0, weights[i]);
		}
		for(int i=0;i<4;i++){
			stats2[i]= stats[i] + weights[i]/2 + Gaussian.getGaussian(0, weights[i]);
		}

		float r=radius/2*(1-100/myStats.splitCostMul);
		float dir = Random.value * 180;

		SpawnHandler.newAI(x+r*Mathf.Sin(dir), y+r*Mathf.Sin(dir), 0, r, dir, 		0, stats1[0], stats1[1], stats1[2], stats1[3]);
		SpawnHandler.newAI(x-r*Mathf.Sin(dir), y-r*Mathf.Sin(dir), 0, r, dir+180, 	0, stats2[0], stats2[1], stats2[2], stats2[3]);

		Destroy(gameObject);	
	}

}
