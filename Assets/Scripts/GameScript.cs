using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public Animator teacherAnimator;
    public Animator mteAnimator;
    public Animator movingAnimator;
    public Animator moeAnimator;
    public Animator sorryAnimator;
    public AudioSource audioSource;
    public TMP_Text scoreTxt;
    public Image hpBar;
    public AudioClip dmcaClip, originalClip;

    private Controls _controls;
    private bool _pressed;
    private float _score;
    private bool _gameOver;
    public bool teacherWatching = true;
    private readonly WaitForSeconds _startWait = new(0.2f);
    private static readonly int SpaceDown = Animator.StringToHash("SpaceDown");
    public static readonly int Watch = Animator.StringToHash("Watch");

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        audioSource.clip = StaticVariables.isDmca switch
        {
            0 => originalClip,
            1 => dmcaClip,
            _ => audioSource.clip
        };
        
        StartCoroutine(nameof(FirstStart));
    }

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnDestroy()
    {
        _controls.Dispose();
    }

    private void Update()
    {
        scoreTxt.SetText(((int)_score).ToString());

        if (hpBar.fillAmount == 0)
        {
            StaticVariables.recentScore = (int)_score;
            SceneManager.LoadScene("HP_GameOver");
            return;
        }
        
        if (teacherWatching && _pressed)
        {
            StaticVariables.recentScore = (int)_score;
            SceneManager.LoadScene("GameOver");
            return;
        }

        if (_controls.Play.GamePlay.WasPressedThisFrame())
        {
            _pressed = true;
            audioSource.Play();
            mteAnimator.SetBool(SpaceDown, true);
            movingAnimator.SetBool(SpaceDown, true);
            moeAnimator.SetBool(SpaceDown, true);
            sorryAnimator.SetBool(SpaceDown, true);
        }
        
        if (_controls.Play.GamePlay.WasReleasedThisFrame())
        {
            _pressed = false;
            audioSource.Stop();
            mteAnimator.SetBool(SpaceDown, false);
            movingAnimator.SetBool(SpaceDown, false);
            moeAnimator.SetBool(SpaceDown, false);
            sorryAnimator.SetBool(SpaceDown, false);
        }
        
        if (_pressed)
        {
            var deltaTime = Time.deltaTime;
            hpBar.fillAmount += deltaTime * 0.125f;
            _score += deltaTime * 1.5f * 15;
        }
        else
        {
            var deltaTime = Time.deltaTime;
            hpBar.fillAmount -= deltaTime * 0.05f;
            _score += deltaTime * 15;
        }
    }

    private IEnumerator FirstStart()
    {
        yield return _startWait;
        teacherAnimator.SetBool(Watch, false);
    }
}
