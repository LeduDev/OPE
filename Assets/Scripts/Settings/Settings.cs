using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings _instance;
    public static Settings Instance { get { return _instance; } }

    public enum Languages { Portuguese, English }

    public static Languages language;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        language = (Languages)PlayerPrefs.GetInt("Language", 0);
    }
}
