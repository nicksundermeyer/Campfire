using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //global variables
    public int numSticks = 0;
    public int numKeys = 0;

    // public references
    public Torch torch;
    public FireController campfire;
    public PlayerController player;

    // implementing singleton pattern
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake () {
        if (_instance != null && _instance != this) {
            Destroy (this.gameObject);
        } else {
            _instance = this;
        }
    }
}