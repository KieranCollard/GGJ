using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raise : MonoBehaviour
{
    public int heightToClimb =11;
    public GameObject countDownObject;
    CountDownDisplay countDown;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        countDown = countDown.GetComponent<CountDownDisplay>();
        Debug.Assert(countDown != null);
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //float pos = Mathf.Lerp()
    }
}
