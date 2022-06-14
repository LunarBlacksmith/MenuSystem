using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null && instance != this)
        { Destroy(this.gameObject); }
        else
        { instance = this; }
    }
}
