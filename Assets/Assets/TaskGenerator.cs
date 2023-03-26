using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TaskGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _variantA;
    [SerializeField] private TextMeshProUGUI _variantB;
    [SerializeField] private TextMeshProUGUI _variantC;
    [SerializeField] private TextMeshProUGUI _variantD;
    [SerializeField] private int _minNumber = -100;
    [SerializeField] private int _maxNumber = 100;

    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    

    private char[] _operations = { '+', '-', '*' };
    private TextMeshProUGUI _taskText;
    private char _operation;
    private int _number1;
    private int _number2;
    private int _rightAnswer;
    private List<int> _variants;
    private int _score = 0;
    private int _record;

    private void Awake()
    {
        _taskText = GetComponent<TextMeshProUGUI>();
        _record = PlayerPrefs.GetInt("record");
    }

    public void GenerateTask()
    {
        _operation = _operations[Random.Range(0, _operations.Length)];
        _number1 = Random.Range(_minNumber, _maxNumber);
        _number2 = Random.Range(_minNumber, _maxNumber);

        switch (_operation)
        {
            case '+':
                _rightAnswer = _number1 + _number2;
                break;
            case '-':
                _rightAnswer = _number1 - _number2;
                break;
            case '*':
                _rightAnswer = _number1 * _number2;
                break;
        }

        _taskText.text = $"{_number1} {_operation} {_number2}";
        _taskText.color = Color.white;
        _variants = new List<int>();
        _variants.Add(_rightAnswer);
        while (_variants.Count < 4)
        {
            int temp = Random.Range(_rightAnswer - 15, _rightAnswer + 15);
            if (temp != _rightAnswer)
            {
                _variants.Add(temp);
            }
        }

        for (int i = _variants.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i);
            (_variants[j], _variants[i]) = (_variants[i], _variants[j]);
        }

        _variantA.text = Convert.ToString(_variants[0]);
        _variantB.text = Convert.ToString(_variants[1]);
        _variantC.text = Convert.ToString(_variants[2]);
        _variantD.text = Convert.ToString(_variants[3]);
    }

    public void CheckAnswer(TextMeshProUGUI answer)
    {
        if (Convert.ToString(_rightAnswer) == answer.text)
        {
            StartCoroutine(ShowResult(Color.green, "Верно"));
        }
        else
        {
            StartCoroutine(ShowResult(Color.red, "Неверно"));
        }
    }

    private IEnumerator ShowResult(Color color, string text)
    {
        _taskText.text = text;
        _taskText.color = color;
        yield return new WaitForSeconds(1f);
        if (color == Color.red)
        {
            _record = Math.Max(_record, _score);
            PlayerPrefs.SetInt("record", _record);
            _recordText.text = "Лучший результат: " + _record;
            _scoreText.text = "Ваш результат: " + _score;
            
            _gameCanvas.SetActive(false);
            _resultCanvas.SetActive(true);
        }
        else
        {
            _score += 1;
            GenerateTask();
        }
    }
}