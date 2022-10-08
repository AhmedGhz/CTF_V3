using UnityEngine;
using UnityEngine.Events;

public abstract class SmartMissile : MonoBehaviour { }

public abstract class SmartMissile<RgbdType, VecType> : SmartMissile
{
	[Header("Missile")]
	[SerializeField, Tooltip("In seconds, 0 for infinite lifetime.")]
	float m_lifeTime = 5;
	[SerializeField]
	UnityEvent m_onNewTargetFound;
	[SerializeField]
	UnityEvent m_onTargetLost;

	[Header("Detection")]
	
	[SerializeField, Tooltip("Range within the missile will search a new target.")]
	public float m_searchRange = 10f;
	[SerializeField, Range(0, 360)]
	public int m_searchAngle = 90;
	[SerializeField, Tooltip("If enabled, target is lost when out of the range.")]
	public bool m_canLooseTarget = true;

	[Header("Guidance")]
	[SerializeField, Tooltip("Intensity the missile will be guided with.")]
	public float m_guidanceIntensity = 5f;
	[SerializeField, Tooltip("Increase the intensity depending on the ditance.")]
	public AnimationCurve m_distanceInfluence = AnimationCurve.Linear(0, 1, 1, 1);

	[Header("Target")]
	[SerializeField, Tooltip("Use this if the center of the object is not what you target.")]
	public VecType m_targetOffset;
	[SerializeField, HideInInspector]
	public string m_targetTag = "Untagged";
	public string TargetTag
	{
		get { return m_targetTag; }
		set { m_targetTag = value; }
	}

	[Header("Debug")]
	[SerializeField, Tooltip("Color of the search zone."), HideInInspector]
	public Color m_zoneColor = new Color(255, 0, 155, 0.1f);
	[SerializeField, Tooltip("Color of the line to the target."), HideInInspector]
	public Color m_lineColor = new Color(255, 0, 155, 1);
	[SerializeField, Tooltip("Draw the search zone."), HideInInspector]
	public bool m_drawSearchZone = false;

	public RgbdType m_rigidbody;
	public Transform m_target;
	public float m_targetDistance;
	public VecType m_direction;

	void Start()
	{
		m_targetDistance = m_searchRange;

		if (m_lifeTime > 0)
			Destroy(gameObject, m_lifeTime);
	}

	void FixedUpdate()
	{
		if (m_target != null)
		{
			if (m_canLooseTarget && !isWithinRange(m_target.transform.position))
			{
				m_target = null;
				m_targetDistance = m_searchRange;
				m_onTargetLost.Invoke();
			}
			else
			{
				goToTarget();
			}
		}
		else if (m_target = findNewTarget())
			m_onNewTargetFound.Invoke();
	}

	/// <summary>
	/// Find a new target within the search zone. Returns null if no target is found.
	/// </summary>
	protected abstract Transform findNewTarget();
	
	/// <summary>
	/// Returns true if the input Coodinates are within the search zone.
	/// </summary>
	protected abstract bool isWithinRange(Vector3 coordinates);

	/// <summary>
	/// Update the direction of the Rigidbody.
	/// </summary>
	protected abstract void goToTarget();
}