using UnityEngine;

public class ShieldBlock : MonoBehaviour
{
    public bool IsBlocking { get; private set; } = false;

    [Header("Опції блокування")]
    public Animator playerAnimator;
    public AudioSource blockAudio;
    public AudioClip blockSound;

    public void PlayBlockEffect()
    {
        if (blockAudio != null && blockSound != null)
        {
            blockAudio.PlayOneShot(blockSound);
        }
    }
}
