using System.Collections;
using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public bool IsHidden => owner != null && owner.isHidden;

    private Player owner;
    private Coroutine damageCoroutine;

    public void Initialize(Player playerOwner)
    {
        owner = playerOwner;
    }

    public void EnterHide()
    {
        if (owner == null || owner.isHidden)
        {
            return;
        }

        if (owner.hidingSource != null && owner.openDoor != null)
        {
            owner.hidingSource.PlayOneShot(owner.openDoor);
        }

        if (owner.hidingSource != null && owner.hidingClip != null)
        {
            owner.hidingSource.clip = owner.hidingClip;
            owner.hidingSource.Play();
        }

        owner.isHidden = true;

        if (owner.SpriteRenderer != null)
        {
            owner.SpriteRenderer.enabled = false;
        }

        if (owner.Collider != null)
        {
            owner.Collider.enabled = false;
        }

        if (owner.hideImage != null)
        {
            owner.hideImage.gameObject.SetActive(true);
        }

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }

        damageCoroutine = StartCoroutine(HideDamageRoutine());
    }

    public void ExitHide()
    {
        if (owner == null || !owner.isHidden)
        {
            return;
        }

        if (owner.hidingSource != null && owner.openDoor != null)
        {
            owner.hidingSource.PlayOneShot(owner.openDoor);
        }

        if (owner.hidingSource != null && owner.hidingSource.isPlaying)
        {
            owner.hidingSource.Stop();
        }

        owner.isHidden = false;

        if (owner.SpriteRenderer != null)
        {
            owner.SpriteRenderer.enabled = true;
        }

        if (owner.Collider != null)
        {
            owner.Collider.enabled = true;
        }

        if (owner.hideImage != null)
        {
            owner.hideImage.gameObject.SetActive(false);
        }

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        StopAllCoroutines();
        ResetWarningImage();
    }

    private IEnumerator HideDamageRoutine()
    {
        float halfSafeTime = owner.safeHideTime / 2f;

        yield return new WaitForSeconds(halfSafeTime);

        if (owner.isHidden && owner.warningImage != null)
        {
            yield return StartCoroutine(FadeWarning(1f));
        }

        yield return new WaitForSeconds(halfSafeTime);

        if (owner.isHidden)
        {
            Services.Health?.TakeDamage(Mathf.RoundToInt(owner.damageRate));
            if (owner.warningImage != null)
            {
                yield return StartCoroutine(FadeWarning(0f));
            }

            ExitHide();
        }
    }

    private IEnumerator FadeWarning(float targetAlpha)
    {
        if (owner.warningImage == null)
        {
            yield break;
        }

        float speed = targetAlpha > 0f ? owner.fadeInSpeed : owner.fadeOutSpeed;
        Color color = owner.warningImage.color;

        while (!Mathf.Approximately(color.a, targetAlpha))
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * speed);
            owner.warningImage.color = color;
            yield return null;
        }
    }

    private void ResetWarningImage()
    {
        if (owner.warningImage == null)
        {
            return;
        }

        Color color = owner.warningImage.color;
        color.a = 0f;
        owner.warningImage.color = color;
    }
}
