using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour, ITimeControlable
{
    public NewBlock Block;
    public void ChangeCurrentTime(float deltaTime)
    {
        Block.ChangeCurrentTime(deltaTime);
    }
    public void Lighten(bool IsLighten)
    {
        Block.Lighten(IsLighten);
    }
}
