using UnityEngine;
using UnityEngine.EventSystems;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SwingController : MonoBehaviour
{
    #region Variables
    // Main variables for physics handling
    private Rigidbody rb;
    private GameObject anchor;
    private HingeJoint joint;
    private HingeJoint anchorJoint;
    public float moveSpeed;

    // Variables for tether indicator
    private GameObject[] gos;
    private GameObject indicatorGameObject;
    private GameObject indicatorSphere;
    private Transform tetherTransform;
    private Vector3 closestTether;
    public float indicatorOffset = 0.5f;
    public float indicatorPressedOffset = 0.4f;
    public float maxTetherDistance;

    // Variables for effects 
    public AudioSource connectSound;
    public AudioSource releaseSound;
    private bool soundplayed;



    // Misc.
    private LineRenderer lr;
    #endregion
    private void Start()
    {
        // Lock rotation of rb 
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;

        // Tether indicator 
        gos = GameObject.FindGameObjectsWithTag("Tether Points");
        anchor = GameObject.Find("Anchor");
        tetherTransform = FindClosestTetherPoint(gos).transform; // Get the closest tether transform
        indicatorGameObject = GameObject.Find("Indicator"); // Find Game Object "Indicator"
        indicatorSphere = GameObject.Find("IndicatorSphere"); // Find Game Object "Indicator"
        indicatorGameObject.transform.position = new Vector3(tetherTransform.position.x, tetherTransform.position.y + indicatorOffset, 0);
        
        // Sound
        connectSound = GetComponent<AudioSource>();
        soundplayed = false;

        // Misc
        lr = GetComponent<LineRenderer>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }

    }
    private void Update()
    {
        // Hold
        if (Input.GetMouseButton(0))
        { 
            DoSwingAction();
        }
        // Release
        else
        {
            DoFallingAction();
        }
        // Update rope positions
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, anchor.transform.position);
    }
    void DoSwingAction()
    {
        // Calculate angle between player and tether point and rotate the player around it
        var dir = (tetherTransform.position - transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // Fire Rope (Extra check, every thing here runs only once per tethering action) 
        if (Input.GetMouseButtonDown(0))
        {
            // Get the vector to the closest Tether point as long as there are points

                closestTether = FindClosestTetherPoint(gos).transform.position - transform.position;

                // Move pressed indicator position to tetherTransform pos
            indicatorGameObject.transform.position = new Vector3(tetherTransform.position.x, tetherTransform.position.y + indicatorPressedOffset , 0);
            indicatorSphere.transform.position = new Vector3(tetherTransform.position.x, tetherTransform.position.y, 0);

            // Shoot a ray out towards that position       
            LayerMask ignorePlayer = ~(1 << LayerMask.NameToLayer("Player"));
            Physics.Raycast(transform.position, closestTether, out RaycastHit hit, maxTetherDistance, ignorePlayer);
            if (hit.collider)
            {
                if (hit.collider.tag == "Tether Points")
                {
                    // Move the anchor to the correct position
                    anchor.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                    // Zero out any rotation of anchor
                    anchor.transform.rotation = Quaternion.identity;

                    // Create HingeJoints
                    joint = gameObject.AddComponent<HingeJoint>();
                    joint.axis = Vector3.forward;
                    joint.anchor = Vector3.zero;
                    joint.connectedBody = anchor.GetComponent<Rigidbody>();

                    // Create anchor HingeJoint
                    anchorJoint = anchor.AddComponent<HingeJoint>();
                    anchorJoint.axis = Vector3.forward;
                    anchorJoint.anchor = Vector3.zero;
                    lr.enabled = true; // show rope

                    // Play connect sound
                    if (!soundplayed)
                    {
                        connectSound.Play(0);
                        soundplayed = true;
                    }
                }
            }
        }
    }
    void DoFallingAction()
    {
        // Keep updating position of closest while flying as long as we find tether points
        if (FindClosestTetherPoint(gos) != null)
        {
            tetherTransform = FindClosestTetherPoint(gos).transform;
        }

        // Move indicator to the closest tether point
        indicatorGameObject.transform.position = new Vector3(tetherTransform.position.x, tetherTransform.position.y + indicatorOffset, 0f);
        // Move sphere away from screen
        indicatorSphere.transform.position = new Vector3(0f, -1.0f, 0f);

        // Update player rotation while flying
        Vector3 direction = rb.velocity.normalized;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (rb.velocity != Vector3.zero)
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - (360.0f));
        
        // Called only once
        if(Input.GetMouseButtonUp(0))
        {
            releaseSound.Play(0);
            // Disable sound
            soundplayed = false;
            // Destroy HingeJoints
            Destroy(joint);
            Destroy(anchorJoint);
            // Hide rope
            lr.enabled = false;
        }
    }

    GameObject FindClosestTetherPoint(GameObject[] gos)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;

        //Player position
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            // Get vector from Tether point to Player
            Vector3 diff = go.transform.position - position;
            // Get distance value of this vector
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

}