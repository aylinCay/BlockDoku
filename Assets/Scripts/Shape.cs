using System;
using UnityEngine.EventSystems;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Shape : MonoBehaviour,IPointerClickHandler,IPointerUpHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerDownHandler
    {
        public GameObject squareShapeImage;
        public Vector3 shapeSelectedScale;
        public Vector2 offset = new Vector2(0f, 700f);
        
        [HideInInspector]
        public ShapeData CurrentShapeData;
        
        public int TotalSquareNumber { get; set; }

        private List<GameObject> _currentShape = new List<GameObject>();
        private Vector3 _shapeStartScale;
        private RectTransform _transform;
        private bool _shapeDraggable = true;
        private Canvas _canvas;
        private Vector3 _startPosition;
        private bool _shapeActive = true;
        
        public void Awake()
        {
            _shapeStartScale = this.GetComponent<RectTransform>().localScale;
            _transform = this.GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _shapeDraggable = true;
            _startPosition = _transform.localPosition;
            _shapeActive = true;

        }

        public void OnEnable()
        {
            GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
            GameEvents.SetShapeInActive += SetShapeInActive;
        }

        public void OnDisable()
        {
            GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
            GameEvents.SetShapeInActive -= SetShapeInActive;
        }

        public bool IsOnStartPosition()
        {
            return _transform.localPosition == _startPosition;
        }

        public bool IsAnyOfShapeSquareActive()
        {
            foreach (var square in _currentShape)
            {
                if (square.gameObject.activeSelf)
                    return true;

            }

            return false;
        }

        public void DeactivateShape()
        {
            if (_shapeActive)
            {
                foreach (var square in _currentShape)
                {
                    square?.GetComponent<ShapeSquare>().DeactivateShape();
                    
                }
            }

            _shapeActive = false;
        }

        public void SetShapeInActive()
        {
            if (IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
            {
                foreach (var square in _currentShape)
                {
                    square.gameObject.SetActive(false);
                    
                }
            }
        }

        public void ActivateShape()
        {
            if (!_shapeActive)
            {
                foreach (var square in _currentShape)
                {
                    square?.GetComponent<ShapeSquare>().ActivateShape();
                }
            }

            _shapeActive = true;
        }

        public void RequestNewShape(ShapeData shapeData)
        {
            _transform.localPosition = _startPosition;
            CreateShape(shapeData);
        }

        public void CreateShape(ShapeData shapeData)
        {
            CurrentShapeData = shapeData;
            TotalSquareNumber = GetNumberOfSquares(shapeData);

            while (_currentShape.Count <= TotalSquareNumber)
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

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
        }
        public void OnDrag(PointerEventData eventData)
        {
            _transform.anchorMin = new Vector2(0f, 0f);
            _transform.anchorMax = new Vector2(0f, 0f);
            _transform.pivot = new Vector2(0f, 0f);

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform,
                eventData.position, Camera.main, out pos);
            _transform.localPosition = pos + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.GetComponent<RectTransform>().localScale = _shapeStartScale;
            GameEvents.CheckIfShapeCanBePlaced();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        private void MoveShapeToStartPosition()
        {
            _transform.transform.localPosition = _startPosition;
        }

    }

}