using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject ship;
    public float length;
    public float strength;
    public float dampening;
    public float thrust;
    public float rotSpeed;
    public float slide;
    public float snap;
    public float smoothing;
    public float maxTilt;
    public float tiltSpeed;
    float currentTilt;
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
        rb.velocity = velocity * rb.transform.forward;
        ship.transform.position = rb.transform.position;
        ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, rb.transform.rotation, smoothing);
        ship.transform.rotation *= Quaternion.Euler(0, 0, Mathf.Lerp(currentTilt, -maxTilt * Input.GetAxis("Horizontal"), tiltSpeed));
        //rb.AddForceAtPosition(rb.transform.forward * velocity, rb.transform.position);
        //transform.position.Lerp(transform.position);
    }

    private float HooksLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + (dampening * (lastHitDist - hitDistance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = hitDistance;

        return forceAmount;
    }
}
