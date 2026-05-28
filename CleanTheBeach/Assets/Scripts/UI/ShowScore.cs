using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public Text Score;
    // Start is called before the first frame update
    void Start()
    {
        float score = ScoreManager.instance.GetScore();
        if(score == 0)
        {
            Score.text = "Not Good\n you Picked up " + score + " Pieces of trash";
        }
        if (score == 1)
        {
            Score.text = "Not Good\n you Picked up " + score + " Piece of trash";
        }
        else
        {
            Score.text = "Congratulations\n you Picked up " + score + " Pieces of trash";
        }
    }
}
