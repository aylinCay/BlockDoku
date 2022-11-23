using System;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameOverPopUp : MonoBehaviour
    {
        public GameObject gameOverPopup;
        public GameObject loosePopup;
        public GameObject newBestSocrePopup;
        
        void Start()
        {
            gameOverPopup.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvents.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameEvents.GameOver -= OnGameOver;
        }

        private void OnGameOver(bool newBestScore)
        {
            gameOverPopup.SetActive(true);
            loosePopup.SetActive(false);
            newBestSocrePopup.SetActive(true);
        }
    }

}