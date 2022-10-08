using UnityEngine;
using System.Collections;

namespace CraftingAnims{
	
	public class CrafterController : MonoBehaviour{
		
		public enum CharacterState{
			Idle,
			Item,
			Box,
			Fishing,
			Hammer,
			Digging,
			Chopping,
			Food,
			Drink,
			Axe,
			Shovel,
			FishingPole,
			Saw,
			Sawing,
			PickAxe,
			PickAxing,
			Sickle,
			Rake,
			Spear,
			Raking,
			Sit,
			Laydown,
			Climb,
			PushPull,
			Lumber,
			Overhead,
			Pray,
			Cart,
			Kneel,
			Painting,
			Use,
			Crawl
		};
		
		//Components.
		[HideInInspector]
		public Animator animator;
		[HideInInspector]
		public Rigidbody rb;
		[HideInInspector]
		public UnityEngine.AI.NavMeshAgent navMeshAgent;
		
		//Objects.
		private GameObject axe;
		private GameObject hammer;
		private GameObject fishingpole;
		private GameObject shovel;
		private GameObject box;
		private GameObject food;
		private GameObject drink;
		private GameObject saw;
		private GameObject pickaxe;
		private GameObject sickle;
		private GameObject rake;
		private GameObject chair;
		private GameObject ladder;
		private GameObject lumber;
		private GameObject pushpull;
		private GameObject sphere;
		private GameObject cart;
		private GameObject paintbrush;
		private GameObject spear;
		
		//Actions.
		public Transform target;
		float rotationSpeed = 10f;
		public float runSpeed = 8f;
		public float walkSpeed = 4f;
		public float spearfishingSpeed = 1.25f;
		public float crawlSpeed = 1f;
		Vector3 inputVec;
		Vector3 newVelocity;
		[HideInInspector]
		public bool isMoving;
		[HideInInspector]
		public bool isPaused;
		public bool useMeshNav;
		bool isAiming;
		bool isRunning;
		bool inputAiming;
		float pushpullTime = 0f;
		[HideInInspector]
		public bool isGrounded;
		[HideInInspector]
		public bool isSpearfishing;
		
		//Inputs.
		float inputHorizontal = 0f;
		float inputVertical = 0f;
		float inputHorizontal2 = 0f;
		float inputVertical2 = 0f;
		bool inputRun;
		
		public CharacterState charState;
		
		void Awake(){
			animator = this.GetComponent<Animator>();
			axe = GameObject.Find("Axe");
			hammer = GameObject.Find("Hammer");
			fishingpole = GameObject.Find("FishingPole");
			shovel = GameObject.Find("Shovel");
			box = GameObject.Find("Carry");
			food = GameObject.Find("Food");
			drink = GameObject.Find("Drink");
			saw = GameObject.Find("Saw");
			pickaxe = GameObject.Find("PickAxe");
			sickle = GameObject.Find("Sickle");
			rake = GameObject.Find("Rake");
			chair = GameObject.Find("Chair");
			ladder = GameObject.Find("Ladder");
			lumber = GameObject.Find("Lumber");
			pushpull = GameObject.Find("PushPull");
			sphere = GameObject.Find("Sphere");
			cart = GameObject.Find("Cart");
			paintbrush = GameObject.Find("Paintbrush");
			spear = GameObject.Find("Spear");
			rb = GetComponent<Rigidbody>();
		}
		
		void Start(){
			StartCoroutine(_ShowItem("none", 0f));
			charState = CharacterState.Idle;
		}
		
		//Input abstraction for easier asset updates using outside control schemes.
		void Inputs(){
			inputHorizontal = Input.GetAxisRaw("Horizontal");
			inputVertical = -(Input.GetAxisRaw("Vertical"));
			inputHorizontal2 = Input.GetAxisRaw("Horizontal2");
			inputVertical2 = -(Input.GetAxisRaw("Vertical2"));
			inputAiming = Input.GetButtonDown("Aiming");
			inputRun = Input.GetButton("Fire3");
		}
		
		void Update(){
			Inputs();
			if(charState != CharacterState.PushPull){
				CameraRelativeInput();
			} 
			else{
				PushPull();
			}
			if(Input.GetKey(KeyCode.R)){
				this.gameObject.transform.position = new Vector3(0,0,0);
			}
			//Aiming switch.
			if(inputAiming){
				if(!isAiming){
					isAiming = true;
				}
				else{
					isAiming = false;
				}
			}
			//Slow time.
			if(Input.GetKeyDown(KeyCode.T)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0.15f;
				}
			}
			//Pause.
			if(Input.GetKeyDown(KeyCode.P)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0f;
				}
			}
		}
		
		void FixedUpdate(){
			CheckForGrounded();
			if(!isPaused){
				if(charState == CharacterState.Climb || charState == CharacterState.PushPull || charState == CharacterState.Laydown || charState == CharacterState.Use){
					animator.applyRootMotion = true;
					isMoving = false;
					rb.useGravity = false;
				} 
				else{
					animator.applyRootMotion = false;
					rb.useGravity = true;
				}
			}
		}
		
		void LateUpdate(){
			//Get local velocity of charcter and update animator with values.
			float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
			float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
			if(charState != CharacterState.PushPull){
				animator.SetFloat("VelocityX", velocityXel / runSpeed);
				animator.SetFloat("VelocityY", velocityZel / runSpeed);
			}
			//Running.
			if(inputRun){
				//Don't run with Box, Cart, Lumber, etc.
				if(charState != CharacterState.Box && charState != CharacterState.Cart && charState != CharacterState.Overhead && charState != CharacterState.PushPull && charState != CharacterState.Lumber && charState != CharacterState.Use){
					animator.SetBool("Running", true);
					isRunning = true;
					isAiming = false;
				}
			}
			else{
				animator.SetBool("Running", false);
				isRunning = false;
			}
			//If using Navmesh nagivation, update values.
			if(useMeshNav){
				if(navMeshAgent.velocity.sqrMagnitude > 0){
					animator.SetBool("Moving", true);
					animator.SetFloat("VelocityY", navMeshAgent.velocity.magnitude);
				}
			}
			//Crafter is moving.
			if(UpdateMovement() > 0){
				isMoving = true;
				animator.SetBool("Moving", true);
			}
			else{
				isMoving = false;
				animator.SetBool("Moving", false);
			}
		}
		
		//Moves the character.
		float UpdateMovement(){
			Vector3 motion = inputVec;
			//reduce input for diagonal movement.
			if(motion.magnitude > 1){
				motion.Normalize();
			}
			if(!isPaused && !useMeshNav && charState != CharacterState.PushPull && charState != CharacterState.Laydown && charState != CharacterState.Crawl){
				//set speed by walking / running.
				if(isRunning){
					newVelocity = motion * runSpeed;
				} 
				else if(isSpearfishing){
					newVelocity = motion * spearfishingSpeed;
				}
				else{
					newVelocity = motion * walkSpeed;
				}
			} 
			else if(charState == CharacterState.Crawl){
				newVelocity = motion * crawlSpeed;
			}
			if(isAiming){
				//make character point at target.
				Quaternion targetRotation;
				Vector3 targetPos = target.transform.position;
				targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
				transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed * 10f);
			}
			else{
				if(!isPaused && charState != CharacterState.PushPull && charState != CharacterState.Laydown && charState != CharacterState.Use){
					RotateTowardsMovementDir();
				}
			}
			//if character is falling use momentum.
			newVelocity.y = rb.velocity.y;
			rb.velocity = newVelocity;
			//return a movement value for the animator.
			return inputVec.magnitude;
		}
		
		//checks if character is within a certain distance from the ground, and markes it IsGrounded.
		void CheckForGrounded(){
			float distanceToGround;
			float threshold = .45f;
			RaycastHit hit;
			Vector3 offset = new Vector3(0, 0.4f, 0);
			if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f)){
				distanceToGround = hit.distance;
				if(distanceToGround < threshold){
					isGrounded = true;
				}
				else{
					isGrounded = false;
				}
			}
		}
		
		//face character along input direction.
		void RotateTowardsMovementDir(){
			if(inputVec != Vector3.zero){
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
			}
		}
		
		//All movement is based off camera facing.
		void CameraRelativeInput(){
			//Camera relative movement
			Transform cameraTransform = Camera.main.transform;
			//Forward vector relative to the camera along the x-z plane.
			Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			//Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			//directional inputs.
			inputVec = inputHorizontal * right + -inputVertical * forward;
		}
		
		void PushPull(){
			if(inputHorizontal == 0 && inputVertical == 0){
				pushpullTime = 0;
			}
			if(inputHorizontal != 0){
				inputVertical = 0;
			}
			if(inputVertical != 0){
				inputHorizontal = 0;
			}
			pushpullTime += 0.5f * Time.deltaTime;
			float h = Mathf.Lerp(0, inputHorizontal, pushpullTime);
			float v = Mathf.Lerp(0, inputVertical, pushpullTime);
			animator.SetFloat("VelocityX", h);
			animator.SetFloat("VelocityY", v);
		}
		
		void Aiming(){
			for(int i = 0; i < Input.GetJoystickNames().Length; i++){
				//if the right joystick is moved, use that for facing.
				if(Mathf.Abs(inputHorizontal2) > 0.1 || Mathf.Abs(inputVertical2) < -0.1){
					Vector3 joyDirection = new Vector3(inputHorizontal2, 0, -inputVertical2);
					joyDirection = joyDirection.normalized;
					Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
					transform.rotation = joyRotation;
				}
			}
			//no joysticks, use mouse aim.
			if(Input.GetJoystickNames().Length == 0){
				Plane characterPlane = new Plane(Vector3.up, transform.position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 mousePosition = new Vector3(0, 0, 0);
				float hitdist = 0.0f;
				if(characterPlane.Raycast(ray, out hitdist)){
					mousePosition = ray.GetPoint(hitdist);
				}
				mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
				Vector3 relativePos = transform.position - mousePosition;
				Quaternion rotation = Quaternion.LookRotation(-relativePos);
				transform.rotation = rotation;
			}
		}
		
		public IEnumerator _MovePause(float pauseTime){
			isPaused = true;
			animator.applyRootMotion = true;
			yield return new WaitForSeconds(pauseTime);
			isPaused = false;
			animator.applyRootMotion = false;
		}
		
		public IEnumerator _ChangeCharacterState(float waitTime, CharacterState state){
			yield return new WaitForSeconds(waitTime);
			charState = state;
		}
		
		public IEnumerator _ShowItem(string item, float waittime){
			yield return new WaitForSeconds (waittime);
			if(item == "none"){
				axe.SetActive(false);
				hammer.SetActive(false);
				fishingpole.SetActive(false);
				shovel.SetActive(false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "axe"){
				axe.SetActive(true);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "hammer"){
				axe.SetActive(false);
				hammer.SetActive (true);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "fishingpole"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (true);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "shovel"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (true);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "box"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(true);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "food"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(true);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "drink"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(true);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "saw"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(true);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "pickaxe"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(true);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "sickle"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(true);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "rake"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(true);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "chair"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(true);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "chaireat"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(true);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(true);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "chairdrink"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(true);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(true);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "ladder"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(true);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "pushpull"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(true);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "lumber"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(true);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "sphere"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(true);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "cart"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(true);
				paintbrush.SetActive(false);
				spear.SetActive(false);
			}
			else if(item == "paintbrush"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(true);
				spear.SetActive(false);
			}
			else if(item == "spear"){
				axe.SetActive(false);
				hammer.SetActive (false);
				fishingpole.SetActive (false);
				shovel.SetActive (false);
				box.SetActive(false);
				food.SetActive(false);
				drink.SetActive(false);
				saw.SetActive(false);
				pickaxe.SetActive(false);
				sickle.SetActive(false);
				rake.SetActive(false);
				chair.SetActive(false);
				ladder.SetActive(false);
				pushpull.SetActive(false);
				lumber.SetActive(false);
				sphere.SetActive(false);
				cart.SetActive(false);
				paintbrush.SetActive(false);
				spear.SetActive(true);
			}
			yield return null;
		}
	}
}