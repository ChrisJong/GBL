using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class jumpPad : MonoBehaviour
{
    public float jumpHeight = 150.0f;
    private BoxCollider collider;

    void Awake()
    {
        collider = GetComponent<BoxCollider>() as BoxCollider;
        collider.isTrigger = true;
        collider.size = new Vector3(1000.0f, 500.0f, 1000.0f);
        collider.center = new Vector3(0.0f, 300.0f, 0.0f);
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
            obj.GetComponent<Rigidbody>().AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
    }
}