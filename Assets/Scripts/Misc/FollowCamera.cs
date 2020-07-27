using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public float speed = 1.5f;

    private Vector3 dest;

    [SerializeField] float y;
    [SerializeField] float z;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dest = new Vector3(player.position.x, player.position.y + y, player.position.z + z);
    }

    void Update()
    {
        dest = new Vector3(player.position.x, player.position.y + y, player.position.z + z);
        transform.position = Vector3.Lerp(transform.position, dest, speed * Time.deltaTime);
    }
}


