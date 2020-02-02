using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nootsounds : MonoBehaviour
{
    AudioClip clip;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        Debug.Assert(source != null);
        clip = source.clip;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonUp("Throw"))
        {
            source.PlayOneShot(clip);
        }*/


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            source.PlayOneShot(clip);
        }
    }
}
