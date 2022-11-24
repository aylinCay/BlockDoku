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
        public Text newScoreText;
        private bool newBestScore = false;

         
        public BestScoreData bestScores = new BestScoreData();

        private int currentScores;

        private string bestScoreKey = "bs.dat";

        private void Awake()
        {
            if (BinaryDataStream.Exist(bestScoreKey))
            {
                StartCoroutine(ReadDataFile());
            }
        }

        private IEnumerator ReadDataFile()
        {
            bestScores = BinaryDataStream.Read<BestScoreData>(bestScoreKey);
            yield return new WaitForEndOfFrame();
        }

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
            BinaryDataStream.Save<BestScoreData>(bestScores, bestScoreKey);
        }

        private void UpdateScoreText()
        {
            scoreText.text = currentScores.ToString();
            newScoreText.text = currentScores.ToString();
        }
    }
}