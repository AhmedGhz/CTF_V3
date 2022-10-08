using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class SmartMissile2D : SmartMissile<Rigidbody2D, Vector2>
{
	void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

	protected override Transform findNewTarget()
	{
		foreach (Collider2D newTarget in Physics2D.OverlapCircleAll(transform.position, m_searchRange))
			if (newTarget.gameObject.CompareTag(m_targetTag) && isWithinRange(newTarget.transform.position))
			{
				m_targetDistance = Vector2.Distance(newTarget.transform.position, transform.position);
				return newTarget.transform;
			}

		return null;
	}
	
	protected override bool isWithinRange(Vector3 Coordinates)
	{
		if (Vector2.Distance(Coordinates, transform.position) < m_targetDistance
			&& Vector2.Angle(transform.forward, Coordinates - transform.position) < m_searchAngle / 2)
			return true;

		return false;
	}
	
	protected override void goToTarget()
	{		
		m_direction = (m_target.position + (Vector3)m_targetOffset - transform.position).normalized * m_distanceInfluence.Evaluate(1 - (m_target.position + (Vector3)m_targetOffset - transform.position).magnitude / m_searchRange);
		m_rigidbody.velocity = Vector2.ClampMagnitude(m_rigidbody.velocity + m_direction * m_guidanceIntensity, m_rigidbody.velocity.magnitude);
		
		if (m_rigidbody.velocity != Vector2.zero)
			transform.LookAt(new Vector3(m_rigidbody.velocity.x, m_rigidbody.velocity.y, transform.position.z));
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (enabled)
		{
			// Draw the search zone
			if (m_drawSearchZone)
			{
				Handles.color = m_zoneColor;
				Handles.DrawSolidArc(transform.position, transform.right, Quaternion.AngleAxis(-m_searchAngle/2, transform.right) * transform.forward, m_searchAngle, m_searchRange);
			}

			// Draw a line to the target
			if (m_target != null)
			{
				Handles.color = m_lineColor;
				Handles.DrawLine(m_target.position + (Vector3)m_targetOffset, transform.position);
			}
		}
	}
#endif
}
