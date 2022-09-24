using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMusicPlayer : MonoBehaviour
{
    AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();

        var audioDuration = audioSources[0].clip.length;

        audioSources[1].PlayDelayed(audioDuration + 2);
    }
}
