using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public float Time = 4f;
    private AudioClip collectSound;

    private void Start()
    {
        collectSound = Resources.Load<AudioClip>("collect");
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("TEST");
        if (!other.CompareTag("Player")) return;

        // Play collect sound
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        ScoreManager.instance.BonusActivated = true;
        ScoreManager.instance.SetTime(Time);
        //print("Trash pickup up!");
        Destroy(gameObject);
    }

}
