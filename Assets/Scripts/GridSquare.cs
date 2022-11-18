using System;

namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class GridSquare : MonoBehaviour
    {
        public Image gridImage;
        public Image activeImage;
        public Image hooverImage;
        
        public List<Sprite> gridImages;
        
        public bool Selected { get; set; }
        public int SquareIndex { get; set; }
        public bool SquareOccupied { get; set; }
        
        void Start()
        {
            Selected = false;
            SquareOccupied = false;
        }

        public bool CanWeUseThisSquare()
        {
            return hooverImage.gameObject.activeSelf;
        }

        public void PlaceShapeOnBoard()
        {
            ActivateSquare();
        }

        public void ActivateSquare()
        {
            hooverImage.gameObject.SetActive(false);
            activeImage.gameObject.SetActive(true);
            Selected = true;
            SquareOccupied = true;
        }

        public void Deactive()
        {
            activeImage.gameObject.SetActive(false);
            
        }

        public void ClearOccupied()
        {
            Selected = false;
            SquareOccupied = false;
        }

        public void SetImage(bool setFistImage)
        {
            gridImage.GetComponent<Image>().sprite = setFistImage ? gridImages[1] : gridImages[0];
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (SquareOccupied == false)
            {
                Selected = true;
                hooverImage.gameObject.SetActive(true);
            }
            else if (col.GetComponent<ShapeSquare>() != null)
            {
                col.GetComponent<ShapeSquare>().SetOccupied();
            }
            
        }

        public void OnTriggerStay2D(Collider2D other)
        {
            Selected = true;
            if (SquareOccupied == false)
            {
               
                hooverImage.gameObject.SetActive(true);
            }
            else if (other.GetComponent<ShapeSquare>() != null)
            {
                other.GetComponent<ShapeSquare>().SetOccupied();
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (SquareOccupied == false)
            {
                Selected = false;
                hooverImage.gameObject.SetActive(false);
            }
            else if (other.GetComponent<ShapeSquare>() != null)
            {
                other.GetComponent<ShapeSquare>().UnSetUccupied();
            }
        }
    }

}