using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Canvas mainPanel;
    [SerializeField] private Animator mainAnimator;
    [SerializeField] private Canvas settingsPanel;
    [SerializeField] private Animator settingsAnimator;
    [SerializeField] private Canvas quitPanel;
    [SerializeField] private Animator quitAnimator;
    private bool isQuit;
    private bool isSettings;
    void Start()
    {
        isQuit = false;
        isSettings = false;
    }
    void Update()
    {

    }
    public void OnPlayButtonClick()
    {
        Debug.Log("PlayPressed");
        SceneManager.LoadSceneAsync("SampleScene");
    }
    //Settings//////////////////////////////////////////
    public void OnSettingsButtonClick()
    {
        isQuit = false;
        isSettings = true;
        Debug.Log("SettingsPressed");
        mainAnimator.SetBool("IsFaded", true);
        settingsAnimator.SetBool("IsFaded", false);
    }
    public void onBackButtonClick()
    {
        isSettings = false;
        mainAnimator.SetBool("IsFaded", false);
        settingsAnimator.SetBool("IsFaded", true);
        mainPanel.gameObject.SetActive(!isQuit);
    }

    //////////////////////////////////////////////////

    //Exit////////////////////////////////////////////
    public void onExitButtonClick()
    {
        isQuit = true;
        Debug.Log("ExitPressed");
        mainAnimator.SetBool("IsFaded", true);
        quitAnimator.SetBool("IsFaded", false);
    }
    public void onYesButtonClick()
    {
        Application.Quit();
    }
    public void onNoButtonClick()
    {
        isQuit = false;
        mainAnimator.SetBool("IsFaded", false);
        quitAnimator.SetBool("IsFaded", true);
        mainPanel.gameObject.SetActive(!isQuit);
    }
    ///////////////////////////
    public void changeMainExit()
    {
        mainPanel.gameObject.SetActive(!isQuit);
        quitPanel.gameObject.SetActive(isQuit);
    }
    public void changeMainSettings()
    {
        mainPanel.gameObject.SetActive(!isSettings);
        settingsPanel.gameObject.SetActive(isSettings);
    }
}
