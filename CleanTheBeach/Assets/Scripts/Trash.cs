using System;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private Texture pressETexture;
    private Transform playerTransform;
    
    // Set this distance to whatever feels right (larger number = bigger range)
    public float pickupRangeRadius = 1.5f; 

    private AudioClip collectSound;

    private void Start()
    {
        pressETexture = Resources.Load<Texture>("press_E");
        collectSound = Resources.Load<AudioClip>("collect");
        
        // Find the player automatically at the start
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Constantly check distance to the player (ignores colliders entirely)
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            isPlayerNearby = distance <= pickupRangeRadius;
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            // Play collect sound
            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            ScoreManager.instance.AddPoint();
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
    private void OnGUI()
    {
        if (isPlayerNearby && pressETexture != null)
        {
            float width = 200f;
            float height = width * ((float)pressETexture.height / pressETexture.width);
            float x = (Screen.width - width) / 2f;
            float y = (Screen.height - height) / 2f + 100f; 
            
            GUI.DrawTexture(new Rect(x, y, width, height), pressETexture, ScaleMode.ScaleToFit);
        }
    }
}
