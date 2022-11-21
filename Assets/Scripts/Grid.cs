using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("_lineIndicator")] public LineIndicator lineIndicator;

        public void Start()
        {
            lineIndicator.GetComponent<LineIndicator>();
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
                    _gridSquares[_gridSquares.Count -1].GetComponent<GridSquare>().SetImage(lineIndicator.GetGridSquareIndex(_squareIndex) % 2 == 0);
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

            if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
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
                CheckIfAnyLineIsCompleted();
            }
            else
            {
                GameEvents.MoveShapeToStartPosition();
            }

        }

        void CheckIfAnyLineIsCompleted()
        {
            List<int[]> lines = new List<int[]>();

            foreach (var column in lineIndicator.columnIndexes)
            {
                lines.Add(lineIndicator.GetVerticalLine(column));
            }

            for (var row = 0; row < 9; row++)
            {
                List<int> data = new List<int>(9);

                for (var index = 0; index < 9; index++)
                {
                    data.Add(lineIndicator.lineData[row,index]);
                }
                lines.Add(data.ToArray());
            }

            for (var square = 0; square < 9; square++)
            {
                List<int> data = new List<int>(9);
                for (var index = 0; index < 9; index++)
                {
                    data.Add(lineIndicator.squareData[square,index]);
                }
                lines.Add(data.ToArray());
            }

            var completedLines = CheckIfSquareComleted(lines);

            if (completedLines > 2)
            {
                //plat bonus animation
            }
            var totalScores = 20 * completedLines;
            GameEvents.AddScores(totalScores);
            CheckIfPlayerLost();
        }

        private int CheckIfSquareComleted(List<int[]> data)
        {
            List<int[]> completedLines = new List<int[]>();

            var linesCompleted = 0;

            foreach (var line in data)
            {
                var lineCompleted = true;
                foreach (var squareIndex in line)
                {
                    var completed = _gridSquares[squareIndex].GetComponent<GridSquare>();
                    if (completed.SquareOccupied == false)
                    {
                        lineCompleted = false;
                    }
                }

                if (lineCompleted)
                {
                    completedLines.Add(line);
                }

            }

            foreach (var line in completedLines)
            {
                var completed = false;
                foreach (var squareIndex in line)
                {
                    var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                    comp.Deactive();
                    completed = true;
                }

                foreach (var squareIndex in line)
                {
                    var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                    comp.ClearOccupied();
                }

                if (completed)
                {
                    linesCompleted++;
                }
            }

            return linesCompleted;
        }

        private void CheckIfPlayerLost()
        {
            var validShapes = 0;
            
            for (var index = 0; index < shapeStorage.shapeList.Count; index++)
            {
                var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
                if (CheckIfShapeCanBePlaceOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
                {
                    shapeStorage.shapeList[index]?.ActivateShape();
                    validShapes++;
                }

            }

            if (validShapes == 0)
            {
                //GameOver
               // GameEvents.GameOver(false);
                Debug.Log("GameOver");
            }
        }

        private bool  CheckIfShapeCanBePlaceOnGrid(Shape currentShape)
        {
            var currentShapeData = currentShape.CurrentShapeData;
            var shapeColumns = currentShapeData.columns;
            var shapeRows = currentShapeData.rows;

            List<int> originalShapeFilledUpSquares = new List<int>();
            var squareIndex = 0;

            for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
                {

                    if (currentShapeData.board[rowIndex].column[columnIndex])
                    {
                        originalShapeFilledUpSquares.Add(squareIndex);
                    }

                    squareIndex++;
                }
            }
            if(currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
                Debug.LogError("Number of filled up squares are not the same as ");

            var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);

            bool canBePlaced = false;

            foreach (var number in squareList)
            {
                bool shapeCanBePlacedOntheBoard = false;

                foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
                {
                    var comp = _gridSquares[number[squareIndex]].GetComponent<GridSquare>();
                    if (comp.SquareOccupied)
                    {
                        shapeCanBePlacedOntheBoard = false;
                    }
                }

                if (shapeCanBePlacedOntheBoard)
                {
                    canBePlaced = true;
                }
            }

            return canBePlaced;
        }

        private List<int[]> GetAllSquaresCombination(int columns, int row)
        {
            var squareList = new List<int[]>();
            var lastColumnIndex = 0;
            var lastRowIndex = 0;

            int safeIndex = 0;

            while (lastRowIndex + (rows - 1) < 9)
            {
                var rowData = new List<int>();

                for ( row = lastRowIndex; row < lastRowIndex + rows; row++)
                {
                    for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                    {
                        rowData.Add(lineIndicator.lineData[row,column]);
                    }
                }
                
                squareList.Add(rowData.ToArray());
                lastColumnIndex++;

                if (lastColumnIndex + (columns - 1) >= 9)
                {
                    lastRowIndex++;
                    lastColumnIndex = 0;
                }

                safeIndex++;
                if (safeIndex > 100)
                {
                    break;
                }
            }

            return squareList;
        }

    }
}
