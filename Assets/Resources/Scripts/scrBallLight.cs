using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBallLight : MonoBehaviour
{
    GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("objBall");
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.position = ball.transform.position;
    }
}
