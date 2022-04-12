using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChanger : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreCounter;
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void Start()
    {
        _scoreCounter.OnCollide += ChangeScore;
    }
    private void OnDisable()
    {
        _scoreCounter.OnCollide -= ChangeScore;
    }
    private void ChangeScore(string newText)
    {
        _text.text = newText;
    }
}
