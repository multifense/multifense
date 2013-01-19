using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{
    abstract class GameObject
    {
        public GameSession gameSession;

        public SpriteFont font1text;
        public string strouput = "Error";
        public Vector2 setOutPutVector;
        //Defines the type of a sprite - Monster,Ermac,Tower
        public string type;
        public int typeID;
        public Color color;

        //Sprite'en som ritas ut
        private Texture2D txt2D;
        /// <summary>
        /// Get or Set Texturesprite i 2D
        /// </summary>
        public Texture2D Text2D { get { return txt2D; } set { txt2D = value; } }

        
        private Rectangle rec;
        /// <summary>
        /// Get or set Rectangle (Its the sprite)
        /// </summary>
        public Rectangle Rec { get { return rec; } set { rec = value; } }

        /// <summary>
        /// Call to draw the Sprite Onto the screen
        /// </summary>
        /// <param name="spriteBatch">The grahic spriteBatch</param>
        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            if (color.ToVector3().Length() == 0)
                spriteBatch.Draw(Text2D, new Rectangle(Rec.X + x, Rec.Y + y, Rec.Width, Rec.Height), Color.White);
            else
                spriteBatch.Draw(Text2D, new Rectangle(Rec.X + x, Rec.Y + y, Rec.Width, Rec.Height), color);
            if (font1text != null)
                if (setOutPutVector.Length() == 0)
                    spriteBatch.DrawString(font1text, strouput, new Vector2(this.rec.X + x, this.rec.Y + y), Color.Black);
                else
                    spriteBatch.DrawString(font1text, strouput, setOutPutVector, Color.Black);
        }
        abstract public bool Update(ref List<GameObject> listToPrint);
        abstract public void SetGameSession(GameSession gameSession);
    }
}