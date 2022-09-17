using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public Rigidbody rb;
    public float length;
    float lastHitDist;
    public float strength;
    public float dampening;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        //Debug.Log(transform.TransformDirection(-Vector3.up));
        Debug.DrawRay(rb.position, transform.TransformDirection(-Vector3.up) * length, Color.green, 2, false);
        if (Physics.Raycast(rb.position, transform.TransformDirection(-Vector3.up), out hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance);
            //Debug.Log("ding");
            rb.AddForceAtPosition(transform.up * forceAmount, transform.position);
        }
        else
        {
            lastHitDist = length * 1.1f;
        }
    }

    private float HooksLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + (dampening * (lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = hitDistance;

        return forceAmount;
    }
}
