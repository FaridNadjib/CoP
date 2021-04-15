using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTrigger : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (transform.position.x < PlayerController.Instance.transform.position.x)
            {
                anim.SetInteger("PlayerPos", -1);
            }
            else if (transform.position.x > PlayerController.Instance.transform.position.x)
                anim.SetInteger("PlayerPos", 1);
            else if (transform.position.x == PlayerController.Instance.transform.position.x)
                anim.SetInteger("PlayerPos", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            anim.SetInteger("PlayerPos", 0);
    }
}
