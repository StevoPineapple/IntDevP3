using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBlockBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "PegsGreen"){
            Debug.Log("fheyuifgeyigs");
            other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = GameObject.Find("objSceneManager").GetComponent<scrSceneManager>().ballGrav;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
