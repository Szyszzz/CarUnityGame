using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToSpawn : MonoBehaviour
{
    public GameObject spawnpoint;
    public PickupCarScript pickups;
    public Rigidbody rigid;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "OutOfBounds")
            return;

        Respawn();
    }

    private void Respawn()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        gameObject.transform.position = spawnpoint.transform.position;
        gameObject.transform.rotation = spawnpoint.transform.rotation;
        pickups.DropOffBox();
    }
}
