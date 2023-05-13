using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDecor : GameBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float rndScale = Random.Range(0.5f, 2f);
        transform.rotation = Random.rotation;
        transform.localScale = Vector3.one * rndScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
