using UnityEngine;

public class Ennemy : MonoBehaviour
{
	Animator m_animator;


	void Start()
	{
		m_animator = GetComponent<Animator>();
	}

	void OnCollisionEnter(Collision input)
	{
		Destroy(input.gameObject);
		m_animator.SetTrigger("Hit");
	}

	void OnCollisionEnter2D(Collision2D input)
	{
		Destroy(input.gameObject);
		m_animator.SetTrigger("Hit");
	}
}
