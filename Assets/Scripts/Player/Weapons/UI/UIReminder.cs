using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIReminder : MonoBehaviour
{
    [SerializeField]
    private Sprite highlighted;
    private Sprite baseSprite;
    private Image img;
    private Animator anim;

    private void Awake()
    {
        img = GetComponent<Image>();
        anim = GetComponent<Animator>();
        baseSprite = img.sprite;
    }

    public void Highlight()
    {
        img.sprite = highlighted;
        StopAllCoroutines();
        StartCoroutine(Revert(0.1f));
    }

    public void Lock()
    {
        anim.SetBool("Locked", true);
    }

    public void UnLock()
    {
        anim.SetBool("Locked", false);
    }

    public void StopBlinking()
    {
        anim.SetBool("Blinking", false);
    }

    private IEnumerator Revert(float after)
    {
        yield return new WaitForSeconds(after);
        img.sprite = baseSprite;
    }
}
