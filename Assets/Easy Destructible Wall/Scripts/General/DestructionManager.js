/**
* This script handles all of the destruction side of things for each chunk.
*
* Here is what it does:
* 1) Waits for the next physics engine tick
* 2) Checks if the object's health is low
* 3) Waits for the next time statements such as Destroy() are processed
* 4) The script iterates through every child object
* 5) The chunk will take damage based on its angular velocity
* 
* 5) If the chunk health is < 1: For each child in the chunk, a brick is spawned
*    in its place
* 6) The velocity from the chunk is transferred into each new brick
* 7) Now that the chunk has been broken, it is now a good idea to delete it,
*    since it's no longer necessary
*
* 8) Meanwhile: Every time the chunk hits into something, it takes some health 
*    away from itself. This amount is determined by how hard the collision was, and the
*    set "impactMultiplier"
*/

//The object (in this case a single brick), to replace each and every child of the chunk
var replacer : GameObject;

//The hitpoints of the object, when this value is below 1, the chunk will fracture
var health : int = 100; 

//These two variables are used to multiply damage based on velocity and torque respectively.
private var impactMultiplier : float = 1.5; 
private var twistMultiplier : float = 0.05;

function Start() {
	while(true) {		
		/** 
		* Becuse of the timing between executions of a script, it is necessary to wait
		* for FixedUpdate to keep it running alongside the physics engine 
		*/
		yield WaitForFixedUpdate(); 
		
		/**
		* Since we are acessing the chunks rigidbody component many times, it would be
		* a good idea to cache it.
		*/
		var chunkRB : Rigidbody = rigidbody; 
		var chunkAngVelocity : Vector3 = chunkRB.angularVelocity;		
		
		/**
		* Visualize Chunk Health
		* To activate, clear both lines which contain a series of "-"
		*/
		/*----
		for (var childColor : Transform in transform) {
			childColor.renderer.material.color = Color.Lerp(Color.red, Color.blue, health * 0.01);
		}
		----*/
		
		/**
		* Damage based on torque. When an object spins very fast, it is expected that this force will
		* tear it apart
		*/
		health -= Mathf.Round(chunkAngVelocity.magnitude * twistMultiplier);
		
		if (health < 1) {
			/**
			* Since we are relying on functions that don't get updated straight away, 
			* such as Destroy(), we need to make sure the script executes alongside that timing 
			*/
			yield;
			
			//Iterate through each child of the chunk of bricks and spawn replacements
			for(var child : Transform in transform) {
				spawned = Instantiate(replacer, child.position, transform.rotation);
				
				//Cache brick's rigidbody component for performance reasons
				var spawnRB = spawned.rigidbody; 
				//Transfer velocity
				spawnRB.velocity = rigidbody.GetPointVelocity(child.position);
				//Transfer torque
				spawnRB.AddTorque(rigidbody.angularVelocity);
			}
			Destroy(gameObject);
		}
	}
}

//When the chunk hits another object, take some of its health away
function OnCollisionEnter(collision : Collision) {
	var relativeVelocity = collision.relativeVelocity.magnitude;
	
	//If the chunk was hit by a rigidbody, multiply the damage by its mass
	if(collision.rigidbody) {
		health -= relativeVelocity * impactMultiplier * collision.rigidbody.mass;
		return;
	}
	
	/**
	* If the chunk isn't a rigidbody, then simply damage it without taking mass into account
	*/
	health -= relativeVelocity * impactMultiplier;
}