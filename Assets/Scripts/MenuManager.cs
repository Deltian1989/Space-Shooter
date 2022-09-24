using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    AudioSource audioSource;

    bool isGamestarted = false;

    // Start is called before the first frame update
    void Awake()
    {
        isGamestarted = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnMenuItemSelect()
    {
        if (isGamestarted)
        {
            audioSource.Play();
        }
        else
        {
            isGamestarted = true;
        }
            
    }
}
