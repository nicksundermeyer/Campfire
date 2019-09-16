using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    //movement variables
    public float movementSpeed = 20f;
    public float rotationSpeed = 20f;
    private float tempRotationSpeed;

    private float horizAxis = 0f;
    private float vertAxis = 0f;

    // GameObject references
    public GameObject Torch;
    private Torch torchComponent;
    public GameObject CampFire;
    private FireController campfireComponent;

    // editor variables
    public bool canDropTorch = true;

    // animation and audio
    private Animator anim;
    private AudioSource audio;

    // component references
    private Rigidbody rb;
    public GameObject pickupPos;

    // collision checking
    private GameObject touchingSconce = null;
    private GameObject currentPickup = null; // whether we currently have something in our hand
    private GameObject pickupObj = null; // potential nearby object to be picked up
    private InteractableObject touchingInteractable = null; // if touching an interactable object

    public InputActionAsset inputBindings;
    private InputAction move;
    private InputAction interact;

    public bool gamePad = false;

    private void Awake () {
        rb = GetComponent<Rigidbody> ();
        campfireComponent = CampFire.GetComponent<FireController> ();
        torchComponent = Torch.GetComponent<Torch> ();
        anim = GetComponent<Animator> ();
        audio = GetComponent<AudioSource> ();
        tempRotationSpeed = rotationSpeed;

        inputBindings.Enable ();
        move = inputBindings.FindAction ("Move");
        interact = inputBindings.FindAction ("Interact");
    }

    private void FixedUpdate () {
        // rotate player to face mouse position
        if (gamePad) {
            // rotate player to face gamepad right stick position
            Vector2 rightStick = Gamepad.current.rightStick.ReadValue ();
            Vector3 rotation = new Vector3 (rightStick.x, 0f, rightStick.y);

            rb.rotation = Quaternion.LookRotation (rotation);
        } else {
            Ray ray = Camera.main.ScreenPointToRay (Mouse.current.position.ReadValue ());
            RaycastHit hit = new RaycastHit ();
            if (Physics.Raycast (ray.origin, ray.direction, out hit)) {
                Vector3 target = hit.point - rb.position;
                target.y = 0.0f;
                rb.rotation = Quaternion.Slerp (rb.rotation, Quaternion.LookRotation (target), tempRotationSpeed * Time.deltaTime);
            }
        }

        // player movment
        Vector3 movement = new Vector3 (movementSpeed * Time.deltaTime * horizAxis, rb.velocity.y, movementSpeed * Time.deltaTime * vertAxis);

        rb.velocity = movement;
    }

    private void Update () {
        var moveVal = move.ReadValue<Vector2> ();
        horizAxis = moveVal.x;
        vertAxis = moveVal.y;

        if (interact.triggered) {
            if (touchingInteractable) {
                touchingInteractable.Interact ();
            }

            checkPickup ();
        }
    }

    /// <summary>
    /// Check if object should be picked up or put down
    /// </summary>
    private void checkPickup () {
        // if next to a sconce and currently holding something, put it in the sconce
        if (touchingSconce && currentPickup) {
            currentPickup.transform.parent = touchingSconce.transform;
            currentPickup.transform.localPosition = new Vector3 (0, 0, 0);
            currentPickup = null;
            Debug.Log ("this is colliding");
        }

        // drop object if one is currently picked up, pick up if one is nearby
        if (currentPickup) {
            if (currentPickup.name != "Torch") {
                dropObject ();
            } else if (canDropTorch) {
                dropObject ();
            }
        } else if (pickupObj) { // if there is an object nearby that can be picked up
            pickupObject ();
        }
    }

    /// <summary>
    /// Called to pick up an object and parent it to the player
    /// </summary>
    private void pickupObject () {
        currentPickup = pickupObj;

        currentPickup.GetComponent<Rigidbody> ().isKinematic = true;

        currentPickup.transform.parent = pickupPos.transform;

        if (pickupObj.tag == "Drag") {
            tempRotationSpeed = rotationSpeed / 4;
        } else {
            currentPickup.GetComponent<Collider> ().enabled = false;

            // set position and rotation
            currentPickup.transform.localPosition = new Vector3 (0, 0, 0);
            currentPickup.transform.localEulerAngles = new Vector3 (0, 0, 0);
        }

    }

    /// <summary>
    /// Called to drop object and unparent it from player
    /// </summary>
    private void dropObject () {
        tempRotationSpeed = rotationSpeed;
        // unparent currently picked up object
        currentPickup.transform.parent = null;
        // set non kinematic and enable collider
        currentPickup.GetComponent<Rigidbody> ().isKinematic = false;
        currentPickup.GetComponent<Collider> ().enabled = true;

        currentPickup = null;
    }

    // Collision checking functions

    private void OnCollisionEnter (Collision other) {
        // checking collisions with interactable objects
        touchingInteractable = (other.gameObject.GetComponent<InteractableObject> () != null) ? other.gameObject.GetComponent<InteractableObject> () : null;
    }

    private void OnCollisionExit (Collision other) {
        touchingInteractable = null;
    }

    private void OnTriggerEnter (Collider other) {
        pickupObj = (other.gameObject.tag == "Pickup" || other.gameObject.tag == "Drag") ? other.gameObject : null;
        Debug.Log (pickupObj);
        touchingSconce = (other.gameObject.name == "Sconce") ? other.gameObject : null;
    }

    private void OnTriggerExit (Collider other) {
        pickupObj = null;
        touchingSconce = null;
    }
}