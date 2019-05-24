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

    // animation and audio
    private Animator anim;
    private AudioSource audio;

    // component references
    private Rigidbody rb;
    public GameObject pickupPos;

    // collision checking
    private bool touchingCampfire = false;
    private bool pickup = false; // whether we currently have something in our hand
    private GameObject pickupObj = null;

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

    public void Move (InputAction.CallbackContext context) {
        float horizAxis = context.ReadValue<Vector2> ().x;
        float vertAxis = context.ReadValue<Vector2> ().y;

        Vector3 movement = new Vector3 (movementSpeed * Time.deltaTime * horizAxis, rb.velocity.y, movementSpeed * Time.deltaTime * vertAxis);

        rb.velocity = movement;
    }

    public void Interact (InputAction.CallbackContext context) {
        if (touchingCampfire) {
            if (numSticks > 0)
                putFuelInFire ();
            else
                takeFuelFromFire ();
        }

        // drop object if one is currently picked up, pick up if one is nearby
        if (pickup) {
            dropObject ();
        } else if (pickupObj != null) {
            pickupObject ();
        }
    }

    private void pickupObject () {
        // set kinematic and disable collider
        pickupObj.GetComponent<Rigidbody> ().isKinematic = true;
        pickupObj.GetComponent<Collider> ().enabled = false;
        // parent object to pickup position
        pickupObj.transform.parent = pickupPos.transform;
        // set position and rotation
        pickupObj.transform.localPosition = new Vector3 (0, 0, 0);
        pickupObj.transform.localEulerAngles = new Vector3 (0, 0, 0);

        pickup = true;
    }

    private void dropObject () {
        // get currently picked up object
        GameObject currentObj = pickupPos.transform.GetChild (0).gameObject;
        // unparent it
        currentObj.transform.parent = null;
        // set non kinematic and enable collider
        currentObj.GetComponent<Rigidbody> ().isKinematic = false;
        currentObj.GetComponent<Collider> ().enabled = true;

        pickup = false;
    }

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

    private void putFuelInFire () {
        Debug.Log ("Added " + (numSticks) + " to Fire");

        campfireComponent.Fuel += numSticks;
        numSticks = 0;
    }

    // checking for collisions and setting variables
    private void OnCollisionEnter (Collision other) {
        // checking collisions with fire
        touchingCampfire = (other.gameObject.name == "Campfire") ? true : false;

        if (other.gameObject.name == "Enemy" || other.gameObject.name == "House")
            isDead = true;
    }

    private void OnCollisionExit (Collision other) {
        touchingCampfire = false;
    }

    private void OnTriggerEnter (Collider other) {
        pickupObj = (other.gameObject.tag == "Pickup") ? other.gameObject : null;
    }

    private void OnTriggerExit (Collider other) {
        pickupObj = null;
    }
}