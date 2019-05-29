using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    //movement variable
    public float movementSpeed = 20f;

    // light detection variables
    public bool printLightLevel;
    public float minLightValue;
    public float maxDarknessTime;
    private LightCheck lightChecker;
    private float darknessTimer = 0f;

    // GameObject references
    public GameObject Torch;
    private Torch torchComponent;
    public GameObject CampFire;
    private FireController campfireComponent;

    // editor variables
    public int numSticks = 0;
    public bool isSwinging = false;
    public bool isDead = false;
    public bool canDropTorch = true;

    // animation and audio
    private Animator anim;
    private AudioSource audio;

    // component references
    private Rigidbody rb;
    public GameObject pickupPos;

    // collision checking
    private bool touchingCampfire = false;
    private GameObject touchingSconce = null;
    private GameObject currentPickup = null; // whether we currently have something in our hand
    private GameObject pickupObj = null; // potential nearby object to be picked up
   

    private void Awake () {
        lightChecker = GetComponentInChildren<LightCheck> ();
        rb = GetComponent<Rigidbody> ();
        campfireComponent = CampFire.GetComponent<FireController> ();
        torchComponent = Torch.GetComponent<Torch> ();
        anim = GetComponent<Animator> ();
        audio = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update () {
        if (printLightLevel) {
            Debug.Log ("Light Level: " + lightChecker.LightLevel);
        }

        if (lightChecker.LightLevel < minLightValue) {
            // Debug.Log("In Darkness: " + lightChecker.LightLevel);

            if (darknessTimer == 0f) {
                darknessTimer = Time.time;
            }

            if ((Time.time - darknessTimer) > maxDarknessTime) {
                SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
            }
        } else {
            darknessTimer = 0f;
        }

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
        if (touchingCampfire) {
            if (numSticks > 0)
                putFuelInFire ();
            else
                takeFuelFromFire ();
        }

        // if next to a sconce and currently holding something, put it in the sconce
        if (touchingSconce && currentPickup) {
            currentPickup.transform.parent = touchingSconce.transform;
            currentPickup.transform.localPosition = new Vector3 (0, 0, 0);
            currentPickup = null;
            Debug.Log("this is colliding");
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

    /// <summary>
    /// Take fuel out of the campfire
    /// </summary>
    private void takeFuelFromFire () {
        Debug.Log ("Just took 10 fuel");
        int amount = 10;

        if (campfireComponent.Fuel > amount) {
            campfireComponent.Fuel -= amount;
            torchComponent.torchFuel += amount;
        } else {
            Debug.Log ("Not Enough Fuel!!");
        }
    }

    /// <summary>
    /// Put all currently held fuel into the campfire
    /// </summary>
    private void putFuelInFire () {
        Debug.Log ("Added " + (numSticks) + " to Fire");

        campfireComponent.Fuel += numSticks;
        numSticks = 0;
    }

    // Collision checking functions

    private void OnCollisionEnter (Collision other) {
        // checking collisions with fire and sconces
        touchingCampfire = (other.gameObject.name == "Campfire") ? true : false;

        if (other.gameObject.name == "Enemy" || other.gameObject.name == "House")
            isDead = true;
    }

    private void OnCollisionExit (Collision other) {
        touchingCampfire = false;
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