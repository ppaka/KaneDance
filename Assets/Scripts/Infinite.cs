using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Infinite : MonoBehaviour
{
    public Canvas descriptionCanvas, IngameCanvas;
    public Animator moongtaengAnimator, movingAnimator, moeAnimator, sorryAnimator;
    public new AudioSource audio;
    
    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");
    private bool _toggle, _playing, _started;
    
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
        audio.gameObject.SetActive(true);
        _started = true;
    }

    public void Exit()
    {
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
                            audio.Play();
                            moongtaengAnimator.SetBool(SpaceDown, true);
                            movingAnimator.SetBool(SpaceDown, true);
                            moeAnimator.SetBool(SpaceDown, true);
                            sorryAnimator.SetBool(SpaceDown, true);
                            break;
                        case true:
                            _playing = false;
                            audio.Stop();
                            moongtaengAnimator.SetBool(SpaceDown, false);
                            movingAnimator.SetBool(SpaceDown, false);
                            moeAnimator.SetBool(SpaceDown, false);
                            sorryAnimator.SetBool(SpaceDown, false);
                            break;
                    }
                }
                else
                {
                    audio.Play();
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
                    audio.Stop();
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
