using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerPrompt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfHide());
    }

    IEnumerator SelfHide()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
