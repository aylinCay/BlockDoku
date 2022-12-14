namespace BlockDoku
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameEvents : MonoBehaviour
    {
        
        public static Action<bool> GameOver;

        public static Action<int> AddScores;

        public static Action CheckIfShapeCanBePlaced;

        public static Action MoveShapeToStartPosition;

        public static Action RequestNewShape;

        public static Action SetShapeInActive;

        public static Action<int, int> UpdateBestScore;
    }
}