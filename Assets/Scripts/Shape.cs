namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Shape : MonoBehaviour
    {
        public GameObject squareShapeImage;
        public ShapeData CurrentShapeData;

        private List<GameObject> _currentShape = new List<GameObject>();
        
        void Start()
        {
RequestNewShape(CurrentShapeData);
        }

        public void RequestNewShape(ShapeData shapeData)
        {
            CreateShape(shapeData);
        }

        public void CreateShape(ShapeData shapeData)
        {
            CurrentShapeData = shapeData;
            var _totalSquareNumber = GetNumberOfSquares(shapeData);

            while (_currentShape.Count <= _totalSquareNumber)
            {
                _currentShape.Add(Instantiate(squareShapeImage,transform)as GameObject);
            }

            foreach (var square in _currentShape)
            {
                square.gameObject.transform.position = Vector3.zero;
                square.gameObject.SetActive(false);
            }

            var _squareRect = squareShapeImage.GetComponent<RectTransform>();
            var _moveDistance = new Vector2(_squareRect.rect.width * _squareRect.localScale.x,_squareRect.rect.height * _squareRect.localScale.y);

            int _currentIndexInList = 0;

            for (var row = 0; row < shapeData.rows; row++)
            {
                for (var column = 0; column < shapeData.columns; column++)
                {
                    if (shapeData.board[row].column[column])
                    {
                        _currentShape[_currentIndexInList].SetActive(true);
                        _currentShape[_currentIndexInList].GetComponent<RectTransform>().localPosition =
                            new Vector2(GetXPositionForShapeSquare(shapeData, column, _moveDistance),
                                GetYPositionForShapeSquare(shapeData, row, _moveDistance));
                        _currentIndexInList++;
                    }
                }
            }
        }

        private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
        {
            float shiftOnY = 0f;

            if (shapeData.rows > 1)
            {
                if(shapeData.rows  % 2 != 0)
                {
                    var _middleSquareIndex = (shapeData.rows - 1) / 2;
                    var _multiplier = (shapeData.rows - 1) / 2;
                    
                    if (row < _middleSquareIndex)
                    {
                        shiftOnY = moveDistance.y * 1;
                        shiftOnY *= _multiplier;
                    }
                    else if (row > _middleSquareIndex)
                    {
                        shiftOnY = moveDistance.y * -1;
                        shiftOnY *= _multiplier;
                    }
                }
                else
                {
                    var _middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                    var _middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                    var _multiplier = shapeData.rows / 2;

                    if (row == _middleSquareIndex1 || row == _middleSquareIndex2)
                    {
                        if (row == _middleSquareIndex1)
                            shiftOnY = moveDistance.y / 2;
                        if (row == _middleSquareIndex2)
                            shiftOnY = (moveDistance.y / 2) * -1;
                    }

                    if (row < _middleSquareIndex1 && row < _middleSquareIndex2)
                    {
                        shiftOnY = moveDistance.y * 1;
                        shiftOnY *= _multiplier;
                    }
                    else if (row > _middleSquareIndex1 && row > _middleSquareIndex2)
                    {
                        shiftOnY = moveDistance.y * -1;
                        shiftOnY *= _multiplier;
                    }
                }
            }

            return shiftOnY; 
        }
            
        

        private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
        {
            float shiftOnX = 0f;

            if (shapeData.columns > 1)
            {
                if (shapeData.columns % 2 != 0)
                {
                    var _middleSquareIndex = (shapeData.columns - 1) / 2;
                    var _multiplier = (shapeData.columns - 1) / 2;
                    if (column < _middleSquareIndex)
                    {
                        shiftOnX = moveDistance.x * -1;
                        shiftOnX *= _multiplier;
                    }
                    else if (column > _middleSquareIndex)
                    {
                        shiftOnX = moveDistance.x * 1;
                        shiftOnX *= _multiplier;
                    }
                }
                else
                {
                    var _middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                    var _middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                    var _multiplier = shapeData.columns / 2;

                    if (column == _middleSquareIndex1 || column == _middleSquareIndex2)
                    {
                        if (column == _middleSquareIndex2)
                            shiftOnX = moveDistance.x / 2;
                        if (column == _middleSquareIndex1)
                            shiftOnX = (moveDistance.x / 2) * -1;
                    }

                    if (column < _middleSquareIndex1 && column < _middleSquareIndex2)
                    {
                        shiftOnX = moveDistance.x * -1;
                        shiftOnX *= _multiplier;
                    }
                    else if (column > _middleSquareIndex1 && column > _middleSquareIndex2)
                    {
                        shiftOnX = moveDistance.x * 1;
                        shiftOnX *= _multiplier;
                    }
                }
            }

            return shiftOnX;
        }

        private int GetNumberOfSquares(ShapeData shapeData)
        {
            int _number = 0;

            foreach (var rowData in shapeData.board)
            {
                foreach (var active in rowData.column)
                {
                    if (active)
                        _number++;

                }
            }

            return _number;
        }

    }

}