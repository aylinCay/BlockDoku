using System;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class BestScoreBar : MonoBehaviour
    {
        public Image fillInImage;
        public Text bestScoreText;

        private void Onable()
        {
            GameEvents.UpdateBestScore += UpdateBestScoreBar;
        }

        private void OnDisable()
        {
            GameEvents.UpdateBestScore -= UpdateBestScoreBar;
        }

        public void UpdateBestScoreBar(int currentScore, int bestScore)
        {
            float curretnPrecentage = (float)currentScore / (float)bestScore;
            fillInImage.fillAmount = (float)curretnPrecentage;
            bestScoreText.text = curretnPrecentage.ToString();

        }
        
    }

}