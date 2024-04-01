using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOffOn : MonoBehaviour
{
    public bool isOn;

    private void Start()
    {
        isOn = true;
    }

    public void OnOffSound()
    {
        if (!isOn)
        {
            AudioListener.volume = 1f;
            isOn = true;
        }
        else if (isOn)
        {
            AudioListener.volume = 0f;
            isOn = false;
        }
    }
}
