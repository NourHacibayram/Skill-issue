using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Player player;

    private bool canDetectLedge;

    void Update()
    {
        if(canDetectLedge) 
        player.ledgeDetected= Physics2D.OverlapCircle(transform.position, radius);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetectLedge = false;
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetectLedge = true;
        
    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
