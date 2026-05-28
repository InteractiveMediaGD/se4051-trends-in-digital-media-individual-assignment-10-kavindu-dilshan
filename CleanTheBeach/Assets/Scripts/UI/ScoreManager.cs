using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text TimerText;
    public bool BonusActivated = false;

    private int score = 0;
    private float time = 0f;

    private void Awake()
    {
        instance = this;
        
        if (scoreText != null)
        {
            scoreText.fontSize = 24; // Make the points text small
        }
        
        AddBackground(scoreText, "points_background");
    }

    private void AddBackground(Text textComponent, string imageName)
    {
        if (textComponent == null) return;
        
        // Make text white and centered
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleCenter;
        
        Texture2D tex = Resources.Load<Texture2D>(imageName);
        if (tex != null)
        {
            GameObject bgObj = new GameObject(imageName + "_bg");
            bgObj.transform.SetParent(textComponent.transform.parent, false);
            bgObj.transform.SetSiblingIndex(textComponent.transform.GetSiblingIndex());
            
            // Ignore layout if inside a layout group
            LayoutElement le = bgObj.AddComponent<LayoutElement>();
            le.ignoreLayout = true;
            
            Image img = bgObj.AddComponent<Image>();
            img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            img.type = Image.Type.Sliced;
            
            RectTransform rect = bgObj.GetComponent<RectTransform>();
            RectTransform textRect = textComponent.GetComponent<RectTransform>();
            
            rect.anchorMin = textRect.anchorMin;
            rect.anchorMax = textRect.anchorMax;
            rect.pivot = textRect.pivot;
            
            // Move the text down slightly as requested
            textRect.anchoredPosition += new Vector2(0f, -20f);
            
            // Adjust position and size perfectly for the POINTS text
            rect.anchoredPosition = textRect.anchoredPosition;
            rect.sizeDelta = new Vector2(300f, 60f); // Wider size to perfectly fit "X POINTS"
        }
    }

    void Update()
    {
        scoreText.text = score.ToString() + " POINTS";
        if (BonusActivated)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                BonusActivated = false;
                TimerText.text = "";
            }
            else
                TimerText.text = (time).ToString("0") + "s left";
        }
    }

    public void AddPoint()
    {
        score++;
        if (BonusActivated) 
        {
            score++;
        }
    }
    public void SetTime(float time)
    {
        this.time = time;
    }
    public float GetScore()
    {
        return score;
    }
}
