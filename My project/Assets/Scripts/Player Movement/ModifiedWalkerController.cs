using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//Advanced walker controller script;
	//This controller is used as a basis for other controller types ('SidescrollerController');
	//Custom movement input can be implemented by creating a new script that inherits 'AdvancedWalkerController' and overriding the 'CalculateMovementDirection' function;
	public class ModifiedWalkerController : Controller {

		

		//References to attached components;
		protected Transform tr;
		protected Mover mover;
		protected CharacterInput characterInput;
		protected CeilingDetector ceilingDetector;

		//Jump key variables;
		bool jumpInputIsLocked = false;
		bool jumpKeyWasPressed = false;
		bool jumpKeyWasLetGo = false;
		bool jumpKeyIsPressed = false;
		public float sprintFactor = 2;
		public int maxJumps = 1;
		[SerializeField]
		int currJumps;

		//Movement speed;
		public float movementSpeed = 7f;

		//How fast the controller can change direction while in the air;
		//Higher values result in more air control;
		public float airControlRate = 2f;

		//Jump speed;
		public float jumpSpeed = 10f;

		//Jump duration variables;
		public float jumpDuration = 0.2f;
		float currentJumpStartTime = 0f;

		//'AirFriction' determines how fast the controller loses its momentum while in the air;
		//'GroundFriction' is used instead, if the controller is grounded;
		public float airFriction = 0.5f;
		public float groundFriction = 100f;
		public float beginGroundFriction = 100f;
		//Current momentum;
		protected Vector3 momentum = Vector3.zero;

		//Saved velocity from last frame;
		Vector3 savedVelocity = Vector3.zero;

		//Saved horizontal movement velocity from last frame;
		Vector3 savedMovementVelocity = Vector3.zero;

		//Amount of downward gravity;
		public float gravity = 30f;
		[Tooltip("How fast the character will slide down steep slopes.")]
		public float slideGravity = 5f;



		//Acceptable slope angle limit;
		public float slopeLimit = 80f;

		[Tooltip("Whether to calculate and apply momentum relative to the controller's transform.")]
		public bool useLocalMomentum = false;

		// Smart Fall Warp Logic Control
		public SmartFallWarp smartFallWarp;
		public bool useSmartFallWarp;


		//Wallrun stuff

		public float wallDetectDistance = 1;
		private CapsuleCollider coll;
		public Transform EntityTransform;
		[SerializeField]
		private bool wallOnRight;
		[SerializeField]
		private bool wallOnLeft;
		public float wallRunMinHeight = 0.5f;
		private Rigidbody rb;
		public float wallRunMinTangentialSpeed = 1f;

		public float standardGravity = 30;
		[SerializeField]
		private bool isWallrunning;


		//Enum describing basic controller states; 
		public enum ControllerState
		{
			Grounded,
			Sliding,
			Falling,
			Rising,
			Jumping,
			Wallrunning,
			Slipping
		}

		public ControllerState currentControllerState = ControllerState.Falling;

		[Tooltip("Optional camera transform used for calculating movement direction. If assigned, character movement will take camera view into account.")]
		public Transform cameraTransform;

		//Get references to all necessary components;
		void Awake() {
			mover = GetComponent<Mover>();
			tr = transform;
			characterInput = GetComponent<CharacterInput>();
			ceilingDetector = GetComponent<CeilingDetector>();

			if (characterInput == null)
				Debug.LogWarning("No character input script has been attached to this gameobject", this.gameObject);
			coll = GetComponent<CapsuleCollider>();
			Setup();
			rb = GetComponent<Rigidbody>();
			smartFallWarp = GetComponentInChildren<SmartFallWarp>();
			Cursor.lockState = CursorLockMode.Locked;
			colliderHeight = coll.height;
			colliderPositionY = coll.center.y;
			modelRoot = GameObject.Find("ModelRoot");
			modelRootLocalScaleHeight = modelRoot.transform.localScale.y;
		}

		//This function is called right after Awake(); It can be overridden by inheriting scripts;
		protected virtual void Setup()
		{
		}

		void Update()
		{
			HandleJumpKeyInput();
			if (useSmartFallWarp)
			{
				HandleSmartFallWarp();
			}
			wallOnRight = DetectWallRight();
			wallOnLeft = DetectWallLeft();

		}

		//Handle jump booleans for later use in FixedUpdate;
		void HandleJumpKeyInput()
		{
			bool _newJumpKeyPressedState = IsJumpKeyPressed();

			if (jumpKeyIsPressed == false && _newJumpKeyPressedState == true)
				jumpKeyWasPressed = true;

			if (jumpKeyIsPressed == true && _newJumpKeyPressedState == false)
			{
				jumpKeyWasLetGo = true;
				jumpInputIsLocked = false;
			}

			jumpKeyIsPressed = _newJumpKeyPressedState;
		}

		void FixedUpdate()
		{
			ControllerUpdate();
			//Debug.Log(DetectWallLeft() + " , " + DetectWallRight());
			//Debug.Log("Can Enter Wallrun:" + CanEnterWallrun() + ", ground clearance reached:" + (DetectDistanceToGround() > wallRunMinHeight));
		}

		//Update controller;
		//This function must be called every fixed update, in order for the controller to work correctly;
		void ControllerUpdate()
		{
			//Check if mover is grounded;
			mover.CheckForGround();

			//Determine controller state;
			currentControllerState = DetermineControllerState();

			//Apply friction and gravity to 'momentum';
			HandleMomentum();

			//Check if the player has initiated a jump;
			HandleJumping();

			//Handle wallruns
			HandleWallruns();

			//Handle crouching.
			HandleCrouch();

			//Handle sliding.
			HandleSlide();

			HandleHeight();
			//Calculate movement velocity;
			Vector3 _velocity = Vector3.zero;
			if (currentControllerState == ControllerState.Grounded)
				_velocity = CalculateMovementVelocity();

			//If local momentum is used, transform momentum into world space first;
			Vector3 _worldMomentum = momentum;
			if (useLocalMomentum)
				_worldMomentum = tr.localToWorldMatrix * momentum;

			//Add current momentum to velocity;
			_velocity += _worldMomentum;

			//If player is grounded or sliding on a slope, extend mover's sensor range;
			//This enables the player to walk up/down stairs and slopes without losing ground contact;
			mover.SetExtendSensorRange(IsGrounded());

			//Set mover velocity;		
			mover.SetVelocity(_velocity);

			//Store velocity for next frame;
			savedVelocity = _velocity;

			//Save controller movement velocity;
			savedMovementVelocity = CalculateMovementVelocity();

			//Reset jump key booleans;
			jumpKeyWasLetGo = false;
			jumpKeyWasPressed = false;

			//Reset ceiling detector, if one is attached to this gameobject;
			if (ceilingDetector != null)
				ceilingDetector.ResetFlags();
		}


		// detect walls around the player. SENSOR
		public LayerMask whatIsWall;
		private bool DetectWallLeft() {

			return Physics.Raycast(EntityTransform.position, -EntityTransform.right, (coll.bounds.size.x + wallDetectDistance), layerMask: whatIsWall);

		}
		private bool DetectWallRight() {
			return Physics.Raycast(EntityTransform.position, EntityTransform.right, (coll.bounds.size.x + wallDetectDistance), layerMask: whatIsWall);

		}

		public LayerMask whatIsGround;
		private float DetectDistanceToGround() {
            Physics.Raycast(EntityTransform.position, -EntityTransform.up, out RaycastHit hit, 100000, layerMask: whatIsGround);
            return EntityTransform.position.y - hit.point.y;
		}
		private Vector3 GetWallrunDirection()
		{
			Vector3 normal;
			Vector3 direction = new Vector3();
			RaycastHit hit;
			if (DetectWallLeft()) {
				if (Physics.Raycast(EntityTransform.position, EntityTransform.right, out hit, 100, layerMask: whatIsWall)) {

					normal = hit.normal;
					direction = Vector3.Cross(normal, EntityTransform.up);
					return direction;

				}
			}
			if (DetectWallRight())
			{
				if (Physics.Raycast(EntityTransform.position, -EntityTransform.right, out hit, 100, layerMask: whatIsWall))
				{
					normal = hit.normal;
					direction = Vector3.Cross(normal, EntityTransform.up);
					return direction;
				}


			}
			return direction;

		}
		private Vector3 GetTangentDirection(RaycastHit hit)
        {
			return Vector3.Cross(hit.normal,transform.up).normalized;

        }
		// detects if the player meet the conditions to enter a wallrun.
		// The player must be above the ground, has a high enough tangential velocity, and pressing going forward and into the wall.
		private bool CanEnterWallrun() {
			RaycastHit hit;
			//Debug.Log((wallOnLeft || wallOnRight) && DetectDistanceToGround() > wallRunMinHeight && characterInput.GetVerticalMovementInput() > 0);
			if ((wallOnLeft || wallOnRight) && DetectDistanceToGround() > wallRunMinHeight && characterInput.GetVerticalMovementInput() > 0)
            {
				if (wallOnLeft && characterInput.GetHorizontalMovementInput() < 0)
                {
					Physics.Raycast(EntityTransform.position, -EntityTransform.right, out hit, 100, layerMask: whatIsWall);
					//Debug.Log(Vector3.Project(rb.velocity, GetTangentDirection(hit)).magnitude );
					if (Vector3.Project(rb.velocity, GetTangentDirection(hit)).magnitude > wallRunMinTangentialSpeed){
						return true;
                    }
				}
				//Debug.Log(characterInput.GetHorizontalMovementInput());
				if (wallOnRight && characterInput.GetHorizontalMovementInput()> 0)
                {
					Physics.Raycast(EntityTransform.position, EntityTransform.right, out hit, 100, layerMask: whatIsWall);
					if (Vector3.Project(rb.velocity, GetTangentDirection(hit)).magnitude > wallRunMinTangentialSpeed)
                    {
						return true;
                    }

				}

            }
			return false;
		}
		// wallrun handler
		void HandleWallruns() {
			//Debug.Log(gravity);
			//Debug.Log(currentControllerState);

			if (CanEnterWallrun()) {
				isWallrunning = true;
				currentControllerState = ControllerState.Wallrunning;
				OnGroundContactRegained();
				Wallrun();
            }
            else
            {
				isWallrunning = false;
				gravity = standardGravity;
				currentControllerState = DetermineControllerState();

			}
			

			
		}


		void Wallrun() {
			
			
			if (currentControllerState == ControllerState.Wallrunning) {
				gravity = 0;

				savedMovementVelocity =CalculateMovementVelocity(GetWallrunDirection());
				momentum = new Vector3(momentum.x, 0, momentum.z);
            }


		}
		// crouching
		public float crouchHeightModifier = 0.5f;
		private float colliderHeight;
		private float colliderPositionY;
		[SerializeField]
		private bool isCrouching = false;
		[SerializeField]
		private GameObject modelRoot;
		private float modelRootLocalScaleHeight;

		public bool getCrouchState()
        {

			return isCrouching;
        }

		void HandleCrouch() {


			//Debug.Log(ceilingDetector.HitCeiling() && isCrouching);


			if (Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftShift) && currentControllerState == ControllerState.Grounded)
			{   


				isCrouching = true;
				
			}
			else if (!IsGrounded() || (!ceilingDetector.HitCeiling() && isCrouching))
            {
				isCrouching = false;
				
            }


			
		}




		public bool hasReducedHeight;
		private void HandleHeight()
        {
			hasReducedHeight = isCrouching || isSlipping;
			if (hasReducedHeight)
            {

				coll.height = colliderHeight * crouchHeightModifier;
				modelRoot.transform.localScale = new Vector3(modelRoot.transform.localScale.x, modelRootLocalScaleHeight * crouchHeightModifier, modelRoot.transform.localScale.z);
				coll.center = new Vector3(coll.center.x, colliderPositionY * crouchHeightModifier, coll.center.z);
            }
            else
            {
				coll.height = colliderHeight;
				modelRoot.transform.localScale = new Vector3(modelRoot.transform.localScale.x, modelRootLocalScaleHeight, modelRoot.transform.localScale.z);
				coll.center = new Vector3(coll.center.x, colliderPositionY, coll.center.z);

			}

		}






		// when sliding down, raycast down and get angle of ground.
		// after getting the angle of the ground, snap the player to somewhere above the slope
		// and assign him a momentum that has the same x and z as his momentum before the sliding but different y
		// 

		private float slideSpeed;
		//public float standardSlideSpeed = 24;
		public float beginSlideSpeedThreshold = 6;
		[SerializeField]
		public bool isSlipping = false;
		public float slideBeginTime = 0;
		public float slideEndTime = 4;
		private Vector3 slideDirection;
		private bool hasSlided = false;
		public LayerMask whatisPlayer;
		private void HandleSlide()
        {

			//Debug.Log("Sliding: " + isSlipping + " beginVelocity:" + slideDirection + " Grounded: " + IsGrounded() + " Momentum: " + momentum + " slide speed: " + slideSpeed + "has slided: " + hasSlided);

			if (Input.GetKeyUp(KeyCode.C))
				hasSlided = false;


			if (Mathf.Abs(rb.velocity.magnitude) > beginSlideSpeedThreshold)
            {
				//Debug.Log(1);
				if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.C) && !hasSlided)
				{
					//Debug.Log(2);


					if (!isSlipping)
					{
						slideDirection = rb.velocity;
						slideBeginTime = Time.time;
						slideSpeed = rb.velocity.magnitude;
					}
					isSlipping = true;
				
                }
                else
                {
					isSlipping = false;
                }

            }
            else
            {
				isSlipping= false;
				hasSlided = false;
            }

			//Debug.DrawRay(tr.position, Vector3.down, Color.red);
			if (isSlipping)
            {
				OnGroundContactLost();
				rb.AddForce(-transform.up * 250, ForceMode.Force);
				groundFriction = 0;
				rb.AddForce(-transform.up * 100, ForceMode.Force);

                if (Physics.Raycast(tr.position + tr.up * 2f, -tr.up, out RaycastHit hit, 10, ~whatisPlayer))
                {
                    // this is the angle the player makes with the normal. 
                    // calculating the gravity * sin(angle) gives us the magnitude of the acceleration that the player should receive. 
                    //Debug.Log("Raycast hits " + hit.collider.gameObject);
                }
                else
                {
                    //Debug.Log("Raycast did not hit");
                }
                Vector3 normal = hit.normal;
				Debug.Log(Vector3.Angle(tr.up, hit.normal));
				float slopeAngle = Vector3.Angle(tr.up, hit.normal);
				Debug.Log(momentum);
				
				
				float slideAcceleration = standardGravity * (Mathf.Abs(Mathf.Sin(slopeAngle * Mathf.Deg2Rad))) * 
					Mathf.Cos(Mathf.Deg2Rad* Vector3.Angle( new Vector3(slideDirection.x, 0, slideDirection.z),new Vector3(normal.x, 0, normal.z))) ;
				
				Debug.Log("Acceleration: " + slideAcceleration + " Speed: " + slideSpeed + 
					"Slope Direction: " + new Vector3(normal.x, 0, normal.z) + " Normal Direction: " + slopeAngle);

				Debug.Log(Vector3.Angle(new Vector3(slideDirection.x, 0, slideDirection.z), new Vector3(normal.x, 0, normal.z)));
				if (slideSpeed >= 7)
				{
					if(IsGrounded())
						slideSpeed += -Time.deltaTime * 10 + Time.deltaTime*slideAcceleration;
                }
                else
                {
					slideSpeed = 0;
					isSlipping = false;
					hasSlided = true;
                }
            }
            else
            {
				groundFriction = beginGroundFriction;
			}

        }




		//Calculate and return movement direction based on player input;
		//This function can be overridden by inheriting scripts to implement different player controls;
		
		protected virtual Vector3 CalculateMovementDirection()
		{
			//If no character input script is attached to this object, return;
			if(characterInput == null)
				return Vector3.zero;

			Vector3 _velocity = Vector3.zero;

			//If no camera transform has been assigned, use the character's transform axes to calculate the movement direction;
			if(cameraTransform == null)
			{
				_velocity += tr.right * characterInput.GetHorizontalMovementInput();
				_velocity += tr.forward * characterInput.GetVerticalMovementInput();
			}
			else
			{
				//If a camera transform has been assigned, use the assigned transform's axes for movement direction;
				//Project movement direction so movement stays parallel to the ground;
				_velocity += Vector3.ProjectOnPlane(cameraTransform.right, tr.up).normalized * characterInput.GetHorizontalMovementInput();
				_velocity += Vector3.ProjectOnPlane(cameraTransform.forward, tr.up).normalized * characterInput.GetVerticalMovementInput();
			}

			//If necessary, clamp movement vector to magnitude of 1f;
			if(_velocity.magnitude > 1f)
				_velocity.Normalize();

			return _velocity;
		}

		//Calculate and return movement velocity based on player input, controller state, ground normal [...];
		protected virtual Vector3 CalculateMovementVelocity()
		{
			//Calculate (normalized) movement direction;
			Vector3 _velocity = CalculateMovementDirection();

			//Multiply (normalized) velocity with movement speed;
			if(Input.GetKey(KeyCode.LeftShift)) 
            {
                _velocity *= movementSpeed * sprintFactor;
            }
			else if (isCrouching)
            {
                _velocity *= movementSpeed * crouchModifier;
            }
			else
            {
				_velocity *= movementSpeed;
            }


			if (isSlipping)
			{
				_velocity = new Vector3(slideDirection.normalized.x,0,slideDirection.normalized.z) * slideSpeed;

			}
				//Debug.Log("next velocity: " + _velocity);
			return _velocity;
		}


		public float crouchModifier = 0.5f;
		protected virtual Vector3 CalculateMovementVelocity(Vector3 direction)
		{
			//use input
			Vector3 _velocity = direction;



			//Debug.Log(isCrouching);
			//Multiply (normalized) velocity with movement speed;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				_velocity *= movementSpeed * sprintFactor;
			}
			else if(isCrouching)
			{
				//Debug.Log("crouch Speed calculated");
				_velocity *= movementSpeed * crouchModifier;
            }
            else
            {
				_velocity *= movementSpeed;
            }


			return _velocity;
		}





		//Returns 'true' if the player presses the jump key;
		protected virtual bool IsJumpKeyPressed()
		{
			//If no character input script is attached to this object, return;
			if(characterInput == null)
				return false;

			return characterInput.IsJumpKeyPressed();
		}

		//Determine current controller state based on current momentum and whether the controller is grounded (or not);
		//Handle state transitions;
		ControllerState DetermineControllerState()
		{
			//Check if vertical momentum is pointing upwards;
			bool _isRising = IsRisingOrFalling() && (VectorMath.GetDotProduct(GetMomentum(), tr.up) > 0f);
			//Check if controller is sliding;
			bool _isSliding = mover.IsGrounded() && IsGroundTooSteep();
			




			if (currentControllerState == ControllerState.Wallrunning)
            {
				OnGroundContactRegained();
				currJumps = 0;
				if (!CanEnterWallrun())
                {
					return ControllerState.Falling;
                }


				return ControllerState.Wallrunning;
            }

			//Grounded;
			if(currentControllerState == ControllerState.Grounded)
			{
				currJumps = 0;
				if(_isRising){
					OnGroundContactLost();
					return ControllerState.Rising;
				}
				if(!mover.IsGrounded()){
					OnGroundContactLost();
					return ControllerState.Falling;
				}
				if(_isSliding){
					OnGroundContactLost();
					return ControllerState.Sliding;
				}
				return ControllerState.Grounded;
			}

			//Falling;
			if(currentControllerState == ControllerState.Falling)
			{
				if (isWallrunning)
                {
					return ControllerState.Falling;
                }

				if(_isRising){
					return ControllerState.Rising;
				}
				if(mover.IsGrounded() && !_isSliding){
					OnGroundContactRegained();
					return ControllerState.Grounded;
				}
				if(_isSliding){
					return ControllerState.Sliding;
				}
				return ControllerState.Falling;
			}
			
			//Sliding;
			if(currentControllerState == ControllerState.Sliding)
			{	
				if(_isRising){
					OnGroundContactLost();
					return ControllerState.Rising;
				}
				if(!mover.IsGrounded()){
					OnGroundContactLost();
					return ControllerState.Falling;
				}
				if(mover.IsGrounded() && !_isSliding){
					OnGroundContactRegained();
					return ControllerState.Grounded;
				}
				return ControllerState.Sliding;
			}

			//Rising;
			if(currentControllerState == ControllerState.Rising)
			{
				if(!_isRising){
					if(mover.IsGrounded() && !_isSliding){
						OnGroundContactRegained();
						return ControllerState.Grounded;
					}
					if(_isSliding){
						return ControllerState.Sliding;
					}
					if(!mover.IsGrounded()){
						return ControllerState.Falling;
					}
				}

				//If a ceiling detector has been attached to this gameobject, check for ceiling hits;
				if(ceilingDetector != null)
				{
					if(ceilingDetector.HitCeiling())
					{
						OnCeilingContact();
						return ControllerState.Falling;
					}
				}
				return ControllerState.Rising;
			}

			//Jumping;
			if(currentControllerState == ControllerState.Jumping)
			{
				//Check for jump timeout;
				if((Time.time - currentJumpStartTime) > jumpDuration)
					return ControllerState.Rising;

				//Check if jump key was let go;
				if(jumpKeyWasLetGo)
					return ControllerState.Rising;

				//If a ceiling detector has been attached to this gameobject, check for ceiling hits;
				if(ceilingDetector != null)
				{
					if(ceilingDetector.HitCeiling())
					{
						OnCeilingContact();
						return ControllerState.Falling;
					}
				}
				return ControllerState.Jumping;
			}
			
			return ControllerState.Falling;
		}

        //Check if player has initiated a jump;
        void HandleJumping()
        {
            if ((currentControllerState == ControllerState.Grounded || currJumps < maxJumps) && maxJumps != 0)
            {

                if ((jumpKeyIsPressed == true || jumpKeyWasPressed) && !jumpInputIsLocked)
                {
					currJumps++;
                    //Call events;
                    OnGroundContactLost();
                    OnJumpStart();

                    currentControllerState = ControllerState.Jumping;
                }
            }
        }

        //Apply friction to both vertical and horizontal momentum based on 'friction' and 'gravity';
		//Handle movement in the air;
        //Handle sliding down steep slopes;
        void HandleMomentum()
		{

			//Debug.Log(currentControllerState + ", momentum: " + momentum);


			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			Vector3 _verticalMomentum = Vector3.zero;
			Vector3 _horizontalMomentum = Vector3.zero;

			//Split momentum into vertical and horizontal components;
			if(momentum != Vector3.zero)
			{
				_verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
				_horizontalMomentum = momentum - _verticalMomentum;
			}

			//Add gravity to vertical momentum;
			_verticalMomentum -= gravity * Time.deltaTime * tr.up;

			//Remove any downward force if the controller is grounded;
			if(currentControllerState == ControllerState.Grounded && VectorMath.GetDotProduct(_verticalMomentum, tr.up) < 0f)
				_verticalMomentum = Vector3.zero;

			//Manipulate momentum to steer controller in the air (if controller is not grounded or sliding);
			if(!IsGrounded())
			{
				Vector3 _movementVelocity = CalculateMovementVelocity();

				//If controller has received additional momentum from somewhere else;
				if(_horizontalMomentum.magnitude > movementSpeed)
				{
					//Prevent unwanted accumulation of speed in the direction of the current momentum;
					if(VectorMath.GetDotProduct(_movementVelocity, _horizontalMomentum.normalized) > 0f)
						_movementVelocity = VectorMath.RemoveDotVector(_movementVelocity, _horizontalMomentum.normalized);
					
					//Lower air control slightly with a multiplier to add some 'weight' to any momentum applied to the controller;
					float _airControlMultiplier = 0.25f;
					_horizontalMomentum += _airControlMultiplier * airControlRate * Time.deltaTime * _movementVelocity;
				}
				//If controller has not received additional momentum;
				else
				{
					//Clamp _horizontal velocity to prevent accumulation of speed;
					_horizontalMomentum += airControlRate * Time.deltaTime * _movementVelocity;
					_horizontalMomentum = Vector3.ClampMagnitude(_horizontalMomentum, movementSpeed);
				}
			}

			//Steer controller on slopes;
			if(currentControllerState == ControllerState.Sliding)
			{
				//Calculate vector pointing away from slope;
				Vector3 _pointDownVector = Vector3.ProjectOnPlane(mover.GetGroundNormal(), tr.up).normalized;

				//Calculate movement velocity;
				Vector3 _slopeMovementVelocity = CalculateMovementVelocity();
				//Remove all velocity that is pointing up the slope;
				_slopeMovementVelocity = VectorMath.RemoveDotVector(_slopeMovementVelocity, _pointDownVector);

				//Add movement velocity to momentum;
				_horizontalMomentum += _slopeMovementVelocity * Time.fixedDeltaTime;
			}

			//Apply friction to horizontal momentum based on whether the controller is grounded;
			if(currentControllerState == ControllerState.Grounded)
				_horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(_horizontalMomentum, groundFriction, Time.deltaTime, Vector3.zero);
			else
				_horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(_horizontalMomentum, airFriction, Time.deltaTime, Vector3.zero); 

			//Add horizontal and vertical momentum back together;
			momentum = _horizontalMomentum + _verticalMomentum;

			//Additional momentum calculations for sliding;
			if(currentControllerState == ControllerState.Sliding)
			{
				//Project the current momentum onto the current ground normal if the controller is sliding down a slope;
				momentum = Vector3.ProjectOnPlane(momentum, mover.GetGroundNormal());

				//Remove any upwards momentum when sliding;
				if(VectorMath.GetDotProduct(momentum, tr.up) > 0f)
					momentum = VectorMath.RemoveDotVector(momentum, tr.up);

				//Apply additional slide gravity;
				Vector3 _slideDirection = Vector3.ProjectOnPlane(-tr.up, mover.GetGroundNormal()).normalized;
				momentum += slideGravity * Time.deltaTime * _slideDirection;
			}
			
			//If controller is jumping, override vertical velocity with jumpSpeed;
			if(currentControllerState == ControllerState.Jumping)
			{
				momentum = VectorMath.RemoveDotVector(momentum, tr.up);
				momentum += tr.up * jumpSpeed;
			}


			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Events;

		public float wallrunUpSpeed = 2;
		//This function is called when the player has initiated a jump;
		void OnJumpStart()
		{
			if (isWallrunning) {
				momentum += tr.up * wallrunUpSpeed;
				return;
			}


			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Add jump force to momentum;
			momentum += tr.up * jumpSpeed;

			//Set jump start time;
			currentJumpStartTime = Time.time;

            //Lock jump input until jump key is released again;
            jumpInputIsLocked = true;

            //Call event;
            if (OnJump != null)
				OnJump(momentum);

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//This function is called when the controller has lost ground contact, i.e. is either falling or rising, or generally in the air;
		void OnGroundContactLost()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Get current movement velocity;
			Vector3 _velocity = GetMovementVelocity();

			//Check if the controller has both momentum and a current movement velocity;
			if(_velocity.sqrMagnitude >= 0f && momentum.sqrMagnitude > 0f)
			{
				//Project momentum onto movement direction;
				Vector3 _projectedMomentum = Vector3.Project(momentum, _velocity.normalized);
				//Calculate dot product to determine whether momentum and movement are aligned;
				float _dot = VectorMath.GetDotProduct(_projectedMomentum.normalized, _velocity.normalized);

				//If current momentum is already pointing in the same direction as movement velocity,
				//Don't add further momentum (or limit movement velocity) to prevent unwanted speed accumulation;
				if(_projectedMomentum.sqrMagnitude >= _velocity.sqrMagnitude && _dot > 0f)
					_velocity = Vector3.zero;
				else if(_dot > 0f)
					_velocity -= _projectedMomentum;	
			}

			//Add movement velocity to momentum;
			momentum += _velocity;

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//This function is called when the controller has landed on a surface after being in the air;
		void OnGroundContactRegained()
		{
			//Call 'OnLand' event;
			if(OnLand != null)
			{
				Vector3 _collisionVelocity = momentum;
				//If local momentum is used, transform momentum into world coordinates first;
				if(useLocalMomentum)
					_collisionVelocity = tr.localToWorldMatrix * _collisionVelocity;

				OnLand(_collisionVelocity);
				currJumps = 0;
			}
				
		}

		//This function is called when the controller has collided with a ceiling while jumping or moving upwards;
		void OnCeilingContact()
		{
			//If local momentum is used, transform momentum into world coordinates first;
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			//Remove all vertical parts of momentum;
			momentum = VectorMath.RemoveDotVector(momentum, tr.up);

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Helper functions;

		//Returns 'true' if vertical momentum is above a small threshold;
		private bool IsRisingOrFalling()
		{
			//Calculate current vertical momentum;
			Vector3 _verticalMomentum = VectorMath.ExtractDotVector(GetMomentum(), tr.up);

			//Setup threshold to check against;
			//For most applications, a value of '0.001f' is recommended;
			float _limit = 0.001f;

			//Return true if vertical momentum is above '_limit';
			return(_verticalMomentum.magnitude > _limit);
		}

		//Returns true if angle between controller and ground normal is too big (> slope limit), i.e. ground is too steep;
		private bool IsGroundTooSteep()
		{
			if(!mover.IsGrounded())
				return true;

			return (Vector3.Angle(mover.GetGroundNormal(), tr.up) > slopeLimit);
		}

		//Getters;

		//Get last frame's velocity;
		public override Vector3 GetVelocity ()
		{
			return savedVelocity;
		}

		//Get last frame's movement velocity (momentum is ignored);
		public override Vector3 GetMovementVelocity()
		{
			return savedMovementVelocity;
		}

		//Get current momentum;
		public Vector3 GetMomentum()
		{
			Vector3 _worldMomentum = momentum;
			if(useLocalMomentum)
				_worldMomentum = tr.localToWorldMatrix * momentum;

			return _worldMomentum;
		}

		//Returns 'true' if controller is grounded (or sliding down a slope);
		public override bool IsGrounded()
		{
			return(currentControllerState == ControllerState.Grounded || currentControllerState == ControllerState.Sliding);
		}

		//Returns 'true' if controller is sliding;
		public bool IsSliding()
		{
			return(currentControllerState == ControllerState.Sliding);
		}

		//Add momentum to controller;
		public void AddMomentum (Vector3 _momentum)
		{
			if(useLocalMomentum)
				momentum = tr.localToWorldMatrix * momentum;

			momentum += _momentum;	

			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * momentum;
		}

		//Set controller momentum directly;
		public void SetMomentum(Vector3 _newMomentum)
		{
			if(useLocalMomentum)
				momentum = tr.worldToLocalMatrix * _newMomentum;
			else
				momentum = _newMomentum;
		}

		// Smart warp control variables
		bool smartWarpUpdateReady = true;
		float smartWarpTimeGap = 1.0f;
		float currSmarWarpTimeGap = 0.0f;

		void HandleSmartFallWarp()
        {
            if(currentControllerState == ControllerState.Grounded && smartWarpUpdateReady)
			{
				Debug.Log("New warp point set!");
				currSmarWarpTimeGap = 0.0f;
				smartWarpUpdateReady = false;
				smartFallWarp.SmartWarpWaypointSet();
			}
			else
            {
				currSmarWarpTimeGap += Time.deltaTime;
				if(currSmarWarpTimeGap >= smartWarpTimeGap)
                {
					smartWarpUpdateReady = true;
                }
            }
        }
	}

}
