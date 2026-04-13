using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeControlable
{
    public void Lighten(bool IsLighten);
    public void ChangeCurrentTime(float deltaTime);
}
