using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonCollider : MonoBehaviour,IDamagableE
{
    public DemonBroad DB;
    public void DieOut(){
        DB.DieOut();
    }
}
