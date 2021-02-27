using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    public BoxCollider2D worldBounds;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float camX;
    float camY;
    float camXFull;
    float camYFull;
    float camWidth;
    float camSizeFull;
    float camSizeZoom;
    float camChangeSize;
    float camChangeSpeed;
    Vector3 smoothPos;
    public float smoothRate;
    Camera mainCam;

    public string camMode;
    void Start()
    {
        camXFull = gameObject.transform.position.x;
        camYFull = gameObject.transform.position.y;
        camChangeSize = 9999.0f; //Temp value
        camChangeSpeed = 100.0f;
        followTransform = GameObject.Find("objBall").transform;
        worldBounds = GameObject.Find("World").GetComponent<BoxCollider2D>();
        camMode = "Full";
        xMin = worldBounds.bounds.min.x;
        yMin = worldBounds.bounds.min.y;
        xMax = worldBounds.bounds.max.x;
        yMax = worldBounds.bounds.max.y;
        mainCam = gameObject.GetComponent<Camera>();
        camSizeFull = mainCam.orthographicSize;
        camSizeZoom = 1.25f;
        //camRatio = (xMax+camSizeZoom)/800000000.0f;
        camWidth = mainCam.aspect*camSizeZoom;
        smoothRate = 0.2f;
    }

    void Update()
    {

    }
    
    void FixedUpdate()
    {
        //camSizeZoom = followTransform.position.y;
        //mainCam.orthographicSize = camSizeZoom;
        //camRatio = (xMax+camSizeFull)/800000000.0f;
        switch(camMode){
            case "Zoom":
            { 
                if(camChangeSize == 9999.0f)//ZOOM TO ZOOM
                    camChangeSize = (mainCam.orthographicSize-camSizeZoom)/camChangeSpeed;
                if(mainCam.orthographicSize>camSizeZoom)
                    mainCam.orthographicSize-=camChangeSize;
                else
                {
                    camChangeSize = 9999.0f;
                    mainCam.orthographicSize = camSizeZoom;
                }
                camY = Mathf.Clamp(followTransform.position.y,yMin+camSizeZoom,yMax-camSizeZoom);
                camX = Mathf.Clamp(followTransform.position.x,xMin+camWidth,xMax-camWidth);
                smoothPos = Vector3.Lerp(gameObject.transform.position,new Vector3(camX,camY,gameObject.transform.position.z),smoothRate);
                gameObject.transform.position = smoothPos;
                break;
            }
            case "Full":
            {
                if(camChangeSize == 9999.0f)//ZOOM TO FULL this is not efficient
                    camChangeSize = (camSizeFull-mainCam.orthographicSize)/camChangeSpeed;
                if(mainCam.orthographicSize<camSizeFull)
                {
                    mainCam.orthographicSize+=camChangeSize;
                    camWidth = mainCam.aspect*mainCam.orthographicSize;
                    camY = Mathf.Clamp(followTransform.position.y,yMin+mainCam.orthographicSize,yMax-mainCam.orthographicSize);
                    camX = Mathf.Clamp(followTransform.position.x,xMin+camWidth,xMax-camWidth);
                    smoothPos = Vector3.Lerp(gameObject.transform.position,new Vector3(camX,camY,gameObject.transform.position.z),smoothRate);
                    gameObject.transform.position = smoothPos;
                }
                else
                {
                    gameObject.transform.position = new Vector3(camXFull,camYFull,gameObject.transform.position.z);
                    camChangeSize = 9999.0f;
                    mainCam.orthographicSize = camSizeFull;
                }
            }
            break;
        }
        
    }
}
