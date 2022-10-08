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

public class TurretSystem_OnEnableDisable : MonoBehaviour 
{
	//for the sake of optimization set this in the inspector
	[Tooltip("Particle to be played on enable. Such as muzzle flash. If not assigned it will automatically be assigned. For the sake of optimization set this in the inspector.")]
	public ParticleSystem m_particle;
	[Tooltip("Audio to be played on enable. Such as gun shot. If not assigned it will automatically be assigned. For the sake of optimization set this in the inspector.")]
	public AudioSource m_audio;
	[Tooltip("For pooling set this to something other than 0. It will disable the gameobject when it hits 0.")]
	public float disableTimer = 0f;

	private float disableTimerKeeper;

	void Awake()
	{
		disableTimerKeeper = disableTimer;
		//if its not set, the script will try assigning it automatically on awake
		if(!m_particle && GetComponent<ParticleSystem>())
			m_particle = GetComponent<ParticleSystem>();
		if(!m_audio && GetComponent<AudioSource>())
			m_audio = GetComponent<AudioSource>();
	}

	void Update()
	{
		if(disableTimer < 0)
		{
			gameObject.SetActive(false);
			disableTimer = disableTimerKeeper;
		}

		if(disableTimer != 0)
			disableTimer-=1 * Time.deltaTime;
	}

	void OnEnable()
	{
		if(m_audio)
			m_audio.Play();
		if(m_particle)
			m_particle.enableEmission = true;
	}

	void OnDisable()
	{
		if(m_audio)
			m_audio.Stop();
		if(m_particle)
			m_particle.enableEmission = false;
	}
}
