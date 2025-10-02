using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManajer : MonoBehaviour
{
    [SerializeField] private TMP_Text paddle1ScoreText;
    [SerializeField] private TMP_Text paddle2ScoreText;


    [SerializeField] private Transform paddle1Transform;
    [SerializeField] private Transform paddle2Transform;
    [SerializeField] private Transform ballTranform;

    private int paddle1Score;
    private int paddle2Score;

    private static GameManajer instance;
    public static GameManajer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManajer>();
            }
            return instance;
        }
    }

    public void Padde1Scored()
    {
        paddle1Score++;
        paddle1ScoreText.text = paddle1Score.ToString();
    }

    public void Padde2Scored()
    {
        paddle2Score++;
        paddle2ScoreText.text = paddle2Score.ToString();
    }

    public void Restart()
    {
        paddle1Transform.position = new Vector2(paddle1Transform.position.x, 0);
        paddle1Transform.position = new Vector2(paddle1Transform.position.x, 0);
        ballTranform.position = new Vector2(0, 0);
    }
}
