using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D barrelRb;
    // Start is called before the first frame update
    void Start()
    {
        barrelRb = GetComponent<Rigidbody2D>(); // get rigid body of barrel
        barrelRb.AddForce(Vector3.left * 10, ForceMode2D.Impulse); // add force to barrel so it can move fast
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Left Bound"))
        {
            barrelRb.AddForce(Vector3.right * 10, ForceMode2D.Impulse); // if collision with left bound apply force in righ direction
        }    
        else if (collision.gameObject.CompareTag("Right Bound"))
        {
            barrelRb.AddForce(Vector3.left * 10, ForceMode2D.Impulse); // if collision with right bound apply force in left direction
        }
        else if(collision.gameObject.CompareTag("Destroy")) 
        {
            Destroy(gameObject); // if collides with last wall destroy barrel
        }
    }
    
}
