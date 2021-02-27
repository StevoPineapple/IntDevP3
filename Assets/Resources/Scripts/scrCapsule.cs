using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrCapsule : MonoBehaviour
{
    Vector3 selfPos;
    float selfAngle;
    public float deltaAngle;
    Vector3 mousePos;
    Camera mainCam;
    public GameObject ball;
    Rigidbody2D ballBody;
    GameObject ballLight;
    public float shotForce;
    scrSceneManager sceneMan;
    public bool isAiming;
    public bool isResetting;
    // Start is called before the first frame update
    void Start()
    {
        sceneMan = GameObject.Find("objSceneManager").GetComponent<scrSceneManager>();
        //Debug.Log(sceneMan.currSceneName);
        shotForce = 175.0f;
        mainCam = Camera.main;
        isAiming = true;
        ball = GameObject.Find("objBall");
        ballBody = ball.GetComponent<Rigidbody2D>();
        ballLight = GameObject.Find("objBallLight");
        GameObject.Find("objTextBalls").GetComponent<Text>().text = 
                    sceneMan.ballNum.ToString() + " Balls Left!";
        //selfPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(sceneMan.currSceneName);
        if(!isResetting)
        {
            pointToMouse();
            if(isAiming)
            {
                ballBody.bodyType = RigidbodyType2D.Kinematic;
                ballBody.gravityScale = 0.0f;
                ball.transform.RotateAround(gameObject.transform.position,Vector3.forward,deltaAngle);     
                if(Input.GetMouseButtonDown(0)||Input.GetKey(KeyCode.Space))
                {
                    isAiming = false;
                    sceneMan.isAiming = false;
                    sceneMan.ballNum-=1;
                    GameObject.Find("objTextBalls").GetComponent<Text>().text = 
                    sceneMan.ballNum.ToString() + " Balls Left!";
                    ballBody.bodyType = RigidbodyType2D.Dynamic;
                    if(sceneMan.currSceneName == "Level")
                        ballBody.gravityScale = sceneMan.ballGravL;
                    else
                        ballBody.gravityScale = sceneMan.ballGrav;
                    ballBody.AddForce(new Vector2(-selfPos.x+mousePos.x,-selfPos.y+mousePos.y)*shotForce);
                        if(sceneMan.currSceneName == "Machine")
                            sceneMan.mainCam.camMode = "Zoom";
                }
            }
            
        }
        else
        {
            ballBody.gravityScale = 0.0f;
            ball.transform.position = new Vector3(0.05f,1.12f,0.0f);
            ballBody.bodyType = RigidbodyType2D.Static;
            pointToMouse();
            //gameObject.transform.rotation = Quaternion.identity;
            ball.transform.RotateAround(gameObject.transform.position,Vector3.forward,selfAngle);     
        }
        ballLight.transform.position = ball.transform.position;
    }

    void pointToMouse()
    {
        deltaAngle = selfAngle;
        selfPos = transform.position;
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        selfAngle = Mathf.Clamp(getAngle(selfPos.y-mousePos.y,selfPos.x-mousePos.x)-90.0f,-87.0f,87.0f);
        transform.rotation = Quaternion.Euler(0,0,selfAngle);
        deltaAngle = -deltaAngle + selfAngle;
        //Debug.Log(deltaAngle);
    }
    public float getAngle(float oppLen, float adjLen)
    {
        return(Mathf.Rad2Deg*Mathf.Atan2(oppLen,adjLen));
    }
}
