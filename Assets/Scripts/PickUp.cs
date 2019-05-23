using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //player's transform
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        //when player collides with this object
        if(col.gameObject.tag == "Player")
        {
            //Debug.Log("FUCK");
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("ya pressed f");
                //make parent transform the player
                transform.parent = player.transform;
                //center transform on player center (we can make the adjustable)
                transform.localPosition = new Vector3(0, 0, 0);
                //I think do something with Rigidbody here
                //Rigidbody.isKinematic = true;
            }
        }
    }
}
