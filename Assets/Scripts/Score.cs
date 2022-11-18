using System;
using UnityEngine.UI;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    

    public class Score : MonoBehaviour
    {
        public Text scoreText;
        private int currentScores;
        
        void Start()
        {
            currentScores = 0;
            UpdateScoreText();
        }

        public void OnEnable()
        {
            GameEvents.AddScores += AddScores;
        }

        public void OnDisable()
        {
            GameEvents.AddScores -= AddScores;
        }

        private void AddScores(int scores)
        {
            currentScores += scores;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            scoreText.text = currentScores.ToString();
        }
    }

}