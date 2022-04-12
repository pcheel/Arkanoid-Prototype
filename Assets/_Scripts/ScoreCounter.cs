using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private string _text;

    private float _scoreCounter = 0;
    public Action<string> OnCollide;

    private void OnCollisionEnter(Collision collision)
    {
        _scoreCounter++;
        OnCollide?.Invoke(_text + _scoreCounter);
    }
}
