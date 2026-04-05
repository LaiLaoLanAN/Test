using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D platformCollider;
    public GameObject player;
    public Collider2D playerCollider;
    public bool isPlayerOnPlatform;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isPlayerOnPlatform && Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space)){
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            Invoke("ResetCollision", 0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            Vector2 normal = collision.contacts[0].normal;
            Debug.Log(normal);
            if (normal.y > -0.5f){
                Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
            }
            else{
                isPlayerOnPlatform = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            isPlayerOnPlatform = false;
            ResetCollision();
        }
    }
    void ResetCollision(){
        Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
    }
}