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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(countDownText != null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        countDownTime -= Time.deltaTime;
        countDownText.text = countDownTime.ToString("0"); 
        if(countDownTime < 0)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
            foreach (Collider col in colliders)
            {
                Rigidbody rig = col.GetComponent<Rigidbody>();
                if (rig != null)
                {
                    rig.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
                }
            }
            countDownText.enabled = false;
            Destroy(this.gameObject);
        }
    }

    IEnumerator CountDown()
    {
        while (countDownTime >0)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            Debug.Log("wait ended");
            countDownTime--;
        }

      
    }
}
