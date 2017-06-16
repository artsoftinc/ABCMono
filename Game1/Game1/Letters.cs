using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace ABC
{
   
        public class Letter
        {
            public Vector2 Screenpos; // позиция на экране
            public Vector2 Center; // ось вращения
            public float Rotation; //поворот
            private Random Rnd; // счетчик случайных чисел
            private int IncreaseSpeedCount; // промежуточное поле для хранения счетчика увеличения скорости
            public Texture2D Letterpng;
            public string NameLetter;
            private TouchCollection Touches;

            // конструктор класса - действия, которые осуществляются при его создании (инициализации)
            public Letter(int x, int y, string nameLetter)
            {
                //Speed = 4;
                Center.X = 40;
                Center.Y = 40;
                //IsAlive = true;
                Screenpos.X = x;
                Screenpos.Y = y;
                NameLetter = nameLetter;
                
            }

            

        }
}
