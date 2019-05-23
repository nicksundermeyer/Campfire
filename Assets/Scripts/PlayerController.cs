using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    //movement variable
    public float movementSpeed = 20f;

    // light level checking
    public float minLightValue;
    private LightCheck lightChecker;

    // time allowed to be in darkness
    private float darknessTimer = 0f;
    public float maxDarknessTime;

    public bool printLightLevel;

    public GameObject Torch;
    private Torch torchComponent;

    public GameObject CampFire;
    private FireController campfireComponent;
    
    public int numSticks;

    public bool isSwinging;

    private Animator anim;
    private AudioSource audio;

    public bool isDead;

    private Rigidbody rb;

    private void Awake() {
        numSticks = 0;
        isSwinging = false;
        isDead = false;

        lightChecker = GetComponentInChildren<LightCheck>();
        rb = GetComponent<Rigidbody>();
        campfireComponent = CampFire.GetComponent<FireController>();
        torchComponent = Torch.GetComponent<Torch>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();

        if(printLightLevel)
        {
            Debug.Log("Light Level: " + lightChecker.LightLevel);
        }

        if(lightChecker.LightLevel < minLightValue)
        {
            // Debug.Log("In Darkness: " + lightChecker.LightLevel);

            if(darknessTimer == 0f)
            {
                darknessTimer = Time.time;
            }

            if((Time.time - darknessTimer) > maxDarknessTime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            darknessTimer = 0f;
        }

	}

    //call this to move the player 1 frame
    private void Move()
    {
        float horizAxis = Input.GetAxis("Horizontal");
        float vertAxis = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movementSpeed * Time.deltaTime * horizAxis, rb.velocity.y, movementSpeed * Time.deltaTime * vertAxis);

        rb.velocity = movement;
    }

    private void takeFuelForTorch(int amount)
    {
        if (campfireComponent.Fuel > amount)
        {
            campfireComponent.Fuel -= amount;
            torchComponent.torchFuel += amount;
        }
        else
        {
            Debug.Log("Not Enough Fuel!!");
        }
    }

    void takeFuelFromFire()
    {
        takeFuelForTorch(10);
        Debug.Log("Just took 10 fuel");
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.name == "Campfire")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                campfireComponent.Fuel += numSticks ;
                Debug.Log("Added " + (numSticks) + " to Fire");
                //set sticks to 0
                numSticks = 0;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                takeFuelFromFire();
            }
        }
        else if(col.gameObject.name == "Enemy" || col.gameObject.name == "House")
        {
            isDead = true;
            //Debug.Log("you're dead!");
        }

        foreach(Transform child in transform)
        {
            if(child.name == "Torch")
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    child.transform.parent = null;
                }
            }
        }

    }

    //function to pick up items
    void OnControllColliderHit(ControllerColliderHit hitObj)
    {
        if(hitObj.gameObject.tag == "CanPickUp")
        {

        }
    }
}
