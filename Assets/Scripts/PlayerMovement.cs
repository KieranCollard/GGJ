using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    Rigidbody body = null;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        Debug.Assert(body != null);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float x = horizontal * movementSpeed * Time.deltaTime;
        float z = vertical * movementSpeed * Time.deltaTime;
        Vector3 newPos = new Vector3(body.position.x + x, body.position.y, body.position.z + z);
        body.MovePosition(newPos);
    }
}