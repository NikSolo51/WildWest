using System.Collections.Generic;
using CodeBase.Logic.IndexCollector;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class SpriteSwitcher : ContainCurrentIndex
    {
        public Image Image;
        public List<Sprite> Sprites;

        public void NextImage()
        {
            if (currentIndex + 1 > Sprites.Count - 1)
            {
                currentIndex = 0;
                Image.sprite = Sprites[currentIndex];
                return;
            }

            currentIndex++;
            Image.sprite = Sprites[currentIndex];
        }

        public void PreviousImage()
        {
            if (currentIndex - 1 < 0)
            {
                currentIndex = Sprites.Count - 1;
                Image.sprite = Sprites[currentIndex];
                return;
            }

            currentIndex--;
            Image.sprite = Sprites[currentIndex];
        }
    }
}