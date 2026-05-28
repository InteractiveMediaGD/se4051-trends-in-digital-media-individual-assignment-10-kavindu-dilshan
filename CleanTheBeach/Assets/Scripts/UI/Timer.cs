using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public string load;
    public float timeLeft = 60f;
    public Text textBox;

    void Awake()
    {
        if (textBox != null)
        {
            textBox.fontSize = 24; // Reduce text size to fit better
        }
        AddBackground(textBox, "timer_background");
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        textBox.text = (timeLeft).ToString("0") + "s";
        
        // Turn text red in last 10 seconds
        if (timeLeft <= 10f)
            textBox.color = Color.red;
        else
            textBox.color = Color.white;
            
        if (timeLeft <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void AddBackground(Text textComponent, string imageName)
    {
        if (textComponent == null) return;
        
        // Make text white by default, center align it
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleCenter;
        
        Texture2D tex = Resources.Load<Texture2D>(imageName);
        if (tex != null)
        {
            GameObject bgObj = new GameObject(imageName + "_bg");
            bgObj.transform.SetParent(textComponent.transform.parent, false);
            bgObj.transform.SetSiblingIndex(textComponent.transform.GetSiblingIndex());
            
            LayoutElement le = bgObj.AddComponent<LayoutElement>();
            le.ignoreLayout = true;
            
            Image img = bgObj.AddComponent<Image>();
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            img.type = Image.Type.Simple;
            img.preserveAspect = true; 
            
            RectTransform rect = bgObj.GetComponent<RectTransform>();
            RectTransform textRect = textComponent.GetComponent<RectTransform>();
            
            // Lock background exactly to the text rect's properties to stay perfectly in the corner
            rect.anchorMin = textRect.anchorMin;
            rect.anchorMax = textRect.anchorMax;
            rect.pivot = textRect.pivot;
            
            // Move the entire Timer UI left and down so it doesn't get clipped off screen
            textRect.anchoredPosition += new Vector2(-30f, -30f);
            
            // Position exactly at the text, no offsets
            rect.anchoredPosition = textRect.anchoredPosition; 
            
            // Give it a fixed size that looks good
            rect.sizeDelta = new Vector2(200f, 80f);
        }
    }
}
