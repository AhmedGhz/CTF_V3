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

public class OnHitPlaySound : MonoBehaviour 
{
	public AudioSource sound;
	public string myTag;

	public bool onCollision = true;
	public bool onTrigger = true;
	public bool onParticle = true;

	void Start () 
	{
		if(!sound)
			sound = GetComponent<AudioSource>();
	}
	
	void OnHitColl(Collision coll)
	{
		if(myTag != "")
		{
			if(myTag == coll.gameObject.tag)
				sound.Play();
		}
		else
		{
			sound.Play();
		}
	}
	void OnHitTrig(Collider coll)
	{
		if(myTag != "")
		{
			if(myTag == coll.gameObject.tag)
				sound.Play();
		}
		else
		{
			sound.Play();
		}
	}

	void OnHitParticle(GameObject gO)
	{
		if(myTag != "")
		{
			if(myTag == gameObject.tag)
				sound.Play();
		}
		else
		{
			sound.Play();
		}
	}

	void OnColliderEnter(Collision coll)
	{
		if(onCollision)
			OnHitColl(coll);
	}

	void OnTriggerEnter(Collider trig)
	{
		if(onTrigger)
			OnHitTrig(trig);
	}
	

	void OnParticleCollision(GameObject gO)
	{
		if(onParticle)
			OnHitParticle(gO);
	}
}
