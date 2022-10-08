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

public class TurretSystem_Projectile : MonoBehaviour 
{
	[Tooltip("Check this if the projectile is a homing missile.")]
	public bool isMissile; //set this if the projectile is a homing missile
	[Tooltip("Check this if its a rigidbody projectile. To move using Rigidbody.MovePosition not Transform.Translate. Also set the angular velocity" +
		" to something high so the bullet doesn't stray, uncheck isKinematic, and set it to interpolate.")]
	public bool isRigidbody;
	[Tooltip("Speed of the missile.")]
	public float speed = 10; //speed of the missile
	[Tooltip("Range before it expires and explodes. If set to 0 the projectile will never expire, and won't explode on impact. Useful for death walls.")]
	public float range = 10; //range before it expires
	[Tooltip("How much damage it deals. The turret will set this.")]
	public float damageAmount = 10; //amount of damage it deals
	[Tooltip("Explossion prefab.")]
	public GameObject explossion; //explossion prefab on impact
	public string enemyTag; //tag of enemy it deals damage to
	public string secondaryEnemyTag; //tag of enemy it deals damage to
	[HideInInspector]public Transform missileTarget; //target it follows, assigned by the launcher when fired
	[Tooltip("How fast the missile turns towards target.")]
	public float missileTurnSpeed = 5; //how fast the missile turns
	[Tooltip("Whatever layer can be hit should be on this mask. Triggers generaly shouldn't be on this mask (eg. target sensor trigger collider)")]
	public LayerMask obstaclesLayerMask; //whatever layer can be hit should be on this mask. Triggers generaly shouldn't be on this mask (eg. target sensor trigger collider)
	[Tooltip("Script name of the hit object to send a message to. SendMessage is slower than GetComponent, so you're better off with override, or just adding your code to it. " +
	         "Search TakeDamage, to find the places damage is applied.")]
	public string customScriptToSendMsgTo;

	private Vector3 dir;
	private Quaternion desiredRot;
	private float dist;
	private TrailRenderer trail;
	private GameObject expInstance;
	private Rigidbody myRB;

	void Start()
	{
		if(isRigidbody)
			myRB = GetComponent<Rigidbody>();
		if(explossion) //if we have an explosion prefab
		{
			expInstance = Instantiate(explossion, transform.position, Quaternion.identity) as GameObject; //we create an instance of it
			expInstance.SetActive(false); //and disable it, since we'll only instantiate it once, and then enable/disable when its needed.
			if(!GameObject.Find("Explosion Pool"))
			{
				GameObject expPool = new GameObject();
				expPool.name = "Explosion Pool";
			}
			else
			{
				expInstance.transform.parent = GameObject.Find("Explosion Pool").transform; //find a pool and add to it
			}
		}
	}

	void Update ()
	{
		if(!isRigidbody)
			transform.Translate(Vector3.forward * Time.deltaTime * speed);

		if(!isMissile) //if its not a missile, just go forward the set distance then explode
		{
			dist+= Time.deltaTime * speed;

			if(dist >= range && range != 0)
				Explode();
		}
		else //if it is a missile, follow the target and explode after set distance
		{
			dist += Time.deltaTime * speed;

			if(dist >= range && range != 0 || !missileTarget && isMissile)
				Explode();

			if(missileTarget)
			{
				dir = (missileTarget.position - transform.position).normalized;
				desiredRot = Quaternion.LookRotation(dir);
				transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * missileTurnSpeed);
			}
		}
	}

	public void TriggerDamage(GameObject hitGO, float damageAmount)
	{
		hitGO.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount);
		if(customScriptToSendMsgTo != "")
			hitGO.SendMessage(customScriptToSendMsgTo);
	}
	void FixedUpdate()
	{
		if(isRigidbody)
		{
			myRB.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
		}
	}


	void OnTriggerEnter(Collider other) //if the enemy object enters the trigger, deal damage and explode
	{
		if(other.gameObject.layer != LayerMask.NameToLayer("RangeCollider"))
		{
			GameObject otherGO = other.gameObject.transform.root.gameObject;
			if(otherGO) //check if it was destroyed in the meantime, to avoid null errors
			{
				if(otherGO.tag == enemyTag && otherGO)
					TriggerDamage(otherGO, damageAmount); //if its primary enemy tag, deal damage
				if(otherGO.tag == secondaryEnemyTag && secondaryEnemyTag != "" && otherGO) //if its secondary enemy tag, and it exists, deal damage. leaving it empty will not deal damage if the projectile hits secondary enemy
					TriggerDamage(otherGO, damageAmount);
			}
			if(range != 0)
				Explode();
		}
	}

	void OnCollisionEnter(Collision other)  //if the enemy object enters the collider, deal damage and explode
	{
		if(other.gameObject.layer != LayerMask.NameToLayer("RangeCollider"))
		{
			GameObject otherGO = other.gameObject.transform.root.gameObject;
			if(otherGO) //check if it was destroyed in the meantime, to avoid null errors
			{
				if(otherGO.tag == enemyTag && otherGO)
					TriggerDamage(otherGO, damageAmount); //if its primary enemy tag, deal damage
				if(otherGO.tag == secondaryEnemyTag && secondaryEnemyTag != "" && otherGO) //if its secondary enemy tag, and it exists, deal damage. leaving it empty will not deal damage if the projectile hits secondary enemy
					TriggerDamage(otherGO, damageAmount);
			}
			if(range != 0)
				Explode();
		}
	}

	
	public void Explode()
	{
		expInstance.transform.position = transform.position; //change the explosion position where its needed
		expInstance.SetActive(true); //enable it
		dist = 0; //reset the projectile traveled distance
		gameObject.SetActive(false); //and disable it
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
}
