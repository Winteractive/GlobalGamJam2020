using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("test_play2", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
