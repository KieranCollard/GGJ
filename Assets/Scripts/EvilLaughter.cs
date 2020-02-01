using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilLaughter : MonoBehaviour
{
    AudioClip sourceClip;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {

        source = GetComponent<AudioSource>();
        Debug.Assert(source != null);
        sourceClip = source.clip;

        Debug.Assert(sourceClip != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Laugh()
    {
        source.PlayOneShot(sourceClip);
    }
}
