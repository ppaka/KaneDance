using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Infinite : MonoBehaviour
{
    public Canvas descriptionCanvas, IngameCanvas;
    public Animator moongtaengAnimator, movingAnimator, moeAnimator, sorryAnimator;
    public AudioSource audioSource;

    public AudioClip dmcaClip, originalClip;
    
    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");
    private bool _toggle, _playing, _started;

    private void Start()
    {
        if (PlayerPrefs.GetInt("isDMCA", 1) == 0)
        {
            audioSource.clip = originalClip;
        }
        else if (PlayerPrefs.GetInt("isDMCA", 1) == 1)
        {
            audioSource.clip = dmcaClip;
        }
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Hold()
    {
        _toggle = false;
        SetGame();
    }
    
    public void Toggle()
    {
        _toggle = true;
        SetGame();
    }

    private void SetGame()
    {
        descriptionCanvas.gameObject.SetActive(false);
        IngameCanvas.gameObject.SetActive(true);
        moongtaengAnimator.gameObject.SetActive(true);
        movingAnimator.gameObject.SetActive(true);
        moeAnimator.gameObject.SetActive(true);
        sorryAnimator.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
        _started = true;
    }

    public void Exit()
    {
        _started = false;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        SceneManager.LoadScene("Main");
    }
    
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if(!_started) return;
        
        if (ctx.started)
        {
            try
            {
                if (_toggle)
                {
                    switch (_playing)
                    {
                        case false:
                            _playing = true;
                            audioSource.Play();
                            moongtaengAnimator.SetBool(SpaceDown, true);
                            movingAnimator.SetBool(SpaceDown, true);
                            moeAnimator.SetBool(SpaceDown, true);
                            sorryAnimator.SetBool(SpaceDown, true);
                            break;
                        case true:
                            _playing = false;
                            audioSource.Stop();
                            moongtaengAnimator.SetBool(SpaceDown, false);
                            movingAnimator.SetBool(SpaceDown, false);
                            moeAnimator.SetBool(SpaceDown, false);
                            sorryAnimator.SetBool(SpaceDown, false);
                            break;
                    }
                }
                else
                {
                    audioSource.Play();
                    moongtaengAnimator.SetBool(SpaceDown, true);
                    movingAnimator.SetBool(SpaceDown, true);
                    moeAnimator.SetBool(SpaceDown, true);
                    sorryAnimator.SetBool(SpaceDown, true);
                }
            }
            catch
            {
                //
            }
        }
        if (ctx.canceled)
        {
            try
            {
                if (!_toggle)
                {
                    audioSource.Stop();
                    moongtaengAnimator.SetBool(SpaceDown, false);
                    movingAnimator.SetBool(SpaceDown, false);
                    moeAnimator.SetBool(SpaceDown, false);
                    sorryAnimator.SetBool(SpaceDown, false);
                }
            }
            catch
            {
                //
            }
        }
    }
}
