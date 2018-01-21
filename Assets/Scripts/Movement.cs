using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {


	public class Stats {
		public float maxAcc;
		public float maxSpeed;
		public float energyMul;
		public float statGainMul;
		public float splitCostMul;
		public Stats(float maxAcc, float maxSpeed, float energyMul, float statGainMul, float splitCostMul) {
			this.maxAcc = maxAcc;
			this.maxSpeed = maxSpeed;
			this.energyMul = energyMul;
			this.statGainMul = statGainMul;
			this.splitCostMul = splitCostMul;
		}
		public Stats() 
		{maxAcc = 5; maxSpeed = 1; energyMul = statGainMul = splitCostMul = 1;}
		public float[] asArray() {
			return new float[]{ maxAcc, maxSpeed, energyMul, statGainMul, splitCostMul };
		}
	}

	[SerializeField]
	private Stats myStats = new Stats();

	[SerializeField]
	bool debugByPlayer = false;

	[SerializeField]
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
		transform.localScale = new Vector3(radius, radius, radius) * SimParameters.SPRITE_SCALAR;
	}
		
	// Update is called once per frame
	void Update () {
		if (myController == null) {
			if (debugByPlayer)
				initController(-1);
			else
				initController(0); //Should never be reached
		}

		//Shrink
		setRadius(radius*(1-Time.deltaTime*0.01f));

        //speed
		Vector2 speedvector =new Vector2 (Mathf.Cos(angle*Mathf.Deg2Rad) * speed, Mathf.Sin(angle*Mathf.Deg2Rad) * speed);
        //Debug.Log("speedvector1 " + speedvector);
		Vector2 targetspeedvector=new Vector2(Mathf.Cos(inputAngle*Mathf.Deg2Rad) * inputSpeed*myStats.maxSpeed, Mathf.Sin(inputAngle*Mathf.Deg2Rad) * inputSpeed*myStats.maxSpeed);
        Vector2 deltav = targetspeedvector - speedvector;
        if (deltav.magnitude/Time.deltaTime <= myStats.maxAcc*(1-radius/1000))
        {
            if(energy>= deltav.magnitude/Time.deltaTime)
            {
                energy -= deltav.magnitude/ Time.deltaTime;
                speedvector = targetspeedvector;
            }
            else
            {
                if (deltav.magnitude != 0)
                {
                    speedvector += ((energy / (deltav.magnitude / Time.deltaTime)) * deltav);
                }
                else
                {
                }
                energy = 0;
            }
        }
        else
        {
            if (deltav.magnitude != 0)
            {
                deltav = ((myStats.maxAcc * (1 - radius / 1000)) / (deltav.magnitude / Time.deltaTime)) * deltav;
            }
            else
            {
                deltav.x = 0;
                deltav.y = 0;
            }
            if (energy >= deltav.magnitude/ Time.deltaTime)
            {
                energy -= deltav.magnitude/ Time.deltaTime;
                speedvector = targetspeedvector;
                
            }
            else
            {
				if (deltav.magnitude != 0) {
					speedvector += ((energy / (deltav.magnitude / Time.deltaTime)) * deltav);
				}
                energy = 0;
                
            }

        }
        //Debug.Log("speed: " + speed + " angle: " + angle + " speedvector: " + speedvector + " deltav: " + deltav + " targetspeed: " + targetspeedvector);
        //Debug.Log("energy" + energy);
		energy += 10*Time.deltaTime;
        speed = speedvector.magnitude;
        if (speedvector.magnitude > 0)
        {
			angle = Mathf.Atan2(speedvector.y, speedvector.x)*Mathf.Rad2Deg;
        }
        else
        {
            angle = 0;
        }
        transform.Translate(speedvector.x * Time.deltaTime, speedvector.y * Time.deltaTime, 0);
	}
		
	void askController()
	{
		Collider2D[] enemiesColliders = Physics2D.OverlapCircleAll (transform.position, radius * SimParameters.SIGHT_PER_RADIUS);
		GameObject[] enemies = new GameObject[enemiesColliders.Length-1];
		for (int i = 0, j = 0; j < enemiesColliders.Length; ++j) {
			if (enemiesColliders[j].gameObject != gameObject) {
				enemies [i++] = enemiesColliders [j].gameObject;
			}
		}
		if (wantSplit)
			split();
		myController.update(radius, energy, speed, angle, enemies, 
			out wantSplit, out inputSpeed, out inputAngle);
	}
		
	public void setRadius(float newR) {
		transform.localScale = new Vector3(newR, newR, newR) * SimParameters.SPRITE_SCALAR;
		SpawnHandler.getInstance().changeRadius(radius, newR);
		radius = newR;
	}
		
	public int getTypeID() {
		if (myController is LuaController) {
			return ((LuaController)myController).getTypeID();
		} else
			return -1;
	}
		
	public void split() {
		float[] stats = { myStats.maxAcc, myStats.maxSpeed, myStats.energyMul, myStats.statGainMul, myStats.splitCostMul };
		float[] weights;
		float[] stats1 = new float[5];
		float[] stats2 = new float[5];
		myController.getEvolve (out weights);
		float stat_max = stats [3];
		float sum=0;
		for (int i = 0; i < 5; i++)
			sum += weights [i];
		if (sum > stat_max) {
			for (int i = 0; i < 5; i++)
				weights[i]=weights[i]*stat_max/sum;
		}
		for(int i=0;i<5;i++){
			stats1[i]= stats[i] + weights[i]/2 + Gaussian.getGaussian(0, weights[i]);
		}
		for(int i=0;i<5;i++){
			stats2[i]= stats[i] + weights[i]/2 + Gaussian.getGaussian(0, weights[i]);
		}

		float r=radius/2*(1-100/myStats.splitCostMul);
		float dir = Random.value * 180;

		float x = transform.position.x;
		float y = transform.position.y;
		Stats statsa = new Stats(stats1 [0], stats1 [1], stats1 [2], stats1 [3], stats1 [4]);
		Stats statsb = new Stats(stats2 [0], stats2 [1], stats2 [2], stats2 [3], stats2 [4]);

		SpawnHandler.getInstance().addAI (getTypeID (), x + r * Mathf.Sin (dir), y + r * Mathf.Sin (dir), r, 0, dir, 0, statsa);
		SpawnHandler.getInstance().addAI(getTypeID(), x-r*Mathf.Sin(dir), y-r*Mathf.Sin(dir), r, 0, dir+180, 	0, statsb);
		transform.SetPositionAndRotation(new Vector3 (1e8f, 1e8f, 0f), Quaternion.identity);
		Destroy(gameObject);	
	}

	private void initController(int aiType) {
		if (aiType == -1) {
			myController = new PlayerController ();
		} else {
			myController = new LuaController (aiType, gameObject);
		}
		myController.setup (myStats);
		InvokeRepeating ("askController", 0f, SimParameters.CONTROLLER_UPDATE_RATE);
	}

	public void init(int aiType, float r, float E, float angle, float speed, Stats stats) {
		initController (aiType);
		setRadius(r);
		energy = E;
		this.angle = angle;
		this.speed = speed;
		myStats = stats;
	}
		
}
