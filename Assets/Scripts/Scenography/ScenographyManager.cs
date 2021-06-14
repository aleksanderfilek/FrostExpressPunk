using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenographyManager : MonoBehaviour
{
    [SerializeField]
    private List<Scenography_Info> elements;

    public GameObject scenographyParent;

    private static ScenographyManager instance;

    void Awake()
    {
        instance = this;
    }
    
    public void AddElement(ScenographyType type, Vector3 position, float angle, Vector2Int index)
    {
        foreach (var element in elements)
        {
            if (element.type != type)
                continue;
            
            var ob = Instantiate(element.prefab, position, Quaternion.Euler(0.0f, angle, 0.0f), scenographyParent.transform) as GameObject;

            if (type == ScenographyType.Switch2 || type == ScenographyType.Switch3)
            {
                ob.GetComponent<SwitchElement>().index = index;
            }
			else if(type == ScenographyType.Generator)
			{
				var gm = GameManager.Get;
				gm.AddBlast(1);
			}
        }
    }

    public void Unload()
    {
         foreach (Transform child in scenographyParent.transform)
		 {
     		GameObject.Destroy(child.gameObject);
 		}
    }
}
