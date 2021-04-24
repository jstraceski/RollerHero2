using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour {
    private Rigidbody _rb;
    private Transform _tf;
    public Transform _tf2;

    public GameObject _gObj;
    private Transform _gtf;
    private Collider _gcldr;

    public float force = 15;
    public float arielForce = 8;
    public float rotSpeed = 800;
    public float angDrag = 0.8f;

    public float jumpForce = 10;
    public float heightCheck = 0.25f;
    public float maxSpeed = 9f;

    public Rigidbody wheelRigidbody;
    public Animator aCont;
    public float respawnSpeed = 10.0f;
    
    public float tapDelay = 0.1f;
    
    private float rightTurnDelay, leftTurnDelay;
    private float hInputLast = 0;
    private bool lastRight, lastLeft;
    private bool tappedRight = false;
    private bool tappedLeft = false;

    private bool gCheck = false;
    private bool grounded = false;
    private AudioSource _audioSource;
    private float angMomentum = 0;
    private bool _groundedState = false;
    public bool respawning = false;

    class RespawnLocation {
        public bool grounded;
        public Vector3 loc;
        public Quaternion rot;

        public RespawnLocation(GameObject o, bool grd) {
            loc = o.transform.position;
            rot = o.transform.rotation;
            if (grd) {
                loc += Vector3.up * 2.5f; 
            }
            grounded = grd;

        }
    }

    private static readonly int Animation_State = Animator.StringToHash("Animation_State");

    private GrappleBehavior grappleBehavior;

    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody>();
        _tf = transform;
        _audioSource = gameObject.GetComponent<AudioSource>();
        _gtf = _gObj.transform;
        _gcldr = _gObj.GetComponent<Collider>();
        grappleBehavior = FindObjectOfType<GrappleBehavior>();
    }

    private Vector3 normal = Vector3.up;

    private List<RespawnLocation> locations = new List<RespawnLocation>();
    public float locTimer = 0;
    private float locTime = 0.25f;
    
    private void Update() {
        bool grnd = Grounded();

        if (respawning) {
            return;
        }
        
        if (locTimer <= 0) {
            locTimer = locTime;
            locations.Add(new RespawnLocation(gameObject, grnd));
            
            if (locations.Count >= 3) {
                RespawnLocation respawnLocation = locations[0];
                if (!respawnLocation.grounded) {
                    locations.Remove(respawnLocation);
                } else {
                    int groundCount = 0;
                    for (int i = 0; i < locations.Count; i++) {
                        if (locations[i].grounded) {
                            groundCount++;
                            if (groundCount >= 10) {
                                break;
                            }
                        }
                    }

                    if (groundCount >= 10) {
                        locations.Remove(respawnLocation);
                    }
                }
            }
        } else {
            locTimer -= Time.deltaTime;
        }

        
        if (grnd) {
            if (Input.GetButtonDown("Jump") && !respawning) {
                _rb.AddForce(_tf.up * jumpForce, ForceMode.Impulse);
            }

            float speed = Mathf.Abs(Vector3.Dot(_rb.velocity, _tf.forward));
            float soundCutoff = 1.5f;
            if (speed > soundCutoff) {
                _audioSource.volume = Mathf.Min(0.25f, 0.05f + 0.5f * ((speed - soundCutoff) / (8 - soundCutoff)));
            } else {
                _audioSource.volume = 0;
            }
        } else {
            _audioSource.volume = 0;
        }
    }

    public ParticleSystem particleSystem;

    public void Respawn() {
        respawning = true;
        respawnOrb.SetActive(true);
        var emission = particleSystem.emission; // Stores the module in a local variable
        emission.enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        grappleBehavior.disabled = true;
    }

    public GameObject respawnOrb;
    private RespawnLocation respawnLocation = null;
    RespawnLocation lastRespawn = null;
    public bool groundHit = false;

    public  int groundCount = 0;
    void RespawnUpdate() {
        Respawn();
        if (!groundHit && respawnLocation == null) {
            RespawnLocation respawnTmp = null;
            Debug.Log(locations.Count);
            int count = locations.Count;
            for (int i = locations.Count - 1; i >= 0; i--) {
                RespawnLocation newRespawn = locations[i];
                if (respawnTmp != null && (Physics.Raycast(transform.position, newRespawn.loc) || count - i >= 2)) {
                    break;
                } else if (respawnTmp != null) {
                    locations.Remove(respawnTmp);
                }

                respawnTmp = newRespawn;
                if (respawnTmp.grounded) {
                    if (respawnTmp != lastRespawn) {
                        groundCount++;
                    }
                    
                    if (groundCount >= 3) {
                        groundHit = true;
                        break;
                    }
                }
            }
            respawnLocation = respawnTmp;
        }

        lastRespawn = respawnLocation;

        Vector3 dirVector;
        if (respawnLocation != null && (dirVector = (respawnLocation.loc - transform.position)).sqrMagnitude > 0.1) {
            Vector3 dirNormal = dirVector.normalized;
            float ammount = Time.deltaTime * respawnSpeed;
            float dAmmount = Vector3.Dot(dirNormal, dirVector);
            if (dAmmount < ammount) {
                ammount = dAmmount;
                respawnLocation = null;
            }
            
            transform.position += dirNormal * ammount;
            transform.rotation = Quaternion.Lerp(transform.rotation, respawnLocation.rot, Time.deltaTime);
            
        } else {
            if (groundHit || respawnLocation == null || (respawnLocation.grounded && locations.Count == 1)) {
                if (locations.Count > 3) {
                    locations.Remove(respawnLocation);
                }
                respawnLocation = null;
                respawning = false;
                groundHit = false;
                groundCount = 0;
                respawnOrb.SetActive(false);
                var emission = particleSystem.emission; // Stores the module in a local variable
                emission.enabled = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                grappleBehavior.disabled = false;
            } else {
                locations.Remove(respawnLocation);
                respawnLocation = null;
            }
        }

    }

    // Update is called once per frame
    void FixedUpdate() {
        gCheck = false;
        _groundedState = Grounded();


        if (respawning) {
            RespawnUpdate();
            return;
        }
        
        angMomentum += Input.GetAxis("Horizontal") * Time.deltaTime * rotSpeed;
        _tf2.Rotate(_tf.up, angMomentum * Time.deltaTime);
        float currentForce = _groundedState ? force : arielForce;
        Vector3 forceImpulse = _tf.forward * (Input.GetAxis("Vertical") * currentForce);
        float upComponent = Vector3.Dot(normal, Vector3.up);

        // steepness check or tether check
        if (upComponent < 0.75 || grappleBehavior.GrappleTaught()) {
            // remove the upwards component of the normal
            Vector3 dirNormal = normal - upComponent * Vector3.up;
            dirNormal = dirNormal.normalized;

            // the forward normal scaled by the movement direction
            float moveScalar = Input.GetAxisRaw("Vertical") > 0 ? 1 : -1;
            float normalVal = Vector3.Dot(_tf.forward * moveScalar, dirNormal);

            // If the player is facing away from the tether direction move the rotation in the direction of the normal
            // But only so that they are perpendicular to the tether direction
            if (normalVal < 0 && normalVal > -0.85) {
                
                // 
                normalVal = -(Mathf.Abs(Mathf.Abs(normalVal) - 0.5f) * 2f)
                    + Mathf.Abs(Mathf.Sin(normalVal * 2 * Mathf.PI) / Mathf.PI) + 1f;
                Debug.Log("Turning Towards Normal");
                if (Vector3.Dot(transform.right * moveScalar, dirNormal) > 0) {
                    angMomentum += Time.deltaTime * rotSpeed * 1f * normalVal;
                } else {
                    angMomentum -= Time.deltaTime * rotSpeed * 1f * normalVal;
                }
            }
        }


        //angMomentum = angMomentum * angDrag;
        angMomentum = angMomentum - angMomentum * (1 - angDrag) * Time.deltaTime * 50;

        
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            wheelRigidbody.angularVelocity *= 0.0f;
            aCont.SetInteger(Animation_State, 2);
        } else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0 && _groundedState) {
            aCont.SetInteger(Animation_State, 1);
        } else {
            aCont.SetInteger(Animation_State, 0);
        }

        if (Vector3.Dot(_rb.velocity, forceImpulse.normalized) < maxSpeed) {
            _rb.AddForce(forceImpulse);
        }


        if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Horizontal") > hInputLast && !lastRight) {
            if (rightTurnDelay > 0) {
                angMomentum += rotSpeed * 1.1f;
                rightTurnDelay = 0;
                tappedRight = true;
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Horizontal") < hInputLast && !lastLeft) {
            if (leftTurnDelay > 0) {
                angMomentum -= rotSpeed * 1.1f;
                leftTurnDelay = 0;
                tappedLeft = true;
            }
        }

        if (Input.GetAxisRaw("Horizontal") > 0 && !tappedRight) {
            rightTurnDelay = tapDelay;
        }

        if (Input.GetAxisRaw("Horizontal") < 0 && !tappedLeft) {
            leftTurnDelay = tapDelay;
        }

        if (Input.GetAxisRaw("Horizontal") < hInputLast || Input.GetAxisRaw("Horizontal") == 0) {
            lastRight = false;
            tappedRight = false;
        } else {
            lastRight = true;
        }

        if (Input.GetAxisRaw("Horizontal") > hInputLast || Input.GetAxisRaw("Horizontal") == 0) {
            lastLeft = false;
            tappedLeft = false;
        } else {
            lastLeft = true;
        }

        hInputLast = Input.GetAxis("Horizontal");

        rightTurnDelay = Mathf.Max(0, rightTurnDelay - Time.deltaTime);
        leftTurnDelay = Mathf.Max(0, leftTurnDelay - Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Debug.DrawLine(transform.position, transform.position + normal * 2, Color.cyan);
        if (_gtf != null) {
            Debug.DrawLine(_gtf.position, _gtf.position + -_tf.up * heightCheck, Color.magenta);
        }
    }

    bool Grounded() {
        bool didHit = false;
        //return true;
        RaycastHit hit;
        if (Physics.Raycast(_gtf.position, -_tf.up, out hit, heightCheck)) {
            normal = hit.normal;
            didHit = true;
        } else if (Physics.Raycast(_gtf.position, -_tf.up, out hit, heightCheck * 3f)) {
            normal = hit.normal;
        }

        if (grappleBehavior.grappleState == GrappleBehavior.GrappleState.Tethered) {
            normal += (grappleBehavior.tetherRb.position - transform.position).normalized;
            normal = normal.normalized;
        }

        Debug.DrawLine(transform.position, transform.position + normal * 3f, Color.green);

        return didHit;
    }
}