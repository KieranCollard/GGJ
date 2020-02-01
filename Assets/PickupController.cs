using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private List<GameObject> pickupsInRange;

    [SerializeField]
    private float chargeTime = 1.0f;

    [SerializeField]
    private float maxThrowPower = 250.0f;

    [SerializeField]
    private Transform pickupPoint;
    GameObject pickedUpObject;

    bool buildCharge = false;
    float throwPower = 0;
    float minThrowPower = 0.5f;
    bool hasPickup;

    // Start is called before the first frame update
    void Start()
    {
        pickupsInRange = new List<GameObject>();
        hasPickup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pickup"))
        {
            PickupObject();
        }
        else if (Input.GetButtonDown("Throw"))
        {
            buildCharge = true;
        }
        else if (Input.GetButtonUp("Throw"))
        {
            buildCharge = false;
            ThrowObject();
        }

        if (buildCharge)
        {
            BuildCharge();
        }

        
    }

    private void BuildCharge()
    {
        float dt = Time.deltaTime;
        float chargeRate = 1 / chargeTime;
        float chargeAmount = dt * chargeRate;

        throwPower += chargeAmount;
        throwPower = Mathf.Min(throwPower, 1);
    }

    void PickupObject()
    {
        if (hasPickup || pickupsInRange.Count == 0)
        {
            return;
        }

        int rand = UnityEngine.Random.Range(0, pickupsInRange.Count);
        pickedUpObject = pickupsInRange[rand];
        pickupsInRange.Remove(pickedUpObject);

        PickupObject pickup = pickedUpObject.GetComponentInParent<PickupObject>();

        hasPickup = true;
        pickup.Pickup(pickupPoint);
    }

    void ThrowObject()
    {
        if (!hasPickup)
        {
            return;
        }

        float actualThrowPower = Mathf.Max(minThrowPower, throwPower);
        PickupObject pickup = pickedUpObject.GetComponentInParent<PickupObject>();
        pickup.Throw(actualThrowPower, maxThrowPower);
        throwPower = 0;
        pickedUpObject = null;
        hasPickup = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            pickupsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pickup")
        {
            GameObject collidingObject = other.gameObject;
            for (int i = 0; i < pickupsInRange.Count; i++)
            {
                GameObject go = pickupsInRange[i];
                if (go == collidingObject)
                {
                    pickupsInRange.Remove(go);
                }
            }
        }
    }
}
