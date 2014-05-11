using UnityEngine;
using System.Collections;

public class CupPlayerManager : MonoBehaviour {

	static CupPlayerManager myInstance;
	static int instances = 0;

	//Just to see if it will be really necessary! Probably some will...
	float speed 							= 0.0f;						//The actual vertical speed of the submarine
	float newSpeed 							= 0.0f;						//The new speed of the submarine, used at the edges
	float distanceToMax;												//The current distance to the maximum depth
	float distanceToMin;												//The current distance to the minimum depth

	bool isJumping							= false;					//See if the character is jumping
	bool paused								= false;					//The game is paused/unpaused
	bool controlIsEnabled					= true;					//The Control is Enabled
	bool movingUpward 						= false;					//The Char is rising

	public float maxVerticalSpeed 			= 45.0f;					//The maximum vertical speed
	public float depthEdge					= 1.0f;					//The edge fo the smoothing zone (minDepth- depthEdge and maxDepth - depthEdge)

	public float minDepth 					= 2f;						//Minimum depth
	public float maxDepth 					= -2f;						//Maximum depth



	Transform thisTransform;											//The transform of this object stored

	// Use this for initialization
	void Start () {
		//Calibrates the myInstance static variable
		instances++;
		
		if (instances > 1)
			Debug.Log("Warning: There are more than one Player Manager at the level");
		else
			myInstance = this;
		
		//Set transform and rotationDiv
		thisTransform = this.GetComponent<Transform>();
		//rotationDiv = maxVerticalSpeed / maxRotation;
	}
	
	// Update is called once per frame
	void Update () {
		//If the control are enabled
		if (controlIsEnabled)
		{
			//Calculate smooth zone distance
			CalculateDistances();
			
			//Calculate player movement
			CalculateMovement();
			
			//Move The Character
			MoveChar();
		}
	}

	//Returns the instance
	public static CupPlayerManager Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType(typeof(CupPlayerManager)) as CupPlayerManager;
			
			return myInstance;
		}
	}

	void CalculateDistances(){
		distanceToMax = thisTransform.position.y - maxDepth;
		distanceToMin = minDepth - thisTransform.position.y;
	}

	void CalculateMovement(){
		//If the sub is moving up
		if (movingUpward)
		{
			//Increase speed
			speed += Time.deltaTime * maxVerticalSpeed;
			
			//If the sub is too close to the minDepth
			if (distanceToMin < depthEdge)
			{
				//Calculate maximum speed at this depth (without this, the sub would leave the gameplay are)
				newSpeed = maxVerticalSpeed * (minDepth - thisTransform.position.y) / depthEdge;
				
				//If the newSpeed is lesser the the current speed
				if (newSpeed < speed)
					//Make newSpeed the current speed
					speed = newSpeed;
			}
			//If the sub is too close to the maxDepth
			else if (distanceToMax < depthEdge)
			{
				//Calculate maximum speed at this depth (without this, the sub would leave the gameplay are)
				newSpeed = maxVerticalSpeed * (maxDepth - thisTransform.position.y) / depthEdge;
				
				//If the newSpeed is greater the the current speed
				if (newSpeed > speed)
					//Make newSpeed the current speed
					speed = newSpeed;
			}
		}
		//If the sub is moving down
		else
		{
			//Decrease speed
			speed -= Time.deltaTime * maxVerticalSpeed;

			
			//If the sub is too close to the maxDepth
			if (distanceToMax < depthEdge)
			{
				//Calculate maximum speed at this depth (without this, the sub would leave the gameplay are)
				newSpeed = maxVerticalSpeed * (maxDepth - thisTransform.position.y) / depthEdge;
				
				//If the newSpeed is greater the the current speed
				if (newSpeed > speed)
					//Make newSpeed the current speed
					speed = newSpeed;
			}
			//If the sub is too close to the minDepth
			else if (distanceToMin < depthEdge)
			{
				//Calculate maximum speed at this depth (without this, the sub would leave the gameplay are)
				newSpeed = maxVerticalSpeed * (minDepth - thisTransform.position.y) / depthEdge;
				
				//If the newSpeed is lesser the the current speed
				if (newSpeed < speed)
					//Make newSpeed the current speed
					speed = newSpeed;
			}
		}
	}

	void MoveChar(){
		thisTransform.position += Vector3.up * speed * Time.deltaTime;
	}

	public void MoveUp()
	{
		//If the player is not at the min depth, and the controls are enabled, move up
		if (distanceToMin > 0 && controlIsEnabled)	
			movingUpward = true;
	}
	//Called from the Input manager
	public void MoveDown()
	{
		//If the player is not at the max depth, and the controls are enabled, move down
		if (distanceToMax > 0 && controlIsEnabled)	
			movingUpward = false;

	}

	//Pause the Game
	public void Pause()
	{
		paused = true;
		//bubbles.Pause();
	}
}
