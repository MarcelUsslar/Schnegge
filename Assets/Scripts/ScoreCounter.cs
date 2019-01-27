using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textHighscore;
    [SerializeField] private GameObject schnegge;
    [SerializeField] private float scoreCountingSpeed = 10;
    private int score = 0;
    private int highscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score = (int)(schnegge.transform.position.x * scoreCountingSpeed);
        text.text = score.ToString();

    }

    public void resetScore(){
        saveScore();
        score = 0;
    }

    private void saveScore(){
        if(score > highscore){
            highscore = score;
            textHighscore.text = highscore.ToString();
            textHighscore.gameObject.SetActive(true);
        }
    }
}
