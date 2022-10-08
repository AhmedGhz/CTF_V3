/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TurretSystem_Health))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class TurretSystem_Turret : MonoBehaviour
{
	#region Variables
	[Header("General Settings")]
	[Tooltip("The amount of damage the turret deals (if you wish to change the max, just edit the Range).")]
	[Range(0,200)]public float damageAmount = 3; //amount of damage the turret deals (if you wish to change the max, just edit the "Range")
	[Tooltip("Time betweent bullets.")]
	[Range(0,30)]public float rateOfFire = 1; //time betweent bullets
	[Tooltip("Range the bullet flies in case of raycastAttack, and range of turret sight.")]
	[Range(0,200)]public float shotRange = 10; //range the bullet flies in case of raycastAttack, and range of turret sight
	[Tooltip("Turret tracking/turning speed.")]
	[Range(0,50)]public float turnSpeed = 5; //turret tracking/turning speed
	[Tooltip("Amount of targetting error (eg. high precision = 0, low precision = 2).")]
	[Range(0,100)]public float errorAmount = 0.5f; //amount of targetting error (eg. high precision = 0, low precision = 2)
	[Tooltip("Check if you want the projectile turret to estimate, and aim ahead of the moving target.")]
	public bool aimAhead = true;
	[Tooltip("Displays the calculated damage per second in editor, making it easier for you to balance the power of turrets in your game.")]
	public float damagePerSecond;

	[Header("Ammunition Settings")]
	public bool useAmmo = true;
	public float ammo = 1000;
	public float clipSize = 100;
	public float reloadTime;

	[Header("Prefabs")]
	[Tooltip("Projectile prefab, in case its a projectile turret.")]
	public GameObject projectile; //projectile prefab, in case of projectile bullet turret
	[Tooltip("Spark created on bullet impact.")]
	public GameObject hitSpark; //bullet hit spark
	[Tooltip("Flash from the turret barrel.")]
	public GameObject muzzleFlash; 

	[Header("Turret Parts")]
	[Tooltip("Turret pivot(stall) that will rotate only on the Y axis. Purely visual.")]
	public Transform turretPivot; //turret pivot(stall) that will rotate only on the Y axis
	[Tooltip("Turret head that will rotate on X axis, if Allow Tilting is checked, allowing the turret to aim under and above it.")]
	public Transform turretHead; //turret head that will rotate on X axis, if "Allow Tilting" is checked
	[Tooltip("Muzzle positions to fire bullets, and muzzle flashes from.")]
	public Transform[] muzzlePositions; //muzzle positions to fire bullets, and muzzle flashes, from
	[Tooltip("Line renderers for laser and shock turrets.")]
	public LineRenderer[] lines; //line renderers for laser and shock turrets
	[Tooltip("Particle systems used as weapons. (eg. flamethrower)")]
	public ParticleSystem[] particles; //particle systems used as weapons (eg. flamethrower)
	[Tooltip("Check this if the particle will be purely visual. Such as bullet casing shooting off the side.")]
	public bool visualParticle = false;

	[Header("Turret Types")]
	[Tooltip("Bullet arrives instantly.")]
	public bool isRaycast; //is the turret an instantanious weapon? (bullet arrives instantly)
	[Tooltip("Fires a physical rigidbody projectile")]
	public bool isProjectile; //is the turret a physical projectile weapon?
	[Tooltip("Saw-like turret.")]
	public bool isMelee; //is the turret a melee type?
	[Tooltip("Shock/Lighting type turret. Uses line renderers.")]
	public bool isLine; //is the turret a shock/lightning type?
	[Tooltip("Laser type turret. Uses line renderers.")]
	public bool isLineContinuous; //is the turret a continuous laser type?
	[Tooltip("Particle turret, such as flamethrowers. Uses particle systems.")]
	public bool isParticle;

	[Header("Player Control")]
	[Tooltip("Check if the turret is controlled by the player, from a top-down view.")]
	public bool topDownPlayerControl; //is the turret controled by the player?
	[Tooltip("Check if the turret is controlled by the player, from a first person perspective.")]
	public bool firstPersonPlayerControl; //is the turret controled by the player?
	[Tooltip("The player camera.")]
	public Camera playerCamera;
	[Tooltip("If you are making a first person shooter game using the turret script for the weapon, you might wanna check this. It will fire from camera, but muzzle will still use muzzle positions," +
		"and everything else works the same.")]
	public bool useCameraForRaycastAttack = false;
	[Tooltip("Button used to fire. Default is Fire1")]
	public string fireButton = "Fire1";
	[Tooltip("Button used to aim. Default is Fire2")]
	public string aimButton = "Fire2";
	public bool fireOnPress = true;
	public bool fireOnRelease;
	public bool fireOnHold;
	[Tooltip("Automatically fire on sight.")]
	public bool fireOnSight;
	[Tooltip("Can you aim before its time to fire again? Such as bows. But you can handle aiming with animation triggers too.")]
	public bool canAimBeforeFireTime = true;
	[Tooltip("Coordinates of the new position to move to when aiming.")]
	public Vector3 aimPosition;
	[Tooltip("What object are you moving/aiming with.")]
	public Transform aimObject;
	public float aimSpeed = 0.3f;
	[Tooltip("How much to reduce aim error. The aimError is reduced to this number.")]
	public float aimErrorReduction = 0.05f;

	[Header("Tower Defense")]
	[Tooltip("If you're making a tower defense game, throw the turret upgrade here, then just call the Upgrade() function")]
	public GameObject turretUpgrade;
	[Tooltip("Upgrade effect to spawn when the turret upgrades. Optional.")]
	public GameObject upgradeEffect;
	public int turretUpgradeCost = 100;
	public int turretSellValue = 50;

	[Header("Misc")]
	[Tooltip("If checked, turret can tilt forward and back, allowing it to aim up and down.")]
	public bool allowTiltingForward; //can the turret tilt up and down?
	[Tooltip("If set to something other than 0, the turret will move randomly when it doesn't have a target. This is how often it will move. Less is more.")]
	public float noTargetRandomMovementInterval; //should the turret move randomly if it doesn't have a target
	[Tooltip("Minimum angle limit.")]
	public float minVerticalAngle = -30;
	[Tooltip("Maximum angle limit.")]
	public float maxVerticalAngle = 70;
	[Tooltip("Minimum horizontal angle limit.")]
	public float minHorizontalAngle = 0;
	[Tooltip("Maximum horizontal angle limit.")]
	public float maxHorizontalAngle = 0;
	[Tooltip("If checked, turret will ignore all obstacles between it and the target, and fire regardless.")]
	public bool ignoreRaycast; //check if you want the turret to ignore when there's an object between it, and the target
	[Tooltip("Check if you want to shoot a secondary target tag.")]
	public bool shootSecondaryToo; //will the turret shoot air units too? (using secondaryEnemyTag)

	[Tooltip("Whatever layer can be hit by raycast should be on this mask. Triggers generaly shouldn't be on this mask (eg. target sensor trigger collider).")]
	public LayerMask obstaclesLayerMask; //whatever layer can be hit by raycast should be on this mask. Triggers generaly shouldn't be on this mask (eg. target sensor trigger collider)
	[Tooltip("Enemy tag (eg. ground enemy).")]
	public string enemyTag; //enemy tag (eg. ground enemy)
	[Tooltip("Air enemy tag (eg. used for SAM turrets, which only shoot air units).")]
	public string secondaryEnemyTag; //air enemy tag (eg. used for SAM turrets, which only shoot air units)
	[Tooltip("If this is checked the script will wait for the fire sound to finish playing before playing it again, to avoid cutting off the audio.")]
	public bool checkIfAudioEnded; //If this is checked the script will wait for the fire sound to finish playing before playing it again, to avoid cutting off the audio
	[Tooltip("If the turret is mobile, check this. Because the turret is mobile, the pool won't be parented to the turret, to avoid weird position and direction changes for projectiles")]
	public bool mobileTurret;
	[Tooltip("Decides how often the code in OnTriggerStay will be run. Higher values mean less ms for Physics.ProcessReports, but bigger delay in switching targets. " +
	         "If you're not experiencing fps issues, no need to change this.")]
	[Range(1,100)]public int OnTriggerStayOptimization = 1; 
	[Tooltip("Script name of the hit object to send a message to. SendMessage is slower than GetComponent, so you're better off with override, or just adding your code to it. " +
	         "Search TakeDamage, to find the places damage is applied.")]
	public string customScriptToSendMsgTo;

	[Header("Animations")]
	[Tooltip("All the recoil animators you want to send the 'Shoot' trigger to.")]
	public Animator[] recoil;
	[Tooltip("All the reload animators you want to send the 'Reload' trigger to.")]
	public Animator[] reload;
	public Animator[] fireOnPressAnim;
	public Animator[] fireOnReleaseAnim;
	public Animator[] fireOnHoldAnim;
	public Animator[] aimAnim;
	[Tooltip("All the onStart animators you want to send the 'Start' trigger to. The trigger will be sent when a target enters the sensor collider.")]
	public Animator[] onStart;
	[Tooltip("All the onStart animators you want to send the 'Shutdown' trigger to. The trigger will be sent when a target leaves the sensor collider.")]
	public Animator[] onShutdown;

	[Tooltip("The trigger you're calling in the animator for shooting (recoil).")]
	public string recoilTrigger = "Shoot";
	[Tooltip("The trigger you're calling in the animator for reload.")]
	public string reloadTrigger = "Reload";
	public string fireOnPressTrigger = "FirePress";
	public string fireOnReleaseTrigger = "FireRelease";
	public string fireOnHoldTrigger = "FireHold";
	public string aimAnimTrigger = "Aim";
	public string onStartTrigger = "Start";
	public string onShutdownTrigger = "Shutdown";

	[Header("Debug")]
	public Transform target; 
	private	bool canShoot; //a check whether the turret is ready to shoot
	private Vector3 rayOrigin; 
	private float nextFireTime;
	private float firePauseTime;
	private Quaternion desiredPivotRotation;
	private Quaternion desiredHeadRotation;
	private float aimError; //calculated aim error, based on errorAmount
	private Vector3 targetPos; //position of the target
	private AudioSource fireSound; 
	private RaycastHit hit; 
	private GameObject permaHitSpark; //permanent hit spark, used for lasers and melee weapons, because of their continuous attack nature, so we dont spawn a hit spark every frame
	private Vector3 aimPoint;
	private Vector3 aimDir;
	private Vector3 topDownAimPoint; //for the top-down cam
	private float x = 0; //for the fps cam
	private float y = 0; //for the fps cam
	private int randBarrel;
	private float fullClipSize;
	private TurretSystem_ObjectPooler projectilePool;
	private TurretSystem_ObjectPooler muzzlePool;
	private TurretSystem_ObjectPooler sparkPool;
	private GameObject pool;
	private bool reloading; //a reload check for particle bullet casings. checking with nextFireTime apparently isn't working. :P
	private Vector3 bulletOrigin;
	private Vector3 bulletDirection;
	private float bulletSpeed;
	private float idleRotation;
	private Vector3 estPos;
	private Vector3 startPosition;
	private float aimErrorKeeper;

	//Debug
	//[SerializeField]GameObject hitGOO;
	#endregion

	#region StartUpdate
	void Start()
	{
		Pooling(); //make pools of bullets, muzzle flashes, and sparks.

		nextFireTime = Time.time + rateOfFire; //set fire time
		foreach(Transform coll in transform) //get the collider used for detecting the target
		{
			if(coll.gameObject.layer == LayerMask.NameToLayer("RangeCollider"))
				coll.GetComponent<SphereCollider>().radius = 0.9f * shotRange;
		}
		fireSound = GetComponent<AudioSource>(); //get the audiosource used for the firing sound
		if(playerCamera == null)
		{
			if(topDownPlayerControl || firstPersonPlayerControl)
				playerCamera = Camera.main;
		}

		fullClipSize = clipSize; //store the clip size capacity
		CalculateDamagePerSecond();
		if(noTargetRandomMovementInterval != 0)
		{
			StartCoroutine("SetRandomRotation");
		}
		if(projectile)
			bulletSpeed = projectile.GetComponent<TurretSystem_Projectile>().speed;
		if(aimObject)
			startPosition = aimObject.localPosition;
		aimErrorKeeper = aimError;
	}

	void Update()
	{
		if(target && !topDownPlayerControl && !firstPersonPlayerControl) //if we have a target
		{
			targetPos = target.position;
			CalculateLookDirection(); //calculate the aim position
			LookAtEnemy(); //face it
			if(Time.time >= nextFireTime) //check if its time to fire
			{
				reloading = false;
				if(!ignoreRaycast && canShoot && !topDownPlayerControl)
				{
					FireProjectile(); //fire
				}
				else if(ignoreRaycast && !topDownPlayerControl) 
				{
					FireProjectile(); //fire
				}
			}

		}
		else
		{
			if(isLineContinuous && !topDownPlayerControl && !firstPersonPlayerControl)
				foreach(LineRenderer line in lines)
				{
					line.SetPosition(1,turretHead.position);
					line.enabled = false;
					Destroy(permaHitSpark);
				}
			if(isParticle && !topDownPlayerControl && !firstPersonPlayerControl)
				foreach(ParticleSystem particle in particles)
					particle.enableEmission = false;
		}
		////Player Control Specific///
		if(topDownPlayerControl)
			TopDownPlayerControl();
		if(firstPersonPlayerControl)
			FirstPersonPlayerControl();

		if(!target && !topDownPlayerControl && !firstPersonPlayerControl)
			IdleState();
	}
	#endregion

	#region LookAimFire

	////Player Control Specific///
	void TopDownPlayerControl()
	{
		Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		topDownAimPoint = new Vector3(0,0,0);

		if(Physics.Raycast(ray, out hit))
			topDownAimPoint = hit.point;

		aimPoint = new Vector3(topDownAimPoint.x + aimError, topDownAimPoint.y + aimError, topDownAimPoint.z + aimError);

		CalculateLookDirection();
		LookAtEnemy();

		PlayerFireInput();
	}

	void FirstPersonPlayerControl()
	{
		x+= Input.GetAxis ("Mouse X") * turnSpeed;
		if(allowTiltingForward)
			y-= Input.GetAxis ("Mouse Y") * turnSpeed;

		y = ClampAngle(y,minVerticalAngle,maxVerticalAngle);
		if(minHorizontalAngle != 0 || maxHorizontalAngle != 0)
			x = ClampAngle(x,minHorizontalAngle,maxHorizontalAngle);
		Quaternion rotation = Quaternion.Euler(y,x,0);
		Vector3 direction = (rotation * Vector3.forward * 100);
		aimPoint = new Vector3(direction.x + aimError, direction.y + aimError, direction.z + aimError);
		aimDir = (aimPoint - playerCamera.transform.position).normalized;

		CalculateLookDirection();
		LookAtEnemy();
		
		PlayerFireInput();
	}

	void PlayerFireInput() //check which option is selected
	{
		if(fireOnPress) 
		{
			if(fireOnPressAnim.Length > 0) //if theres any animations for firing, play them
				foreach(Animator anim in fireOnPressAnim)
			{
				anim.SetTrigger(fireOnPressTrigger);
			}

			if(Input.GetButtonDown(fireButton) && Time.time >= nextFireTime) 
				FireProjectile();
			else
				TurnOffLaserAndParticles();
		}
		if(fireOnRelease)
		{
			if(fireOnReleaseAnim.Length > 0)
				foreach(Animator anim in fireOnPressAnim)
			{
				anim.SetTrigger(fireOnReleaseTrigger);
			}
			if(Input.GetButtonUp(fireButton) && Time.time >= nextFireTime)
				FireProjectile();
			else
				TurnOffLaserAndParticles();
		}
		if(fireOnHold)
		{
			if(fireOnHoldAnim.Length > 0)
				foreach(Animator anim in fireOnPressAnim)
			{
				anim.SetTrigger(fireOnHoldTrigger);
			}
			if(Input.GetButton(fireButton) && Time.time >= nextFireTime)
				FireProjectile();
			else
				TurnOffLaserAndParticles();
		}
		if(fireOnSight) 
		{
			if(fireOnPressAnim.Length > 0)
				foreach(Animator anim in fireOnPressAnim)
			{
				anim.SetTrigger(fireOnPressTrigger);
			}

			CheckIfCanShoot();
			if(canShoot && Time.time >= nextFireTime) 
				FireProjectile();
			else
				TurnOffLaserAndParticles();
		}
		if(Input.GetButton(aimButton))
		{
			if(canAimBeforeFireTime)
			{
				if(aimAnim.Length > 0)
					foreach(Animator anim in aimAnim)
				{
					anim.SetTrigger(aimAnimTrigger);
				}
				if(aimObject)
				{
					aimObject.localPosition = Vector3.Lerp(aimObject.localPosition, aimPosition, aimSpeed);
					aimError = aimErrorReduction;
				}
			}
			else if(Time.time >= nextFireTime)
			{
				if(aimAnim.Length > 0)
					foreach(Animator anim in aimAnim)
				{
					anim.SetTrigger(aimAnimTrigger);
				}
				if(aimObject)
				{
					if(aimPosition != Vector3.zero)
						aimObject.localPosition = Vector3.Lerp(aimObject.localPosition, aimPosition, aimSpeed);
					aimError = aimErrorReduction;
				}
			}
		}
		else
		{
			if(aimObject)
			{
				aimObject.localPosition = Vector3.Lerp(aimObject.localPosition, startPosition, aimSpeed);
				aimError = aimErrorKeeper;
			}
		}
	}

	void TurnOffLaserAndParticles()
	{
		if(isParticle && particles.Length > 0)
		{
			foreach(ParticleSystem particle in particles)
				particle.enableEmission = false;
		}
		if(isLineContinuous && lines.Length > 0)
		{
			foreach(LineRenderer line in lines)
			{
				line.SetPosition(1,turretHead.position);
				line.enabled = false;
				Destroy(permaHitSpark);
			}
		}
	}

	//calculate angle limits, and return the new clamped angle values
	float ClampAngle(float angle, float min, float max)
	{
		if(angle < -360f)
			angle += 360f;
		if(angle > 360f)
			angle -= 360f;
		return Mathf.Clamp(angle,min,max);
	}
	//the same but different for AI, because its different from the way player controls rotation works
	float ClampAngleAI(float angle) 
	{
		if(angle < 360f+minVerticalAngle && angle > 270)
			angle = 360f+minVerticalAngle;
		else if(angle > maxVerticalAngle && angle < 360f+minVerticalAngle)
			angle = maxVerticalAngle;
		return angle;
	}

	float ClampAngleAIHorizontal(float angle) 
	{
		if(angle < 360f+minHorizontalAngle && angle > 270)
			angle = 360f+minHorizontalAngle;
		else if(angle > maxHorizontalAngle && angle < 360f+minHorizontalAngle)
			angle = maxHorizontalAngle;
		return angle;
	}

	void CalculateLookDirection()
	{
		////Player Control Specific////
		if(!topDownPlayerControl && !firstPersonPlayerControl)
		{
			if(aimAhead && projectile)
			{
				AimAhead(); //get estimated pos
				aimPoint = new Vector3(estPos.x + aimError, estPos.y + aimError, estPos.z + aimError); //estimate the aim-ahead position with set error amount
			}
			else
			{
				aimPoint = new Vector3(targetPos.x + aimError, targetPos.y + aimError, targetPos.z + aimError); //calculate aim position with set error amount
			}
		}

		////General////
		if(!firstPersonPlayerControl)
			aimDir = (aimPoint - turretHead.position).normalized; //get the direction

		desiredPivotRotation = Quaternion.LookRotation(aimDir); //create the pivot look rotation
		desiredHeadRotation = desiredPivotRotation; //create the head rocation
		if(minHorizontalAngle != 0 || maxHorizontalAngle != 0)
		{
			Vector3 eulerRot = desiredPivotRotation.eulerAngles;
			eulerRot = new Vector3(0, ClampAngleAIHorizontal(eulerRot.y),0);
			desiredPivotRotation = Quaternion.Euler(eulerRot);
		}
		else
		{
			desiredPivotRotation.x = 0; //neutralize the rotation on X
			desiredPivotRotation.z = 0; //and Z
		}

		if(minVerticalAngle != 0 || maxVerticalAngle != 0) //if we have angle limits set, calculate the clamped value of X
		{
			Vector3 eulerRot = desiredHeadRotation.eulerAngles; //get the rotation in euler angles
			eulerRot = new Vector3(ClampAngleAI(eulerRot.x),eulerRot.y,0); //clamp it
			desiredHeadRotation = Quaternion.Euler(eulerRot); //set it as the new desired head rotation
		}
		if(minHorizontalAngle != 0 || maxHorizontalAngle != 0)
		{
			Vector3 eulerRot = desiredHeadRotation.eulerAngles; //get the rotation in euler angles
			eulerRot = new Vector3(eulerRot.x,ClampAngleAIHorizontal(eulerRot.y),0); //clamp it
			desiredHeadRotation = Quaternion.Euler(eulerRot); //set it as the new desired head rotation
		}

		if(!allowTiltingForward) //if tilting isn't allowed, neutralize the X and Z rotations
		{
			desiredHeadRotation.x = 0;
			desiredHeadRotation.z = 0;
		}
	}

	public void LookAtEnemy()
	{
		if(turretPivot) //if we assigned a turret pivot (for rotation on Y axis)
			turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, desiredPivotRotation, Time.deltaTime * turnSpeed); //turn the pivot 
		if(turretHead) //if we assigned a turret head (for rotation on X axis)
			turretHead.rotation = Quaternion.Lerp(turretHead.rotation, desiredHeadRotation, Time.deltaTime * turnSpeed);; //and head, based on calculated rotations

		CheckIfCanShoot();
	}

	void CheckIfCanShoot()
	{
		rayOrigin = turretHead.position; 
		if(Physics.Raycast(rayOrigin, turretHead.forward, out hit, shotRange, obstaclesLayerMask)) //cast a ray from head outward with set range
		{
			if(hit.collider.transform.root.gameObject.tag == enemyTag) //if the ray hits an enemy the turret can shoot (this is ignored if ignoreRaycast is checked)
				canShoot = true;
			else if(hit.collider.transform.root.gameObject.tag == secondaryEnemyTag && shootSecondaryToo) //if the ray hits an enemy the turret can shoot (this is ignored if ignoreRaycast is checked)
				canShoot = true;
			else
				canShoot = false;
			
			Debug.DrawLine(rayOrigin, hit.point); //just a debug check to see what the turret is looking at
		}	
		else
			canShoot = false;
		if(target == null)
			canShoot = false;
	}

	void FireProjectile()
	{
		if(clipSize <= 0) //if we have no ammo, do nothing
			return;
		if(!checkIfAudioEnded) //if we dont want to check did the firing sound end, just play it
			fireSound.Play();
		else
			if(checkIfAudioEnded && !fireSound.isPlaying) //otherwise check and then play when it finished
				fireSound.Play();
		nextFireTime = Time.time + rateOfFire; //determine the time to fire again
		aimError = Random.Range(-errorAmount, errorAmount); //determine the error amount to offset the aim again

		if(isRaycast) //if we selected raycast turret type itll go to that attack type, and same of the other choices
		{
			RaycastAttack();
		}
		if(isProjectile)
		{
			ProjectileAttack();
		}
		if(isMelee)
		{
			MeleeAttack();
		}
		if(isLine)
		{
			StartCoroutine(ShockAttack());
		}
		if(isLineContinuous)
		{
			LaserAttack();
		}
		if(isParticle)
		{
			ParticleAttack();
		}
		if(recoil.Length > 0)
			recoil[randBarrel].SetTrigger(recoilTrigger); //play the shoot animation
	}
	#endregion

	#region Attacks

	public virtual void TriggerDamage(GameObject hitGO, float damageAmount) //override this if you have your own health script
	{
		hitGO.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount);
		if(customScriptToSendMsgTo != "")
			hitGO.SendMessage(customScriptToSendMsgTo);
	}

	void RaycastAttack()
	{
		RaycastHit hit;
		randBarrel = Random.Range(0,muzzlePositions.Length); //pick a random barrel to fire from

//		if(muzzleFlash) //if there's a muzzle prefab assigned, create one
//		Instantiate(muzzleFlash, muzzlePositions[randBarrel].position, muzzlePositions[randBarrel].rotation);
		if(muzzleFlash)
		{
			GameObject muz = muzzlePool.GetPooledObject();
				
			if(muz == null) 
				return;
			muz.transform.position = muzzlePositions[randBarrel].position;
			muz.transform.rotation = muzzlePositions[randBarrel].rotation;
			muz.SetActive(true);
		}
		UpdateBulletOrigin();
		if(Physics.Raycast(bulletOrigin, bulletDirection, out hit, shotRange, obstaclesLayerMask)) //casting a ray, and ignoring the RangeCollider
		{
			GameObject hitGO = hit.collider.transform.root.gameObject;
			if(hitGO.tag == enemyTag) //if the hit object tag matches our enemy tag, deal damage to it
			{
				TriggerDamage(hitGO, damageAmount);
			}
			if(hitGO.tag == secondaryEnemyTag && shootSecondaryToo)
			{
				TriggerDamage(hitGO, damageAmount);
			}
			if(hit.collider && hitSpark) //if we assigned a hitSpark, create one at the hit point
			{
				//Instantiate(hitSpark, hit.point, Quaternion.identity);
				if(hitSpark)
				{
					GameObject sprk = sparkPool.GetPooledObject();
					
					if(sprk == null) 
						return;
					sprk.transform.position = hit.point;
					sprk.SetActive(true);
				}
			}
		}
		if(useAmmo) //you'll see this a few times in the script. if we checked that we're using ammo, spend it
		{
			clipSize-=1; //deduct one bullet per shot
			if(clipSize <= 0) //if theres no more bullets in the clip
				Reload(); //reload
		}
	}
	void ProjectileAttack()
	{
		randBarrel = Random.Range(0,muzzlePositions.Length); //pick a random barrel to fire from

		GameObject proj = projectilePool.GetPooledObject();
		if(proj == null) 
			return;
		proj.transform.position = muzzlePositions[randBarrel].position;
		proj.transform.rotation = muzzlePositions[randBarrel].rotation;
		proj.SetActive(true);
//		GameObject proj = Instantiate(projectile, muzzlePositions[randBarrel].position, muzzlePositions[randBarrel].rotation) as GameObject; //instantiate the projectile
		TurretSystem_Projectile projScript = proj.GetComponent<TurretSystem_Projectile>(); //grab the projectile script

		if(projScript.isMissile == true) //and check whether its a missile projectile
			projScript.missileTarget = target; //and if it is, assign the target to it

//		if(muzzleFlash) //and create a muzzle flash prefab if its assigned
//			Instantiate(muzzleFlash, muzzlePositions[randBarrel].position, muzzlePositions[randBarrel].rotation);
		if(muzzleFlash)
		{
			GameObject muz = muzzlePool.GetPooledObject();

			if(muz == null) 
				return;
			muz.transform.position = muzzlePositions[randBarrel].position;
			muz.transform.rotation = muzzlePositions[randBarrel].rotation;
			muz.SetActive(true);
		}
		if(useAmmo)
		{
			clipSize-=1;
			if(clipSize <= 0)
				Reload();
		}
	}
	void MeleeAttack()
	{
//		if(hitSpark) //if a hit spark is assigned
//			Instantiate(hitSpark, hit.point, Quaternion.identity); //create a hitspark at the hit point (allow tilting and ignore raycast should be selected)
		if(hitSpark)
		{
			GameObject sprk = sparkPool.GetPooledObject();
			
			if(sprk == null) 
				return;
			sprk.transform.position = hit.point;
			sprk.SetActive(true);
		}
		TriggerDamage(target.root.gameObject, damageAmount * Time.deltaTime);
		//target.root.gameObject.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount * Time.deltaTime); //deal damage per time

		if(useAmmo)
		{
			clipSize-=1;
			if(clipSize <= 0)
				Reload();
		}
	}
	IEnumerator ShockAttack()
	{
		foreach(LineRenderer line in lines)
			line.enabled = true;
		foreach(LineRenderer line in lines) //for each line of the lightning type attack set the origin from head to target position
		{
			line.SetPosition(0, turretHead.position); 
			////Player Control Specific////
			if(!topDownPlayerControl && !firstPersonPlayerControl)
				line.SetPosition(1, target.position);
			else ////General////
				line.SetPosition(1, hit.point);
//			if(hitSpark) //and if there is a hit spark assigned, create it
//				Instantiate(hitSpark, hit.point, turretHead.rotation);
			if(hitSpark)
			{	
				GameObject sprk = sparkPool.GetPooledObject();
				
				if(sprk == null) 
					yield break;
				sprk.transform.position = hit.point;
				sprk.SetActive(true);
			}
		}
		////General////
		if(!topDownPlayerControl && !firstPersonPlayerControl)
			TriggerDamage(target.root.gameObject, damageAmount);
			//target.root.gameObject.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount); //deal damage
		else////Player Control Specific////
			if(hit.collider.transform.root.gameObject.GetComponent<TurretSystem_Health>())
				TriggerDamage(hit.collider.transform.root.gameObject, damageAmount);
				//hit.collider.transform.root.gameObject.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount); //deal damage

		yield return new WaitForSeconds(0.2f); //wait a moment before turning turning off the lighting
		foreach(LineRenderer line in lines)
		{
			line.enabled = false;
		}
		if(useAmmo)
		{
			clipSize-=1;
			if(clipSize <= 0)
				Reload();
		}
	}
	void LaserAttack()
	{
		if(target || topDownPlayerControl || firstPersonPlayerControl)
		{
			GameObject targetGO = target.root.gameObject;
			foreach(LineRenderer line in lines) //activate the laser-like attack 
			{
				line.enabled = true; //enable it and set the origin and end positions
				line.SetPosition(0, turretHead.position);
				line.SetPosition(1, hit.point);

				if(hitSpark && !permaHitSpark) //since the attack is continuous we only need one spark, so we create it if it doesnt exist, and if the hit spark is assigned
					permaHitSpark = Instantiate(hitSpark, hit.point, turretHead.rotation) as GameObject;

			}
			if(targetGO.tag == enemyTag || (targetGO.tag == secondaryEnemyTag && shootSecondaryToo))
			{
				if(!topDownPlayerControl && !firstPersonPlayerControl)
				{////General////
					//TurretSystem_Health enemyScript = targetGO.GetComponent<TurretSystem_Health>(); 
					TriggerDamage(targetGO.gameObject, damageAmount * Time.deltaTime);
					//enemyScript.TakeDamage(damageAmount * Time.deltaTime); //deal damage
					if(useAmmo)
					{
						float clipSizeF = clipSize; //since laser uses a continuous attack, we turn the bullets into floats
						clipSizeF-=1 * Time.deltaTime; //to reduce over time
						if(clipSizeF <= 0) //the rest is same
							Reload();
					}
				}
				else if(hit.collider != null)
				{////Player Control Specific////
					if(hit.collider.transform.root.gameObject.GetComponent<TurretSystem_Health>())
					{
						//TurretSystem_Health enemyScript = hit.collider.transform.root.gameObject.GetComponent<TurretSystem_Health>();
						TriggerDamage(targetGO.gameObject, damageAmount * Time.deltaTime);
						//enemyScript.TakeDamage(damageAmount * Time.deltaTime);
						if(useAmmo)
						{
							clipSize-=1 * Time.deltaTime;
							if(clipSize <= 0)
								Reload();
						}
					}
				}
				else //if its not hitting anything, put the line endings at the end of the ray
				{
					foreach(LineRenderer line in lines)
					{
						line.enabled = true; //enable it and set the origin and end positions
						line.SetPosition(0, turretHead.position);
						line.SetPosition(1, turretHead.forward * shotRange);
					}
				}
			}
		}
		if(permaHitSpark) //keep the permanent spark at the hit point
			permaHitSpark.transform.position = hit.point;
	}
	void ParticleAttack()
	{
		if(useAmmo && !visualParticle)
		{
			clipSize-=1 * Time.deltaTime;
			if(clipSize <= 0)
				Reload();
		}
		if(!reloading)
		{
			foreach(ParticleSystem particle in particles)
				particle.enableEmission = true;
		}
		else
		{
			foreach(ParticleSystem particle in particles)
				particle.enableEmission = false;
		}
	}

	void Reload()
	{
		reloading = true;
		clipSize = 0; //set the clip size to 0 in case it went into negative
		nextFireTime = Time.time + reloadTime; //move the next fire time after reload time
		if(ammo >= fullClipSize) //check if we have enough ammo for a full clip
		{
			ammo-=fullClipSize; //take from total ammo
			clipSize+= fullClipSize; //and add to the mag
		}
		else if(ammo <= fullClipSize) //if we dont have enough ammo
		{
			clipSize+=ammo; //then just throw in what we have
			ammo = 0; 
		}
		if(recoil.Length > 0)
		foreach(Animator anim in reload)
		{
			anim.SetTrigger(reloadTrigger);
		}
		if(isParticle)
			foreach(ParticleSystem particle in particles)
				particle.enableEmission = false;
	}
	#endregion

	#region Triggers

	void DetectTarget(GameObject otherGO)
	{
		if(otherGO.tag == enemyTag) //and if it's tag matches the enemy tag, and the turret doesnt have a target, set it as new target
		{
			if(target == null)
			{
				target = otherGO.transform;
			}
		}
		else
			target = null;
		if(shootSecondaryToo) //do the same for secondary tag
		{
			if(otherGO.tag == secondaryEnemyTag)
			{
				if(target == null)
				{
					target = otherGO.transform;
				}
			}
			else
				target = null;
		}

	}

	void OnTriggerEnter(Collider other) //when the object enters the sensor (layer RangeCollider)
	{
		string otherGOTag = other.gameObject.tag;
		if(otherGOTag != enemyTag || otherGOTag != secondaryEnemyTag && shootSecondaryToo)
		{
			if(ParentWithTagGO(other.gameObject, enemyTag))
			{
				DetectTarget(ParentWithTagGO(other.gameObject, enemyTag));
				nextFireTime = Time.time + (rateOfFire * 0.5f); //let the turret fire the first shot twice as fast if the target has just been aquired
				TurretStart();
			}
		}
		else if(otherGOTag == enemyTag) //if its tagged as enemy, set the fire time
		{
			nextFireTime = Time.time + (rateOfFire * 0.5f); //let the turret fire the first shot twice as fast if the target has just been aquired
			DetectTarget(other.gameObject);
			TurretStart();
		}
		else if(shootSecondaryToo) //check if the turret can shoot air units too
		{
			if(otherGOTag == secondaryEnemyTag) //in that case check against the air enemy tag and set the fire time
			{
				nextFireTime = Time.time + (rateOfFire * 0.5f);
				DetectTarget(other.gameObject);
				TurretStart();
			}
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(Time.frameCount % OnTriggerStayOptimization == 0)
		{
			if(ParentWithTagGO(other.gameObject, enemyTag) || ParentWithTagGO(other.gameObject, secondaryEnemyTag))
			{
				if(other.tag != enemyTag || other.tag != secondaryEnemyTag && shootSecondaryToo)
					DetectTarget(ParentWithTagGO(other.gameObject, enemyTag));
			}
			else if(other.tag == enemyTag || other.tag == secondaryEnemyTag)
				DetectTarget(other.gameObject);
		}
	}

	public GameObject ParentWithTagGO(GameObject child, string tag) //forward the hit GO and its tag
	{
		Transform t = child.transform; //get the GO transform
		while(t.parent != null) //while the transform has a parent
		{
			if(t.parent.tag == tag) //check against a tag
				return t.parent.gameObject; //and if it hits the "Enemy" tag or something, return the object

			t = t.parent; //climb up the hieararchy 
		}
		return null; //if it doesnt find anything
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log(other.gameObject);
		if(target)
		{
			if(ParentWithTagGO(other.gameObject, enemyTag))
			{
				Transform enemyT;
			   	enemyT = ParentWithTagGO(other.gameObject, enemyTag).transform;
				if(enemyT == target)
				{
					target = null;
					TurretShutdown();
				}
			}
			else if(ParentWithTagGO(other.gameObject, secondaryEnemyTag))
			{
				Transform secEnemyT;
				secEnemyT = ParentWithTagGO(other.gameObject, secondaryEnemyTag).transform;
				if(secEnemyT == target)
				{
					target = null;
					TurretShutdown();
				}
			}
			if(other.gameObject.transform == target)
			{
				Debug.Log ("Yes");
				target = null;
				TurretShutdown();
			}

//			if(ParentWithTagGO(other.gameObject, enemyTag) || ParentWithTagGO(other.gameObject, secondaryEnemyTag))
//			{
//				if(ParentWithTagGO(other.gameObject, enemyTag).transform == target)
//				{
//					target = null;
//					TurretShutdown();
//				}
//				else if(ParentWithTagGO(other.gameObject, secondaryEnemyTag).transform == target && shootSecondaryToo)
//				{
//					target = null;
//					TurretShutdown();
//				}
//				else if(other.transform == target)
//				{
//					target = null;
//					TurretShutdown();
//				}
//			}
		}
	}
	#endregion

	#region Pooling

	public void Pooling()
	{
		projectilePool = new TurretSystem_ObjectPooler();
		muzzlePool = new TurretSystem_ObjectPooler();
		sparkPool = new TurretSystem_ObjectPooler();

		pool = new GameObject();
		pool.name = "Pool "+gameObject.name;
		if(!mobileTurret)
			pool.transform.parent = transform;

		int SparkMuzzleEstimatedAmount = Mathf.CeilToInt(1/rateOfFire); //simply estimate the amount to pool. it will grow if its not enough

		if(projectile)
		{
			projectilePool.pooledObject = projectile; //assign the pooled object

			//now since these are physical objects, and a good amount will exist at the same time, we can estimate how much using the fligh time, and rate of fire
			float projFlightTime = projectile.GetComponent<TurretSystem_Projectile>().range; //we get the flight time
			int EstimatedAmount = Mathf.CeilToInt(projFlightTime / (rateOfFire*projFlightTime) + 1); //then estimate how much will exist in that time. eg. 10 / 2 = 5, or 10 / 4 = 3 (rounded to the higher value)
			projectilePool.pooledAmount = EstimatedAmount; //and assign it to the pooled amount
			projectilePool.Start();
			foreach(GameObject obj in projectilePool.pooledObjects)
			{
				obj.transform.parent = pool.transform;
			}

		}
		if(muzzleFlash)
		{
			muzzlePool.pooledObject = muzzleFlash;
			muzzlePool.pooledAmount = SparkMuzzleEstimatedAmount;
			muzzlePool.Start();
			foreach(GameObject obj in muzzlePool.pooledObjects)
			{
				obj.transform.parent = pool.transform;
			}
		}
		if(hitSpark)
		{
			sparkPool.pooledObject = hitSpark;
			sparkPool.pooledAmount = SparkMuzzleEstimatedAmount;
			sparkPool.Start();
			foreach(GameObject obj in sparkPool.pooledObjects)
			{
				obj.transform.parent = pool.transform;
			}
		}
	}

	#endregion

	#region Misc

	#if UNITY_EDITOR 
	void OnValidate() 
	{
		AssignParticleTurrets(); //we use this to automatically assign the turret particle script addon, so you dont have to do it manualy. As it has to be on the gameobject with the ParticleSystem afaik
		CalculateDamagePerSecond(); //we use this to just calculate the damage per second in editor, to make it easier for you to balance the power of turrets in your game
		UpdateBulletOrigin();
	}
	#endif
	void CalculateDamagePerSecond()
	{
		float shotsPerSecond = 1 / rateOfFire;
		if(!isProjectile && !isParticle)
		{
			damagePerSecond = shotsPerSecond * damageAmount * muzzlePositions.Length;
		}
		else if(isProjectile && projectile)
		{
			float projectileDamage = projectile.GetComponent<TurretSystem_Projectile>().damageAmount;
			damagePerSecond = shotsPerSecond * projectileDamage;
		}
		else if(isParticle && particles.Length > 0)
		{
			float totalParticleRate = 0;
			foreach(ParticleSystem particle in particles)
			{
				totalParticleRate+= particle.emissionRate;
			}
			damagePerSecond = totalParticleRate * damageAmount;
		}
	}

	void AssignParticleTurrets()
	{
		if (particles == null)
			return;

		if(particles.Length < 1)
			return;
		foreach(ParticleSystem pt in particles)
		{
			TurretSystem_ParticleTurret ptScript;
			if(!pt.gameObject.GetComponent<TurretSystem_ParticleTurret>())
				ptScript = pt.gameObject.AddComponent<TurretSystem_ParticleTurret>();
			else
				ptScript = pt.gameObject.GetComponent<TurretSystem_ParticleTurret>();

			ptScript.enemyTag = enemyTag;
			ptScript.secondaryEnemyTag = secondaryEnemyTag;
			ptScript.shootSecondaryToo = shootSecondaryToo;
			ptScript.damageAmount = damageAmount;
		}
	}

	void UpdateBulletOrigin()
	{
		if(!useCameraForRaycastAttack)
		{
			if(muzzlePositions.Length > 0 && muzzlePositions[0])
			{
				bulletOrigin = muzzlePositions[randBarrel].position;
				bulletDirection = muzzlePositions[randBarrel].forward;
			}
			else if(turretHead)
			{
				bulletOrigin = turretHead.position;
				bulletDirection = turretHead.forward;
			}
		}
		else
		{
			bulletOrigin = playerCamera.transform.position;
			bulletDirection = playerCamera.transform.forward;
		}
	}

	void IdleState()
	{
		Vector3 curRot = transform.localEulerAngles;
		curRot.y = idleRotation;
		curRot.x = 0;
		curRot.z = 0;
		Quaternion newRot = Quaternion.Euler(curRot);
		if(turretPivot)
			turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, newRot, Time.deltaTime * turnSpeed); //turn the pivot 
		if(turretHead)
			turretHead.rotation = Quaternion.Lerp(turretHead.rotation, newRot, Time.deltaTime * turnSpeed); //and head, based on calculated rotations
	}

	IEnumerator SetRandomRotation()
	{
		idleRotation = Random.Range(0,360);
		if(minHorizontalAngle != 0 || maxHorizontalAngle != 0)
			idleRotation = ClampAngleAIHorizontal(idleRotation);
		yield return new WaitForSeconds(noTargetRandomMovementInterval);
		StartCoroutine("SetRandomRotation");
	}

	public virtual void AimAhead()
	{
		//Debug.DrawLine(turretHead.position, estPos, Color.red);

		Vector3 newPos = targetPos;
		Vector3 newDir = target.GetComponent<Rigidbody>().velocity;

		float dis;
		if(muzzlePositions.Length > 0)
			dis = (newPos - muzzlePositions[randBarrel].position).magnitude;
		else
			dis = (newPos - turretHead.position).magnitude;
		estPos = newPos + (dis/bulletSpeed)*newDir;
	}

	public virtual void Upgrade()
	{
		if(turretUpgrade)
			Instantiate(turretUpgrade, transform.position, transform.rotation);
		else
			return;
		if(upgradeEffect)
			Instantiate(upgradeEffect, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	void TurretStart() //a secondary start void in case the turret will be turned off in some case, and back on again
	{
		if(onStart.Length > 0 && !target)
		foreach(Animator anim in onStart)
		{
			anim.SetTrigger(onStartTrigger);
		}
	}
	 /////////Potentially in future update//////////
	void TurretShutdown()
	{
		if(onShutdown.Length > 0)
		foreach(Animator anim in onShutdown)
		{
			anim.SetTrigger(onShutdownTrigger);
		}
	}
////////////Future version maybe
//	public GameObject ParentWithTagGO(GameObject child, string tag) //forward the hit GO and its tag
//	{
//		Transform t = child.transform; //get the GO transform
//		while(t.parent != null) //while the transform has a parent
//		{
//			if(t.parent.tag == tag) //check against a tag
//				return t.parent.gameObject; //and if it hits the "Enemy" tag or something, return the object
//			
//			t = t.parent.transform; //climb up the hieararchy 
//		}
//		return null; //if it doesnt find anything
//	}
	#endregion
	
}
