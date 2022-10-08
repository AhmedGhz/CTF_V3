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

public class TurretSystem_ParticleTurret : MonoBehaviour
{
	//all this will be assigned automatically if the particle is automatically added
	public string enemyTag;
	public string secondaryEnemyTag;
	public bool shootSecondaryToo;
	public float damageAmount;

	public void TriggerDamage(GameObject hitGO, float damageAmount)
	{
		hitGO.GetComponent<TurretSystem_Health>().TakeDamage(damageAmount);
	}

	void OnParticleCollision(GameObject other)
	{
		GameObject hitGO = other.transform.root.gameObject;
		if(hitGO.tag == enemyTag)
		{
			TriggerDamage(hitGO, damageAmount);
		}
		if(hitGO.tag == secondaryEnemyTag && shootSecondaryToo)
		{
			TriggerDamage(hitGO, damageAmount);
		}
	}
}
