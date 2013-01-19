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

namespace C_SharpClient_1._1.GameObjects
{
    class Portals : GameObject
    {
        public Portals(Texture2D txture, Rectangle Rec, Rectangle scaleRec)
        {
            base.Text2D = txture;
            base.Rec = Rec;
            base.ScaleRectangle = scaleRec;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            throw new NotImplementedException();
        }
        private int counter = 0;
        private int currentFrame = 0;
        public override bool Update(ref List<GameObject> listToPrint)
        {
            counter++;
            if (counter > 2)
            {
                currentFrame++;
                if (currentFrame > (base.Text2D.Width / base.ScaleRectangle.Width) - 1)
                    currentFrame = 0;
                // just update to next frame
                base.ScaleRectangle = new Rectangle((currentFrame * base.ScaleRectangle.Width), 0, base.ScaleRectangle.Width, base.ScaleRectangle.Height);
            }
            return false;
        }
    }
}
