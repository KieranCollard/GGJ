using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CountDownExplosion : MonoBehaviour
{
    public Text countDownText;
    public float countDownTime;
    public float explosionForce;
    public float explosionRadius;
    public GameObject gameTimer;
    public GameObject explosionEffect;
    CountDownDisplay displayTimer;
    bool active = false;
    bool played = false;
    public void StartTimer()
    {
        active = true;
    }
    AudioSource audio;
    // Start is called before the first frame update
    private void Awake()
    {
        explosionEffect.SetActive(false);
    }
    void Start()
    {
        Debug.Assert(countDownText != null);
        Debug.Assert(gameTimer != null);
        displayTimer = gameTimer.GetComponent<CountDownDisplay>();
        Debug.Assert(displayTimer != null);
        audio = GetComponent<AudioSource>();
        Debug.Assert(audio != null);
        Debug.Assert(explosionEffect != null);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active == false)
        {
            return;
        }
        countDownTime -= Time.deltaTime;
        countDownText.text = countDownTime.ToString("0"); 
        if(countDownTime < 0)
        {  
            countDownText.enabled = false;
            if (played == false)
            {
                played = true;
                explosionEffect.SetActive(true);
                StartCoroutine(audioBeforeDestroy());
            }
        }
    }

    IEnumerator audioBeforeDestroy()
    {

        audio.PlayOneShot(audio.clip);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        
        foreach (Collider col in colliders)
        {
            Rigidbody rig = col.GetComponent<Rigidbody>();
            if (rig != null)
            {
                rig.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
            }
        }

        Animator anim = explosionEffect.GetComponentInChildren<Animator>();
        bool donePlaying = anim.GetCurrentAnimatorStateInfo(0).tagHash == 0;  
        while (audio.isPlaying == true || donePlaying == false)
        {
            yield return null;
        }
        displayTimer.TurnOn();
        Destroy(this.gameObject);
    }
}
