using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public int points;
    public float mass;
    [SerializeField] private PickupCarScript carListener;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            carListener.PickupBox(mass, points);
            Destroy(gameObject);
        }
    }
}
