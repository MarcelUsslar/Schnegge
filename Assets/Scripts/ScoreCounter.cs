using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textHighscore;
    [SerializeField] private GameObject schnegge;
    [SerializeField] private float scoreCountingSpeed = 10;
    private int _score = 0;
    private int _highscore;
    private float _startPosition;

    public int Score => _score;
    public float ScoreCountingSpeed => scoreCountingSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        _score = Mathf.Max((int)((schnegge.transform.position.x - _startPosition) * scoreCountingSpeed), _score);
        text.text = _score.ToString();

    }

    public void resetScore(){
        saveScore();
        _score = 0;
    }

    private void saveScore(){
        if(_score > _highscore){
            _highscore = _score;
            textHighscore.text = _highscore.ToString();
            textHighscore.gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        _startPosition = schnegge.transform.position.x;
    }
}
