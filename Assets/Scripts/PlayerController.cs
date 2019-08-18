using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    //movement variable
    public float movementSpeed = 20f;

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
    public InteractableObject touchingInteractable = null; // if touching an interactable object

    public GameObject test;

    private void Awake () {
        rb = GetComponent<Rigidbody> ();
        campfireComponent = CampFire.GetComponent<FireController> ();
        torchComponent = Torch.GetComponent<Torch> ();
        anim = GetComponent<Animator> ();
        audio = GetComponent<AudioSource> ();
    }

    /// <summary>
    /// Called on move input
    /// </summary>
    /// <param name="context">InputAction context information</param>
    public void Move (InputAction.CallbackContext context) {
        float horizAxis = context.ReadValue<Vector2> ().x;
        float vertAxis = context.ReadValue<Vector2> ().y;

        Vector3 movement = new Vector3 (movementSpeed * Time.deltaTime * horizAxis, rb.velocity.y, movementSpeed * Time.deltaTime * vertAxis);

        rb.velocity = movement;
    }

    /// <summary>
    /// called on interact button input
    /// </summary>
    /// <param name="context">InputAction context information</param>
    public void Interact (InputAction.CallbackContext context) {
        // Debug.Log(context.interaction);
        if (context.interaction is UnityEngine.InputSystem.Interactions.PressInteraction) {
            if(touchingInteractable) {
                touchingInteractable.Interact();
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
        // set kinematic and disable collider
        currentPickup.GetComponent<Rigidbody> ().isKinematic = true;
        currentPickup.GetComponent<Collider> ().enabled = false;
        // parent object to pickup position
        currentPickup.transform.parent = pickupPos.transform;
        // set position and rotation
        currentPickup.transform.localPosition = new Vector3 (0, 0, 0);
        currentPickup.transform.localEulerAngles = new Vector3 (0, 0, 0);
    }

    /// <summary>
    /// Called to drop object and unparent it from player
    /// </summary>
    private void dropObject () {
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
        pickupObj = (other.gameObject.tag == "Pickup") ? other.gameObject : null;
        touchingSconce = (other.gameObject.name == "Sconce") ? other.gameObject : null;
    }

    private void OnTriggerExit (Collider other) {
        pickupObj = null;
        touchingSconce = null;
    }
}