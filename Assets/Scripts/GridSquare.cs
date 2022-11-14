namespace BlockDoku
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class GridSquare : MonoBehaviour
    {
        public Image gridImage;
        public List<Sprite> gridImages;
        void Start()
        {

        }

        public void SetImage(bool setFistImage)
        {
            gridImage.GetComponent<Image>().sprite = setFistImage ? gridImages[1] : gridImages[0];
        }
    }

}