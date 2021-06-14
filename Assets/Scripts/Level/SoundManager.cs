using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        audioData.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayClickSound()
    {
        audioData.volume = 0.3f;
        audioData.Play();
    }
}
