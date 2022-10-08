	Introduction :

The 2D/3D Homing Missile is a fast and easy way to implement smart missiles to your games.
As simple as a grab and drop, it will make your missiles smart and bring a new dimension to your gameplay.
You can also create new behaviours using customizable settings right in the editor.

This asset contains :
 - SmartMissile2D script for 2D gamplay
 - SmartMissile3D script for 3D gamplay
 - A Readme file with instructions
 - Two demo scenes

Feel free to contact me if you have any question : youe.graillot@gmail.com

	How to integrate ?

First thing you need to do is add the appropriate script on your projectile containing a Rigidbody.
SmartMissile2D is for Rigidbody2D projectiles and SmartMissile3D is for Rigidbody3D projectiles.
Once your missile prefab is created, you can instanciate it as you would usually do :
	GameObject newProjectile = Instantiate(projectilePrefab, position, rotation) as GameObject;
	newProjectile.GetComponent<Rigidbody>().AddForce(direction);
Now you just have to define the tag parameter on the SmartMissile script and on your the objects you want to target.
In order for SmartMissile script to detect them, these objects must contains a Collider component.

	How does it work ?

SmartMissile2D and SmartMissile3D inherit from SmartMissile abstract class.
Using one of these script will make your projectile find a new target within a cone-shaped zone in front of the projectile..
Once the target is acquired, SmartMissile script will alter the Rigidbody direction in order to approach the target.
If the target comes out of the zone in front of the projectile, SmartMissile will look for another one.

	Customization

You will find five these parameters under SmartMissile component in the property drawer :
Missile
	Life Time - Missile will be destroyed after this amount in seconds, 0 for infinite lifetime.
	On New Target Found - This event will be called when the missile find a new target.
	On Target lost - This event will be called when the missile loose its target.
Detection
	Can Loose Target - Disable to prevent the missile from loosing its target when out of range.
	Search Range - Define the maximum distance to find a target.
	Search Angle - Define the detection field angle to find a target.
Guidance
	Guidance Intensity - A highter value result in a faster trajectory change. A negative value will cause the projectile to avoid the target.
	Distance Influence - Guidance Intensity is multiplied by this value depending on the distance with the target. T=0 representing a close target and T=1 one at maximum range.
Target
	Offset - Use this Vector3 if your missile should aim at an offset position from the center of the target.
	Target Tag - SmartMissile will look for this tag.
Debug
	Draw Search Zone - Enable to draw the search zone in the scene view.
	Zone Color - Define the color of the drawn search zone.