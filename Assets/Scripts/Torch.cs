using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject CampFire;
    public Light Light;
    private GameObject[] Enemies;
    public float MaxRange;
    public float torchFuel = 100f;
    private float torchScale;
    public float torchBurnRate = 10f;
    private float Range;
    public float rangeScale;


    void Start()
    {
        //here, the torch should get set to 0 so it is unlit at the beginning of the game, 
        //but not for now cuz of testing and whatnot
        // StartCoroutine("oneSecPrint");
        // Enemies = GameObject.FindGameObjectsWithTag("Enemies");
    }

    void Update()
    {
        if(torchFuel > 0)
        {
            burnFuel();
        }

    }

    IEnumerator oneSecPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            OutputFuel();
        }
    }

    void burnFuel()
    {
        torchFuel -= torchBurnRate * Time.deltaTime;

        // float closestEnemy = 100;
        // for(int i=0; i<Enemies.Length; i++)
        // {
        //     float dist = Vector3.Distance(this.transform.position, Enemies[i].transform.position);
        //     if(dist < closestEnemy)
        //     {
        //         closestEnemy = dist;
        //     }
        // }

        // if(closestEnemy*rangeScale < MaxRange)
        // {
        //     torchScale = Mathf.Clamp(torchFuel, 0, closestEnemy*rangeScale);
        // }
        // else
        // {
        //     torchScale = Mathf.Clamp(torchFuel, 0, MaxRange);
        // }
        // Light.transform.localScale = new Vector3(torchScale, torchScale, torchScale);
        torchScale = Mathf.Clamp(torchFuel, 0, MaxRange);
        Light.range = torchScale;
    }

    //for testing purposes only
    void OutputFuel()
    {
        Debug.Log("TORCH :: Remaining Torch Fuel: " + torchFuel);
    }
}
