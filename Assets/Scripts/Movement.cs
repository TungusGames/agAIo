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

		public Stats(float maxAcc, float maxSpeed, float energyMul, float statGainMul, float splitCostMul)
		{this.maxAcc = maxAcc; this.maxSpeed = maxSpeed; this.energyMul = energyMul; this.statGainMul = statGainMul; this.splitCostMul = splitCostMul;}

		public Stats() 
		{maxAcc = 5; maxSpeed = 1; energyMul = statGainMul = splitCostMul = 1;}
		public float[] asArray()
		{
			return new float[]{ maxAcc, energyMul, statGainMul, splitCostMul };
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
		transform.localScale = new Vector3(radius, radius, radius) * SimParameters.SPRITE_SCALAR;
	}
		
	// Update is called once per frame
	void Update () {
		if (myController == null) {
			if (debugByPlayer)
				myController = new PlayerController();
			else
				myController = new LuaController (0, gameObject); //Should be never reached
			myController.setup (myStats);
			InvokeRepeating ("askController", 0f, SimParameters.CONTROLLER_UPDATE_RATE);
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
                if (deltav.magnitude != 0)
					speedvector += ((energy / (deltav.magnitude / Time.deltaTime)) * deltav);
                else
					;
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
		myController.update(radius, energy, speed, angle, enemies, 
			out wantSplit, out inputSpeed, out inputAngle);
	}
		
	public void setRadius(float newR)
	{
		transform.localScale = new Vector3(newR, newR, newR) * SimParameters.SPRITE_SCALAR;
		SpawnHandler.getInstance().changeRadius(radius, newR);
		radius = newR;
	}

	public int getTypeID()
	{
		if (myController is LuaController) {
			return ((LuaController)myController).getTypeID();
		} else
			return -1;
	}


	public void initAIWithTypeID(int aiType) {
		myController = new LuaController(aiType, gameObject);
	}
}
