using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToSpawn : MonoBehaviour
{
    public GameObject spawnpoint;
    public PickupCarScript pickups;
    public Rigidbody rigid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "OutOfBounds")
            return;

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        gameObject.transform.position = spawnpoint.transform.position;
        gameObject.transform.rotation = spawnpoint.transform.rotation;
        pickups.DropOffBox();
    }
}
