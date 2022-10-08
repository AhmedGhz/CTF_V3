﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class SmartMissile3D : SmartMissile<Rigidbody, Vector3>
{
	void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	protected override Transform findNewTarget()
	{
		foreach (Collider newTarget in Physics.OverlapSphere(transform.position, m_searchRange))
			if (newTarget.gameObject.CompareTag(m_targetTag) && isWithinRange(newTarget.transform.position))
			{
				m_targetDistance = Vector3.Distance(newTarget.transform.position, transform.position);
				return newTarget.transform;
			}

		return null;
	}

	protected override bool isWithinRange(Vector3 Coordinates)
	{
		if (Vector3.Distance(Coordinates, transform.position) < m_targetDistance
			&& Vector3.Angle(transform.forward, Coordinates - transform.position) < m_searchAngle / 2)
			return true;

		return false;
	}

	protected override void goToTarget()
	{		
		m_direction = (m_target.position + m_targetOffset - transform.position).normalized * m_distanceInfluence.Evaluate(1 - (m_target.position + m_targetOffset - transform.position).magnitude / m_searchRange);
		m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity + m_direction * m_guidanceIntensity, m_rigidbody.velocity.magnitude);
		
		if (m_rigidbody.velocity != Vector3.zero)
			transform.LookAt(m_rigidbody.velocity);
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
				Handles.DrawSolidArc(transform.position, Quaternion.AngleAxis(90, -transform.right) * transform.forward, Quaternion.AngleAxis(-m_searchAngle / 2, transform.up) * transform.forward, m_searchAngle, m_searchRange);
				Handles.DrawSolidArc(transform.position, transform.forward, Quaternion.AngleAxis(-m_searchAngle / 2, transform.up) * transform.forward, 360, m_searchRange);
			}

			// Draw a line to the target
			if (m_target != null)
			{
				Handles.color = m_lineColor;
				Handles.DrawLine(m_target.position + m_targetOffset, transform.position);
			}
		}
	}
#endif
}
