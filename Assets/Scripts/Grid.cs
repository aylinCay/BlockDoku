using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockDoku
{
    public class Grid : MonoBehaviour
    {
        public ShapeStorage shapeStorage;
        public int columns = 0;
        public int rows = 0;
        public float squaresGap = 0.1f;
        public GameObject gridSquare;
        public Vector2 startposition = new Vector2(0.0f, 0.0f);
        public float squareScale = 0.5f;
        public float everySquareOffset = 0.0f;

        private Vector2 _offset = new Vector2(0.0f, 0.0f);
        private List<GameObject> _gridSquares = new List<GameObject>();

        public void Start()
        {
            CreateGrid();
        }
        private void OnEnable()
        {
            GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        }

        private void OnDisable()
        {
            GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        }

       

        public void CreateGrid()
        {
            SpawnGridSquare();
            SetGridSquaresPositions();
        }

        public void SpawnGridSquare()
        {
            int _squareIndex = 0;
            for (var row = 0; row < rows; row++)
            {
                for (var colunm = 0; colunm < columns; colunm++)
                {
                    _gridSquares.Add(Instantiate(gridSquare) as GameObject);

                    _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = _squareIndex;
                    _gridSquares[_gridSquares.Count -1].transform.SetParent(this.transform);
                    _gridSquares[_gridSquares.Count - 1].transform.localScale =
                        new Vector3(squareScale, squareScale, squareScale);
                    _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SetImage(_squareIndex % 2 == 0);
                    _squareIndex++;
                }
            }
        }

        public void SetGridSquaresPositions()
        {
            int _columnNumber = 0;
            int _rowNumber = 0;
            Vector2 _squareGapNumber = new Vector2(0.0f, 0.0f);
            bool _rowMoved = false;
            var _squareRect = _gridSquares[0].GetComponent<RectTransform>();

            _offset.x = _squareRect.rect.width * _squareRect.transform.localScale.x + everySquareOffset;
            _offset.y = _squareRect.rect.height * _squareRect.transform.localScale.x + everySquareOffset;

            foreach (GameObject square in _gridSquares)
            {
                if (_columnNumber + 1 > columns)
                {
                    _squareGapNumber.x = 0;
                    _columnNumber = 0;
                    _rowNumber++;
                    _rowMoved = false;
                }

                var _posXOffset = _offset.x * _columnNumber + (_squareGapNumber.x * squaresGap);
                var _posYOffset = _offset.y * _rowNumber + (_squareGapNumber.y * squaresGap);

                if (_columnNumber > 0 && _columnNumber % 3 == 0)
                {
                    _squareGapNumber.x++;
                    _posXOffset += squaresGap;
                }

                if (_rowNumber > 0 && _rowNumber % 3 == 0 && _rowMoved == false)
                {
                    _rowMoved = true;
                    _squareGapNumber.y++;
                    _posYOffset += squaresGap;
                }

                square.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(startposition.x + _posXOffset, startposition.y - _posYOffset);
                square.GetComponent<RectTransform>().localPosition = new Vector3(startposition.x + _posXOffset,
                    startposition.y - _posYOffset, 0.0f);
                
                _columnNumber++;
            }
            

        }

        private void CheckIfShapeCanBePlaced()
        {
            var squareIndexes = new List<int>();

            foreach (var square in _gridSquares)
            {
                var gridSquare = square.GetComponent<GridSquare>();

                if (gridSquare.Selected && !gridSquare.SquareOccupied)
                {
                    squareIndexes.Add(gridSquare.SquareIndex);
                    gridSquare.Selected = false;
                    // gridSquare.ActivateSquare();
                }

            }

            var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
            if (currentSelectedShape == null) return;

            if (currentSelectedShape.TotalSquareNuber == squareIndexes.Count)
            {
                foreach (var squareIndex in squareIndexes)
                {
                    _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
                }

                var shapeLeft = 0;

                foreach (var shape in shapeStorage.shapeList)
                {
                    if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                    {
                        shapeLeft++;
                    }
                }

                
                
                if (shapeLeft == 0)
                {
                    GameEvents.RequestNewShape();
                }
                else
                {
                    GameEvents.SetShapeInActive();
                }
            }
            else
            {
                GameEvents.MoveShapeToStartPosition();
            }

        }

    }
}
