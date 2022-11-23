using System;
using UnityEngine.UI;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
[Serializable]
    public class BestScoreData
    {
        public int score = 0;
    }

    public class Score : MonoBehaviour
    {
        public Text scoreText;

        private bool newBestScore = false;

        private BestScoreData bestScores = new BestScoreData();
        
        private int currentScores;
        
        void Start()
        {
            currentScores = 0;
            newBestScore = false;
            UpdateScoreText();
        }

        public void OnEnable()
        {
            GameEvents.AddScores += AddScores;
            GameEvents.GameOver += SaveBestScore;
        }

        public void OnDisable()
        {
            GameEvents.AddScores -= AddScores;
            GameEvents.GameOver -= SaveBestScore;
        }

        private void AddScores(int scores)
        {
            currentScores += scores;
            if (currentScores > bestScores.score)
            {
                newBestScore = true;
                bestScores.score = currentScores;
            }
            UpdateScoreText();
        }

        private void SaveBestScore(bool newBestScore)
        {
            
        }

        private void UpdateScoreText()
        {
            scoreText.text = currentScores.ToString();
        }
    }

}