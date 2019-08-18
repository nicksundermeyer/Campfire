using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : InteractableObject {
    public override void Interact () {
        Vector3 newRot = transform.rotation.eulerAngles;
        newRot.y += 45;
        transform.rotation = Quaternion.Euler(newRot);
    }
}