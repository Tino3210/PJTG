using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndManager : MonoBehaviour
{
    public TextMeshProUGUI _scoreText;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = ""+GameController.score;
    }
}
