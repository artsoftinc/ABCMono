//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace ABC
{
    partial class ABCGame
    {
        public class Button
        {
            public int YUp;
            public int YDown;
            public int XRight;
            public int XLeft;
            // текстуры
            public Texture2D TextureButton;
            public Texture2D TextureButtonLight;


           

            private TouchCollection Touches;
            private bool IsPressed;
            public bool IsEnabled;

            public Button()
            {
                XRight = -100;
                XLeft = -100;
                YUp = -100;
                YDown = -100;
                IsPressed = false;
                IsEnabled = false;
            }

            public void Update(int xLeft, int yUp)
            {
                XRight = XLeft + TextureButton.Width;
                XLeft = xLeft;
                YUp = yUp;
                YDown = yUp + TextureButton.Height;
            }

            public void Process(SpriteBatch spriteBatch)
            {
                Touches = TouchPanel.GetState();
                if (Touches.Count == 1)
                {
                    if (!IsPressed)
                    {
                        spriteBatch.Draw(TextureButton, new Vector2(/*AbsoluteX(*/XLeft/*)*/, /*AbsoluteY(*/YUp/*)*/),
                                 new Rectangle(0, 0, TextureButton.Width, TextureButton.Height), Color.White, 0,
                                 new Vector2(0, 0),
                                 Dx, SpriteEffects.None, 0);
                    }
                    if ((Touches[0].Position.X > /*AbsoluteX(*/XLeft/*)*/) && (Touches[0].Position.X < /*AbsoluteX(*/XRight + 10/*)*/) &&
                        (Touches[0].Position.Y > /*AbsoluteX(*/YUp/*)*/) && (Touches[0].Position.Y < /*AbsoluteY(*/YDown + 10/*)*/))
                    {
                        spriteBatch.Draw(TextureButtonLight, new Vector2(/*AbsoluteX(*/XLeft/*)*/, /*AbsoluteY(*/YUp/*)*/),
                                         new Rectangle(0, 0, TextureButtonLight.Width, TextureButtonLight.Height), Color.White, 0,
                                         new Vector2(0, 0),
                                         Dx, SpriteEffects.None, 0);
                        IsPressed = true;
                    }
                    else
                    {
                        if (IsPressed) IsPressed = false;
                    }
                }
                else
                {
                    spriteBatch.Draw(TextureButton, new Vector2(/*AbsoluteX(*/XLeft/*)*/, /*AbsoluteY(*/YUp/*)*/),
                                 new Rectangle(0, 0, TextureButton.Width, TextureButton.Height), Color.White, 0,
                                 new Vector2(0, 0),
                                 Dx, SpriteEffects.None, 0);
                }
                if ((IsPressed) && (Touches.Count == 0))
                {
                    IsEnabled = true;
                }
            }
            public void Reset()
            {
                IsPressed = false;
                IsEnabled = false;
            }
        }
    }
}