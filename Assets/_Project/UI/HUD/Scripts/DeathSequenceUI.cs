using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DeathSequenceUI : MonoBehaviour
{
    [SerializeField] private GameObject serializedDeathScreen;
    [SerializeField] private VideoPlayer serializedDeathVideoPlayer;
    [SerializeField] private GameObject serializedVideoRawImage;

    private GameObject deathScreen;
    private VideoPlayer deathVideoPlayer;
    private GameObject videoRawImage;
    private Coroutine sequenceCoroutine;
    private bool subscribed;
    private bool initialized;

    public void Initialize(GameObject screen, VideoPlayer videoPlayer, GameObject rawImageObject)
    {
        deathScreen = screen;
        deathVideoPlayer = videoPlayer;
        videoRawImage = rawImageObject;
        initialized = true;
        Subscribe();
        HideDeathUI();
    }

    private void Start()
    {
        if (!initialized)
        {
            Initialize(serializedDeathScreen, serializedDeathVideoPlayer, serializedVideoRawImage);
        }
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (!subscribed || Services.Health == null)
        {
            return;
        }

        Services.Health.OnDeath -= PlayDeathSequence;
        Services.Health.OnRevived -= HideDeathUI;
        subscribed = false;
    }

    private void Subscribe()
    {
        if (subscribed || Services.Health == null)
        {
            return;
        }

        Services.Health.OnDeath += PlayDeathSequence;
        Services.Health.OnRevived += HideDeathUI;
        subscribed = true;
    }

    private void PlayDeathSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        sequenceCoroutine = StartCoroutine(PlayDeathSequenceRoutine());
    }

    private IEnumerator PlayDeathSequenceRoutine()
    {
        if (deathVideoPlayer != null && deathVideoPlayer.clip != null)
        {
            if (videoRawImage != null)
            {
                videoRawImage.SetActive(true);
            }

            deathVideoPlayer.Stop();
            deathVideoPlayer.Play();
            yield return new WaitForSeconds((float)deathVideoPlayer.clip.length);
        }

        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }
    }

    private void HideDeathUI()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        if (deathVideoPlayer != null)
        {
            deathVideoPlayer.Stop();
        }

        if (videoRawImage != null)
        {
            videoRawImage.SetActive(false);
        }

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }
}
