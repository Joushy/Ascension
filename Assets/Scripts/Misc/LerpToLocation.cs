using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToLocation : MonoBehaviour
{

    [SerializeField] Transform startMarker;
    [SerializeField] Transform endMarker;
    [SerializeField] float speed;
    [SerializeField] float fraction;
    [SerializeField] bool waitIsOver;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTwoSeconds());
    }

    // Update is called once per frame
    void Update()
    {
        if (fraction < 2 && waitIsOver)
        {
            fraction += speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fraction);
        }
    }


    IEnumerator WaitTwoSeconds()
    {
        waitIsOver = false;
        yield return new WaitForSeconds(1.5f);
        waitIsOver = true;
    }
}
