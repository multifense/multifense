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

    //inherits sprite
    class Hidden: GameObject
    {
        /// <summary>
        /// A class used for you to create rectagnles at areas you will not want to build
        /// The type is the cool and secret Ermac(hidden)
        /// </summary>
        /// <param name="rec">The area you want to be unable to build on!</param>
        public Hidden(Texture2D txt2D, Rectangle rec)
        {
            base.Text2D = txt2D;
            base.Rec = rec;
            base.type = "Ermac";
            base.color = Color.Transparent;
        }

        public override bool Update(ref List<GameObject> listToPrint)
        {
            /*if (!gameSession.ui.IsMouseVisable)
                base.color = new Color(50, 0, 0, 1);
            else
                base.color = Color.Transparent;
            */
            return false;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
        }
    }
}
