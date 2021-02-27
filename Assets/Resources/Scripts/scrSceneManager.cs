using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scrSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAiming;
    public bool isResetting;
    public bool isWin;
    bool initDone;
    public int ballNum;
    public int resetTime;
    public int resetTimeMax;
    public float ballGrav;
    public float ballGravL;
    public GameObject[] Pegs;
    GameObject capsule;
    public string currSceneName;
    public CameraFollow mainCam;
    int pegLen; 
    public int hitCount;
    public int score;
    public int deltaScore;
    public List<Text> scoreTextList;
    GameObject resetText;
    //int numOfScoreText;
    //int numOfScoreTextMax;
    void Start()
    {
        Debug.Log("Start");
        initDone = false;
        resetTimeMax = 200;
        resetTime = resetTimeMax;
        GameObject[] goArr = GameObject.FindGameObjectsWithTag("ScoreText");
        resetText = GameObject.Find("ScoreTextReset");
        int i = 0;
        for(i = 0; i<goArr.Length;i++) {
             scoreTextList.Add (goArr[i].gameObject.GetComponent<Text>());
        }
        //numOfScoreTextMax = goArr.Length;
        //numOfScoreText = 0;
        hitCount = 0;
        score = 0;
        deltaScore = 0;
        mainCam = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        isAiming = true;
        ballGrav = 1.2f;
        ballGravL = 1.0f;
        capsule = GameObject.Find("objCapsule");
        currSceneName = SceneManager.GetActiveScene().name;
        GameObject.Find("objTextLevel").GetComponent<Text>().text = currSceneName;
        if(currSceneName == "Level")
        {
            ballNum = 10;
            InitOrange();
        }
        else
            ballNum = 1;

        /*Pegs = GameObject.FindGameObjectsWithTag("PegsOrange");
        foreach(GameObject peg in Pegs)
            peg.tag = "Pegs";
        Pegs = GameObject.FindGameObjectsWithTag("PegGreen");
        foreach(GameObject peg in Pegs)
            peg.tag = "Pegs";*/

    }

    // Update is called once per frame
    void Update() //could optimize
    {
        if(Input.GetKey(KeyCode.R))
        {
            Debug.Log("ReStart");
            currSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currSceneName);
            //isAiming = true;
            //initDone = false;
            //capsule.GetComponent<scrCapsule>().isAiming = true;
            //mainCam.camMode = "Full";
        }
        else if(Input.GetKey(KeyCode.M))
        {
            SceneManager.LoadScene("Machine");
            currSceneName = SceneManager.GetActiveScene().name;
            isAiming = true;
            capsule.GetComponent<scrCapsule>().isAiming = true;
            mainCam.camMode = "Full";
        }
        else if(Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene("Level");
            currSceneName = SceneManager.GetActiveScene().name;
            isAiming = true;
            capsule.GetComponent<scrCapsule>().isAiming = true;
            mainCam.camMode = "Full";
            if(currSceneName == "Level")
                InitOrange();
        }
    }

    void FixedUpdate()
    {
        if(!isWin)
        {
            if(isResetting)
            {
                if(resetTime>0){
                    resetTime-=1;
                    resetText.gameObject.transform.position = new Vector3 (0.0f,0.0f,0.0f);
                    resetText.GetComponent<Text>().text = "Ball High Score:\n"+hitCount.ToString();
                }
                else{
                    isAiming = true;
                    GameObject.Find("objCapsule").GetComponent<scrCapsule>().isAiming = true;
                    GameObject.Find("objCapsule").GetComponent<scrCapsule>().isResetting = false;
                    GameObject.Find("objCapsule").GetComponent<scrCapsule>().deltaAngle = 0.0f;
                    resetText.gameObject.transform.position = new Vector3 (-20.0f,0.0f,0.0f);
                    isResetting = false;
                    resetTime = resetTimeMax;
                    hitCount = 0;
                }
            }
        }
        else{
            resetText.gameObject.transform.position = new Vector3 (0.0f,0.0f,0.0f);
            resetText.GetComponent<Text>().text = "You WIn~!:))))\n"+"final score:\n"+score.ToString();
        }
        if(deltaScore>0)
        {
            score+=10;
            deltaScore-=10;
            float tempScale = 0.15f*(1.0f+score/70000.0f);
            GameObject.Find("objTextScore").GetComponent<Text>().text = "Score: " + score.ToString();
            GameObject.Find("objTextScore").GetComponent<Text>().transform.localScale = 
            new Vector3(tempScale,tempScale,1.0f);
        }
    }
    void InitOrange()
    {
        if(!initDone){
            Debug.Log("InitOrange");
            
        Pegs = GameObject.FindGameObjectsWithTag("Pegs");
        Debug.Log("PegsLen "+Pegs.Length);
        Debug.Log("Modifying:");
        int orangeLen = Pegs.Length/5;
        int orangeIndex = Random.Range(0,Pegs.Length-1);
        for(int i = 0; i<orangeLen; i++)
        {
            Debug.Log(Pegs[orangeIndex].name);
            Pegs[orangeIndex].tag = "PegsOrange";
            Pegs[orangeIndex].GetComponent<scrPeg>().changeToOrange();
            Pegs = GameObject.FindGameObjectsWithTag("Pegs");
            orangeIndex = Random.Range(0,Pegs.Length-1);
        }
        if(currSceneName == "Level"){ ////////////INITGREEN
            Pegs = GameObject.FindGameObjectsWithTag("Pegs");
            orangeIndex = Random.Range(0,Pegs.Length-1);
            for(int i = 0; i<2; i++)
            {
                Pegs[orangeIndex].tag = "PegsGreen";
                Pegs[orangeIndex].GetComponent<scrPeg>().changeToGreen();
                Pegs = GameObject.FindGameObjectsWithTag("Pegs");
                orangeIndex = Random.Range(0,Pegs.Length-1);
            }
        }
        initDone = true;
        Debug.Log("PegsLen "+Pegs.Length);

        }
    }
    public void AddTOTextList(Collision2D peg) //For score
    {
        InsertToScoreTextList(scoreTextList,peg,hitCount.ToString());    
        /*if(numOfScoreText<numOfScoreTextMax)
        {
            //scoreTextPosList.Insert(0,peg.transform.transform.position);
            scoreTextList[numOfScoreText].transform.position =
            new Vector3(peg.transform.position.x,peg.transform.position.y+0.08f,peg.transform.position.z);
            //scoreTextList[numOfScoreText].text = hitCount.ToString();
            scoreTextList[numOfScoreText].text = numOfScoreText.ToString();
            numOfScoreText+=1;
        }
        else
        {
            
        }*/
    }
    List<Text> InsertToScoreTextList(List<Text> list, Collision2D peg, string str)
    {
        for(int i=list.Count-2;i>=0;i--)
        {
            //Debug.Log(i);
            list[i+1].text=list[i].text;
            list[i+1].transform.position = list[i].transform.position;
        }
        list[0].text = str;
        list[0].transform.position = 
        new Vector3(peg.transform.position.x,peg.transform.position.y+0.08f,peg.transform.position.z);
        return list;
    }
    public void ResetBall()
    {
        if(ballNum>0)
        {
            foreach(Text textObj in scoreTextList)
            {
                textObj.transform.position = new Vector3(-10.0f,0.0f,0.0f);
                textObj.text = "00";
            }
            isResetting = true;
        }
        else
        {
            capsule.GetComponent<scrCapsule>().ball.GetComponent<SpriteRenderer>().color = new Color(0.0f,0.0f,0.0f,0.0f);
            GameObject.Find("objBallLight").GetComponent<SpriteRenderer>().color = new Color(0.0f,0.0f,0.0f,0.0f);
            GameObject.Find("sprPegShadow_0").GetComponent<SpriteRenderer>().color = new Color(0.0f,0.0f,0.0f,0.0f);
        }
    }
}
