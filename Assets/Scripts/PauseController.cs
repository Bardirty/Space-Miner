using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private Animator[] animators;
    [SerializeField] private Animator fade;
    [SerializeField] private Canvas mainInterface;
    [SerializeField] private Canvas pause;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    public bool isPaused = false;
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
    }
    public void OnResumeButtonClick()
    {
        Cursor.visible = false;
        isPaused = false;
        Time.timeScale = 1.0f;
        SwitchPause();
    }
    public void SetVolume()
    {
        audioMixer.SetFloat("Volume", volumeSlider.value * 50 - 50);
    }
    public void OnBackButtonClick()
    {
        Time.timeScale = 1;
        isPaused = false;
        fade.SetTrigger("isFaded");
        animators[1].SetBool("isPaused", false);
        Invoke(nameof(Back), 2f);
    }
    public void OnExitButtonClick()
    {
        Time.timeScale = 1;
        isPaused = false;
        fade.SetTrigger("isFaded");
        animators[1].SetBool("isPaused", false);
        Invoke(nameof(Exit), 2f);
    }
    public void SwitchPause()
    {
        mainInterface.gameObject.SetActive(!isPaused);
        pause.gameObject.SetActive(isPaused);
        animators[0].SetBool("isFaded", isPaused);
        animators[1].SetBool("isPaused", isPaused);
        Invoke(nameof(PauseGame), 1f);
    }
    private void PauseGame()
    {
        Time.timeScale = (isPaused) ? 0 : 1;
    }
    private void Back()
    {
        SceneManager.LoadSceneAsync($"MainMenu{(int)Random.Range(1, 4)}");
    }
    private void Exit()
    {
        Application.Quit();
    }
}
