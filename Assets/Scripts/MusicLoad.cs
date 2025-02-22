using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoad : MonoBehaviour
{
    private static MusicLoad Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
