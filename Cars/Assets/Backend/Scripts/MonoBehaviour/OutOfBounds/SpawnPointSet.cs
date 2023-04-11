using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSet : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private Vector3 pos;
    [SerializeField] private Quaternion rot;

    void Start()
    {
        pos = player.transform.position;
        rot = player.transform.rotation;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rot;
    }
}
