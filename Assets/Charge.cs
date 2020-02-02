using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    float scale = 0;
    float rate = 1;
    Vector3 initalScale;
    // Start is called before the first frame update
    void Start()
    {
        initalScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Throw"))
        {
            scale += rate * Time.deltaTime;
            scale = Mathf.Clamp(scale, 0, 1);
            this.transform.localScale = new Vector3(scale, initalScale.y, initalScale.z);
        }
        else
        {
            scale = 0;
            this.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
