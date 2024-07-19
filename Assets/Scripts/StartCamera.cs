using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartCamera : GameBehaviour
{
    private Animator animator;
    public Camera cam;
    public TMP_Text levelTitle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cam.gameObject.SetActive(false);
    }

    public void StartCamAnimation(LevelID _levelID)
    {
        cam.gameObject.SetActive(true);
        levelTitle.text = _DATA.GetLevel(_levelID).name;
        switch (_GM.level)
        {
            case LevelNumber.One:
                animator.SetTrigger("One");
                break;
            case LevelNumber.Two:
                animator.SetTrigger("Two");
                break;
            case LevelNumber.Three:
                animator.SetTrigger("Three");
                break;
            case LevelNumber.Four:
                break;
            case LevelNumber.Five:
                break;
        }
    }
    
    public void BeginGame()
    {
        _GM.ChangeGameState(_showTutorial ? GameState.Tutorial : GameState.Build);
        gameObject.SetActive(false);
    }
}
