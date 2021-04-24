using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GrappleBehavior : MonoBehaviour {

    public enum GrappleState {
        InFlight, Tethered, Retracting, Stored, Disabled
    }
    
    public GameObject hook;
    public LineRenderer line;

    public GameObject player;
    public Rigidbody playerRigidbody;
    public Transform shotLoc;
    public GameObject shotView;
    public Vector3 direction = (Vector3.forward + Vector3.up).normalized;
    public float shootSpeed;

    public float hookTime = 2f;
    public float minTether = 2f;
    public float pullSpeed = 15f;
    public float maxTetherDistance = 20f;
    public float retractSpeed = 5f;
    public float minRetract = 0.5f;
    public bool autoPull = false;
    public GrappleState grappleState = GrappleState.Stored;
    public bool disabled = false;
    
    public KeyCode retractKey = KeyCode.R;
    public KeyCode shootKey = KeyCode.R;
    
    private GameObject _activeTether;
    private HookBehavior _behavior;
    public Rigidbody tetherRb;
    private SpringJoint _tSpring;
    private Transform _tetherTf;
    private Collider _tetherCollider;
    
    private float _airTime = 0f;
    private MouseLook mouseLook;
    private float minRetractSqr;
    private Vector3 lastRetractLoc;
    
    // Start is called before the first frame update
    void Start()
    {
        if (disabled) {
            return; 
        }
        if (shotLoc == null) {
            shotLoc = gameObject.transform;
        }

        mouseLook = shotView.GetComponent<MouseLook>();
        line = GetComponent<LineRenderer>();
        minRetractSqr = minRetract * minRetract;
    }

    // Update is called once per frame
    void Update() {
        if (disabled) {
           return; 
        }

        DrawTether();
        

        switch (grappleState) {
            case GrappleState.Stored:
                Stored();
                break;
            case GrappleState.InFlight:
                InFlight();
                break;
            case GrappleState.Tethered:
                Tethered();
                break;
            case GrappleState.Retracting:
                break;
        }
        
    }
    
    void FixedUpdate() {
        if (disabled) {
            return; 
        }
        switch (grappleState) {
            case GrappleState.Retracting:
                Retracting();
                break;
        }

        lastRetractLoc = shotLoc.position;
    }

    void Stored() {
        if (Input.GetKeyDown(shootKey)) {
            Quaternion relativeDir = shotLoc.rotation;

            _activeTether = Instantiate(hook, shotLoc.position, relativeDir);
            tetherRb = _activeTether.GetComponent<Rigidbody>();
            _tetherTf = _activeTether.GetComponent<Transform>();
            _tSpring = _activeTether.GetComponent<SpringJoint>();
            _tetherCollider = _activeTether.GetComponent<Collider>();
            
            Vector3 sDir = mouseLook.looking ? mouseLook.transform.forward : relativeDir * direction;

            tetherRb.velocity = sDir * shootSpeed;
            _tetherTf.position += sDir * 0.5f;

            autoPull = false;
            
            _behavior = _activeTether.AddComponent<HookBehavior>();
            _behavior.grappleBehavior = this;
            grappleState = GrappleState.InFlight;
        }
    }

    void InFlight() {
        if (_behavior.hooked) {
            _tSpring.autoConfigureConnectedAnchor = false;
            _tSpring.connectedAnchor = Vector3.zero;
            _tSpring.connectedBody = playerRigidbody;
            _tSpring.maxDistance = Vector3.Distance(tetherRb.position, playerRigidbody.position);
            
            grappleState = GrappleState.Tethered;
        }
        
        RetractCheck();
        
        if (Vector3.Distance(tetherRb.position, playerRigidbody.position) > maxTetherDistance) {
            Retract();
        }
    }

    void Tethered() {
        if (Input.GetKey(retractKey) || autoPull) {
            Debug.Log("PULLING");
            _tSpring.maxDistance = Mathf.Lerp(_tSpring.maxDistance, minTether, Time.deltaTime * pullSpeed);
        }

        RetractCheck();
    }

    public void Retract() {
        Destroy(_tSpring);
        foreach (var joint in _activeTether.GetComponents<FixedJoint>()) {
            Destroy(joint);
        }
        
        Destroy(tetherRb);
        Destroy(_tetherCollider);
            
        _tetherTf.SetParent(null, true);
        grappleState = GrappleState.Retracting;
    }

    void RetractCheck() {
        if (Input.GetKeyDown(shootKey)) {
            Retract();
        }
    }

    void Retracting() {
        Vector3 movDir = shotLoc.position - lastRetractLoc;
        Vector3 tetherDir = (shotLoc.position - _tetherTf.position).normalized;
        
        _tetherTf.position += tetherDir * Vector3.Dot(movDir, tetherDir);
        _tetherTf.position = Vector3.Lerp(_tetherTf.position, shotLoc.position, Time.deltaTime * retractSpeed);

        if ((_tetherTf.position - shotLoc.position).sqrMagnitude < minRetractSqr) {
            Destroy(_activeTether);
            grappleState = GrappleState.Stored;
        }
    }

    public bool GrappleTaught() {
        if (grappleState == GrappleState.Tethered) {
            float diff = _tSpring.maxDistance - Vector3.Distance(tetherRb.position, playerRigidbody.position);
            return Mathf.Abs(diff) < 1.0f;
        }

        return false;
    }

    void DrawTether() {
        if (grappleState != GrappleState.Disabled && grappleState != GrappleState.Stored) {
            line.enabled = true;
            line.SetPosition(0, shotLoc.position); 
            line.SetPosition(1, _activeTether.transform.position);
        } else {
            line.enabled = false;
        }
    }
}
