using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public GameObject Player;
    public int stickValue = 20;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("You picked up a stick!");
            Player.GetComponent<PlayerController>().numSticks += stickValue;
            Destroy(gameObject);
        }
    }
}
