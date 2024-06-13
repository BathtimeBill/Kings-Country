using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SceneManager : GameBehaviour
{
    public TransitionType transitionType;
    public float transitionTime = 5f;
    public DG.Tweening.Ease transitionEase;

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
    public void LoadScene(SceneAsset _scene)
    {
        TransitionOut(_scene.name);
    }
    public void LoadScene(string _scene)
    {
        TransitionOut(_scene);
    }
    private void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneToLoad)) sceneToLoad = currentSceneName;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoad);
        loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);

    }
    public void RestartScene()
    {
        TransitionOut(currentSceneName);
    }

    public void ResumeScene()
    {
        _GM.ChangeGameState(_GM.previousState);
    }

    public void LoadTitle()
    {
        TransitionOut("Title");
    }

    public void TransitionOut(string _newScene)
    {
        sceneToLoad = _newScene;
        switch (transitionType)
        {
            case TransitionType.Fade:
                FadeX.FadeIn(transitionPanel, 2f, transitionEase, true, ()=> LoadScene());
                break;
            case TransitionType.Fill:
                transitionImage.fillAmount = 0;
                transitionPanel.alpha = 1;
                TweenX.TweenFill(transitionImage, 2f, transitionEase, 1f, ()=> LoadScene());
                break;
        }
    }

    public void TransitionIn()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                transitionPanel.alpha = 1;
                FadeX.FadeOut(transitionPanel, 2f, transitionEase, false, () => SceneLoaded());
                break;
            case TransitionType.Fill:
                transitionImage.fillAmount = 1;
                transitionPanel.alpha = 1;
                TweenX.TweenFill(transitionImage, 2f, transitionEase, 0f, () => SceneLoaded());
                break;
        }
    }

    private void SceneLoaded()
    {

    }

    //Quit Button
    public void QuitGame()
    {
        Application.Quit();
    }
}

public enum TransitionType
{
    Fade, Fill
}
