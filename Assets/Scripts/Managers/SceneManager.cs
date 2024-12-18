using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneManager : GameBehaviour
{
    private CanvasGroup transitionPanel;
    private Image transitionImage;
    private AsyncOperation loadingOperation;

    private string sceneToLoad = "";
    string currentSceneName => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    private void Awake()
    {
        transitionPanel = GetComponentInChildren<CanvasGroup>();
        transitionImage = GetComponentInChildren<Image>();
        TransitionIn();
    }
    private void Start()
    {
        
    }
    public void SetSceneName(string _sceneName)
    {
        sceneToLoad = _sceneName;
    }
    //public void LoadScene(SceneAsset _scene)
    //{
    //    TransitionOut(_scene.name);
    //}
    public void LoadScene(string _scene)
    {
        TransitionOut(_scene);
    }
    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad)) sceneToLoad = currentSceneName;
        loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
        Time.timeScale = 1;
    }
    public void RestartScene()
    {
        TransitionOut(currentSceneName);
    }

    public void ResumeScene()
    {
        _GAME.ChangeGameState(_GAME.previousState);
    }

    public void LoadTitle()
    {
        TransitionOut("Title");
    }

    public void TransitionOut(string _newScene)
    {
        sceneToLoad = _newScene;
        switch (_TWEENING.sceneTransitionType)
        {
            case TransitionType.Fade:
                FadeX.FadeIn(transitionPanel, 2f, _TWEENING.sceneTransitionEase, true, ()=> LoadScene());
                break;
            case TransitionType.Fill:
                transitionImage.fillAmount = 0;
                transitionPanel.alpha = 1;
                TweenX.TweenFill(transitionImage, 2f, _TWEENING.sceneTransitionEase, 1f, ()=> LoadScene());
                break;
        }
    }

    public void TransitionIn()
    {
        switch (_TWEENING.sceneTransitionType)
        {
            case TransitionType.Fade:
                transitionPanel.alpha = 1;
                FadeX.FadeOut(transitionPanel, 2f, _TWEENING.sceneTransitionEase, false, () => SceneLoaded());
                break;
            case TransitionType.Fill:
                transitionImage.fillAmount = 1;
                transitionPanel.alpha = 1;
                TweenX.TweenFill(transitionImage, 2f, _TWEENING.sceneTransitionEase, 0f, () => SceneLoaded());
                break;
        }
    }

    private void SceneLoaded()
    {

    }

    //Quit Button
    public void QuitGame()
    {
        
        switch (_TWEENING.sceneTransitionType)
        {
            case TransitionType.Fade:
                FadeX.FadeIn(transitionPanel, 2f, _TWEENING.sceneTransitionEase, true, () => Application.Quit());
                break;
            case TransitionType.Fill:
                transitionImage.fillAmount = 0;
                transitionPanel.alpha = 1;
                TweenX.TweenFill(transitionImage, 2f, _TWEENING.sceneTransitionEase, 1f, () => Application.Quit());
                break;
        }
    }
}

public enum TransitionType
{
    Fade, Fill
}
