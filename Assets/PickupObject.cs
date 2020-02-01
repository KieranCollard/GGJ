using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    bool inRange;
    Rigidbody rb;
    Transform parent;
    float upModifier = 2;
    float forwardModifier = 1;
    float playerVelocityModifier = 2;
    float objectMass;

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pickup(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;

        rb.freezeRotation = true;
        rb.detectCollisions = false;
        rb.isKinematic = true;
    }

    // We want to pass the force here if we want to have any sort of throw power.
    public void Throw(float throwPower, float maxThrowCharge)
    {
        Vector3 forceDirection = transform.parent.up + transform.parent.forward;
        Rigidbody playerRb = transform.parent.GetComponentInParent<Rigidbody>();

        Vector3 upForce = transform.parent.up * upModifier * maxThrowCharge * throwPower;
        Vector3 forwardForce = transform.parent.forward * forwardModifier * maxThrowCharge * throwPower;
        Vector3 impartedForce = playerRb.velocity * playerVelocityModifier;
        Vector3 force = upForce + forwardForce + impartedForce;

        transform.parent = null;
        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.detectCollisions = true;
        rb.AddForce(force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }
}
