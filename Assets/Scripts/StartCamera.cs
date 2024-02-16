using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartCamera : GameBehaviour
{
    private Animator animator;
    Camera cam;
    public GameObject[] uiElements;
    public TMP_Text levelTitle;
    public bool isStartCam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cam = GetComponent<Camera>();
    }
    IEnumerator WaitToTurnOnUI()
    {
        yield return new WaitForSeconds(10);
        foreach (GameObject go in uiElements)
        {
            go.SetActive(true);
        }

    }
    void Start()
    {
        if(isStartCam)
        {
            StartCoroutine(WaitToTurnOnUI());
            foreach (GameObject go in uiElements)
            {
                go.SetActive(false);
            }
            switch (_GM.level)
            {
                case LevelNumber.One:

                    animator.SetTrigger("One");
                    levelTitle.text = "Ironwood";
                    break;
                case LevelNumber.Two:

                    animator.SetTrigger("Two");
                    levelTitle.text = "Wormturn Road";
                    break;
                case LevelNumber.Three:

                    animator.SetTrigger("Three");
                    levelTitle.text = "Jotenheim Pass";
                    break;
                case LevelNumber.Four:

                    animator.SetTrigger("Four");
                    levelTitle.text = "Level 4";
                    break;
                case LevelNumber.Five:

                    animator.SetTrigger("Five");
                    levelTitle.text = "Level 5";
                    break;
            }
        }
        else
        {
            levelTitle.text = "Lord Oswyn";
        }

    }
    //public void Check()
    //{
    //    StartCoroutine(WaitToTurnOnUI());
    //    foreach (GameObject go in uiElements)
    //    {
    //        go.SetActive(false);
    //    }
    //    switch (_GM.level)
    //    {
    //        case LevelNumber.One:

    //            animator.SetTrigger("One");
    //            levelTitle.text = "Ironwood";
    //            break;
    //        case LevelNumber.Two:

    //            animator.SetTrigger("Two");
    //            levelTitle.text = "Wormturn Road";
    //            break;
    //        case LevelNumber.Three:

    //            animator.SetTrigger("Three");
    //            levelTitle.text = "Jotenheim Pass";
    //            break;
    //        case LevelNumber.Four:

    //            animator.SetTrigger("Four");
    //            levelTitle.text = "Level 4";
    //            break;
    //        case LevelNumber.Five:

    //            animator.SetTrigger("Five");
    //            levelTitle.text = "Level 5";
    //            break;
    //    }
    //}

}
