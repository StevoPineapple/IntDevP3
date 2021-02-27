using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPeg : MonoBehaviour
{
    public bool isOrange;
    public bool isGreen;
    public bool isPurple;
    public bool isHit;
    SpriteRenderer sprRenderer;
    Sprite[] sprPeg;
    // Start is called before the first frame update
    
    
    void Awake()
    {
        isHit = false;
        isOrange = false;
        sprRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeToOrange()
    {
        //Debug.Log(gameObject.name);
        isOrange = true;
        sprPeg = Resources.LoadAll<Sprite>("Sprites/sprPeg");
        sprRenderer.sprite = sprPeg[2];
        //Debug.Log(Resources.Load<Sprite>("Sprites/sprPeg_2"));
    }
    public void changeToGreen()
    {
        isGreen = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/sprPegGreen");
        gameObject.AddComponent<scrSpecialPeg>();
        gameObject.AddComponent<BallBehaviorM>();
        //Debug.Log(Resources.Load<Sprite>("Sprites/sprPeg_2"));
    }
    public void changeToPurple()
    {
        isPurple = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/sprPegPurple");
        gameObject.tag = "PegsPurple";
        //Debug.Log(Resources.Load<Sprite>("Sprites/sprPeg_2"));
    }
    public void changeToBlue()
    {
        //Debug.Log(Resources.LoadAll<Sprite>("Sprites/sprPeg").Length);
        isPurple = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/sprPeg")[1];
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        gameObject.tag = "Pegs";  
    }  
}

