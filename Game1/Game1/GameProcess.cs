using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System;


namespace ABC
{
    class GameProcess
    {
        public bool IsWin; // была ли победа (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsLose; // было ли поражение (к полю можно обратиться извне, но нельзя изменить извне)
        public bool IsGameLearn; // идет ли игровой процесс-по одной букве вручную  (к полю можно обратиться извне, но нельзя изменить извне) 
        public bool IsPause; // включена ли пауза
        public bool IsDrag;
        public Vector2 Delta;
        public static List<Letter> Letters = new List<Letter>(); //массив букв
        public int LetterIndex {get; set;}
        public GameProcess()
        {
            IsGameLearn = false;
            IsDrag = false;

        }

        public GameProcess(int currWidth, int currHeight, bool reverse=false)
        {
            IsGameLearn = true;
            IsDrag = false;
            int i = 0; 
            //задание начальных координат для букв
            //первую показываем в центре
            if (!reverse)
            {
                
                foreach (Letter let in Letters)
                {
                    try
                    {
                        let.Screenpos.X = currWidth / 2 - let.Letterpng.Width / 2 - currWidth * i;
                        let.Screenpos.Y = currHeight / 2 - let.Letterpng.Height / 2;
                        i = 1;
                    }
                    catch (Exception ex)
                    { }
                }
                LetterIndex = 0;
            }
            else
            {
                for (int j=Letters.Count-1;j>=0;j--)
                {
                    try
                    {
                        Letters[j].Screenpos.X = currWidth / 2 - Letters[j].Letterpng.Width / 2 + currWidth * i;
                        Letters[j].Screenpos.Y = currHeight / 2 - Letters[j].Letterpng.Height / 2;
                        i = 1;
                    }
                    catch (Exception ex)
                    { }
                }
                LetterIndex = Letters.Count - 1;
            }

        }

        public void Process(GameTime gametime)
        {
            IsDrag = false;
            if (TouchPanel.IsGestureAvailable)
            {
                // Read the next gesture    
                GestureSample gesture = TouchPanel.ReadGesture();
                if (gesture.GestureType == GestureType.Flick )
                {
                    IsDrag = true;
                    Delta = gesture.Delta;
                }
                
            }
        }
    }
}