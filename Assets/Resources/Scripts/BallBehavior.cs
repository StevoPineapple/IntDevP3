using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    SpriteRenderer sprRender;
    Rigidbody2D ball;
    public Color colorBBlue;
    public Color colorBOrange;
    public float movePower;
    Vector3 deltaPos;
    Vector3 lastPos;
    int stuckTick;
    int stuckTickBuffer;
    int stuckTickBufferMax;
    int stuckTickMax;
    scrSceneManager sceneMan;
    List<Collider2D> currColList = new List<Collider2D>();
    List<Collider2D> ballColList = new List<Collider2D>();
    void Start()
    {
        if(gameObject.tag == "PegsGreen")
        {
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        }
        Vector3 deltaPos = new Vector3(1,1,1);
        sceneMan = GameObject.Find("objSceneManager").GetComponent<scrSceneManager>();
        stuckTickMax = 200;
        stuckTickBufferMax = 2;
        stuckTickBuffer = stuckTickBufferMax;
        stuckTick = stuckTickMax;
        sprRender = gameObject.GetComponent<SpriteRenderer>();
        ball = gameObject.GetComponent<Rigidbody2D>();
        movePower = 200.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deltaPos = gameObject.transform.position;
    }
    void LateUpdate()
    {
        if(sceneMan.isAiming == false)
        {
            deltaPos = gameObject.transform.position - deltaPos;
            if(Mathf.Abs(deltaPos.x) == 0.0f && Mathf.Abs(deltaPos.y) == 0.0f)
            {
                stuckTickBuffer = stuckTickBufferMax;
                if(stuckTick > 0)
                {
                    stuckTick -= stuckTickBufferMax;
                }
                else
                {
                    //Debug.Log("STUCK");
                    for(int i=0;i<currColList.Count;i++)
                    {
                        if(currColList[i].gameObject.tag == "Pegs" || currColList[i].gameObject.tag == "PegsOrange" )
                        {
                            currColList[i].gameObject.GetComponent<Collider2D>().isTrigger = true;
                            currColList[i].gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                        }
                    }   
                    currColList.Clear();
                    ballColList.Clear();
                }
                //Debug.Log(stuckTick);
            }
            else{
                if(stuckTickBuffer>0)
                    stuckTickBuffer-=1;
                else
                    stuckTick = stuckTickMax;
            }
        }
    }
    bool checkWin()
    {
        GameObject[] orangeArr = GameObject.FindGameObjectsWithTag("PegsOrange");
        foreach(GameObject gmObj in orangeArr)
        {
            if(!gmObj.GetComponent<scrPeg>().isHit)
                return false;
        }
        return true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(!currColList.Contains(other.gameObject.GetComponent<Collider2D>())){
            currColList.Add(other.gameObject.GetComponent<Collider2D>());  
            ballColList.Add(other.gameObject.GetComponent<Collider2D>());  
        }
        if(other.gameObject.tag == "Pegs" && !other.gameObject.GetComponent<scrPeg>().isHit)
        {
            sceneMan.hitCount += 10;
            sceneMan.deltaScore+=sceneMan.hitCount;
            sceneMan.AddTOTextList(other);
            other.gameObject.GetComponent<scrPeg>().isHit = true;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/sprPegsHit")[1];
        }
        else if(other.gameObject.tag == "PegsOrange" && !other.gameObject.GetComponent<scrPeg>().isHit)
        {
            sceneMan.hitCount += 100;
            sceneMan.deltaScore+=sceneMan.hitCount;
            sceneMan.AddTOTextList(other);
            other.gameObject.GetComponent<scrPeg>().isHit = true;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/sprPegsHit")[2];
            sceneMan.isWin = checkWin();
        }
        else if(other.gameObject.tag == "PegsGreen" && !other.gameObject.GetComponent<scrPeg>().isHit)
        {
            sceneMan.hitCount += 100;
            sceneMan.deltaScore+=sceneMan.hitCount;
            sceneMan.AddTOTextList(other);
            other.gameObject.GetComponent<scrPeg>().isHit = true;
            other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = sceneMan.GetComponent<scrSceneManager>().ballGravL;
        }
        else if(other.gameObject.tag == "PegsPurple" && !other.gameObject.GetComponent<scrPeg>().isHit)
        {
            sceneMan.hitCount += 400;
            sceneMan.deltaScore+=sceneMan.hitCount;
            sceneMan.AddTOTextList(other);
            other.gameObject.GetComponent<scrPeg>().isHit = true;
            other.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/sprPegPurpleHit");
            other.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(!currColList.Contains(other.gameObject.GetComponent<Collider2D>())){
            currColList.Remove(other.gameObject.GetComponent<Collider2D>());  
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Trigger")
        {
            if(other.gameObject.GetComponent<scrChangeCam>().toZoom)
            {
                sceneMan.mainCam.camMode = "Zoom";
                Object.Destroy(other);
            }
            else if(other.gameObject.GetComponent<scrChangeCam>().toFull)
            {
                sceneMan.mainCam.camMode = "Full";
                Object.Destroy(other);
            }
        }
        else if(other.gameObject.tag == "ResetPlane"&&gameObject.name == "objBall")
        {
            GameObject objCap = GameObject.Find("objCapsule");
            objCap.GetComponent<scrCapsule>().isResetting = true;
            foreach(GameObject greenPeg in GameObject.FindGameObjectsWithTag("PegsGreen"))
            {
                if(greenPeg.GetComponent<scrPeg>().isHit)
                    Object.Destroy(greenPeg);
            }
            if(!sceneMan.isWin)
                sceneMan.ResetBall();
            foreach(GameObject peg in GameObject.FindGameObjectsWithTag("Pegs"))
            {
                if(peg.GetComponent<scrPeg>().isHit)
                {
                    peg.gameObject.GetComponent<Collider2D>().isTrigger = true;
                    peg.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                }  
            } 
            foreach(GameObject peg in GameObject.FindGameObjectsWithTag("PegsOrange"))
            {
                if(peg.GetComponent<scrPeg>().isHit)
                {
                    peg.gameObject.GetComponent<Collider2D>().isTrigger = true;
                    peg.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                }  
            } 
            foreach(GameObject peg in GameObject.FindGameObjectsWithTag("PegsPurple"))
            {
                if(peg.GetComponent<scrPeg>().isHit)
                {
                    peg.gameObject.GetComponent<Collider2D>().isTrigger = true;
                    peg.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
                }
                else
                    peg.gameObject.GetComponent<scrPeg>().changeToBlue();
            } 
            currColList.Clear();
            ballColList.Clear();
        }
    }
}
