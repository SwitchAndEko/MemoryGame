/*
 * Memory Flip Card Games
 * Author:Switch_Squirrel
 * Date:05/13/2018
 * The Copyright of the cards' picture is belong to KADOKAWA Animation(No Game No Life Zero-Japan Bonus Gift).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MemoryGame.Properties;

namespace MemoryGame
{
    class Card : Button
    {
        private Bitmap realImage { get; set; } // 卡牌實際照片
        private Bitmap coverImage { get; set; } // 封面，每張卡牌都一樣
        private int ID { get; set; } // 卡牌的ID，用以配對卡片
        private int Direction { get; set; } // 卡牌在遊戲開始時的排序
        
        public Card()
        {
            SetCoverImage();
        }

        internal Bitmap GetRealImage()
        {
            return realImage;
        }

        internal Bitmap GetCoverImage()
        {
            return coverImage;
        }

        internal int GetID()
        {
            return ID;
        }

        internal int GetDirection()
        {
            return Direction;
        }

        internal void SetCoverImage()
        {
            coverImage = Resource1.cover;
        }

        internal void SetID(int ID)
        {
            this.ID = ID;
        }

        internal void SetRealImage(Bitmap realImage)
        {
            this.realImage = realImage;
        }

        internal void SetDirection(int Direction)
        {
            this.Direction = Direction;
        }
    }
}
