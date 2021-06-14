using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailNode
{
    public ushort dir = 0;
    public ushort maxDir = 0;
    public ushort type = 0;
    public ushort rotation = 0;
    public bool isValid = false;
    public Vector3 position;
}
