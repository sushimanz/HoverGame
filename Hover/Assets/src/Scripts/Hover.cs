using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public Rigidbody rb;
    public float length;
    public float strength;
    public float dampening;
    public float thrust;
    public float rotSpeed;
    public float slide;
    public float snap;
    float velocity;
    float lastHitDist;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForceAtPosition(-rb.transform.up * strength, rb.transform.position);
        RaycastHit hit;
        Debug.DrawRay(rb.position, rb.transform.TransformDirection(-Vector3.up) * length, Color.green, 2, false);
        if (Physics.Raycast(rb.position, -rb.transform.up, out hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance * 3);
            rb.AddForceAtPosition(rb.transform.up * forceAmount, rb.transform.position);
        }
        else
        {
            lastHitDist = length * 1.1f;
        }
        Physics.Raycast(rb.position, -rb.transform.up, out hit, length);
        rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, Quaternion.FromToRotation(rb.transform.up, hit.normal) * rb.transform.rotation, snap);
        rb.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * rotSpeed, 0), Space.Self);
        velocity = Mathf.Lerp(velocity, thrust * Input.GetAxis("Vertical"), slide);
        rb.transform.position += velocity * rb.transform.forward;
        //transform.position.Lerp(transform.position);
        //transform.rotation = transform.rotation * Quaternion.Euler(0, Input.GetAxis("Horizontal") * 10, 0);
    }

    private float HooksLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + (dampening * (lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = hitDistance;

        return forceAmount;
    }
}
