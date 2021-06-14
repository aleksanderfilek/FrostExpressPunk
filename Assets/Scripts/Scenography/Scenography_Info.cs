using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenography_Info", menuName = "Custom/Scenography_Info", order = 2)]
public class Scenography_Info : ScriptableObject
{
    public ScenographyType type;
    public GameObject prefab;
}