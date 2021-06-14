using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SwitchElement : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    
    
    public Vector2Int index;

    private Image button;

    private void Awake()
    {
        button = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        GameManager.Get.GameStarted += ChangeSprite;
    }

    public void Switch()
    {
        var railManager = GameManager.Get.gameObject.GetComponent<RailManager>();
        railManager.Switch(index);
        ChangeSprite(this, EventArgs.Empty);
    }

    private void ChangeSprite(object sender, EventArgs e)
    {
        var railManager = GameManager.Get.gameObject.GetComponent<RailManager>();
        ushort dir = 0, rotation = 0;
        railManager.GetRailNodeData(index, ref dir, ref rotation);
        Debug.Log("sprite - " + (2*rotation  + dir));

        button.sprite = _sprites[2*rotation + dir]; 
    }
}
