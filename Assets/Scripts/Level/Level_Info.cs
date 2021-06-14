using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_Info", menuName = "Custom/Level_Info", order = 1)]
public class Level_Info : ScriptableObject
{
    public Texture2D railTex;
    public ushort carNumber;
    public int initialFuel;
    public int maxBlastCoal;
    public int[] blastCoal;

    public Vector4 worldSize;
}
