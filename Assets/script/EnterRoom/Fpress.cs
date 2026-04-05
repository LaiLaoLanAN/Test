using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fpress : MonoBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector2(player.position.x,player.position.y+6f);
    }
}
