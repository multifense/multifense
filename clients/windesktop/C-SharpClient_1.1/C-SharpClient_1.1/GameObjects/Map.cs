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
    class Map : GameObject
    {
        public enum Stat
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            END
        }
        /// <summary>
        /// Simple class to Mark the path
        /// </summary>
        public class Path
        {
            public Path(int walkPixels, Stat state)
            {
                this.state = state;
                this.walkPixels = walkPixels;
            }
            public Path(int x, int y, int walkPixels, Stat state)
            {
                this.x = x;
                this.y = y;
                this.state = state;
                this.walkPixels = walkPixels;
            }
            public int x;
            public int y;
            public int walkPixels;
            public Stat state;
            
        }
        //To store the path
        private Path[] p = new Path[12];

        public Map()
        {
            base.type = "Map";
            p[0] = new Path(-50, 125, 210, Stat.RIGHT);
            p[1] = new Path(135, Stat.DOWN);
            p[2] = new Path(590, Stat.RIGHT);
            p[3] = new Path(0, Stat.END);
        }

        /// <summary>
        /// It creates a sprite-map, with a type, texture, and is defined as a rectangle
        /// and creates a path for it.
        /// </summary>
        /// <param name="txt2D"></param>
        /// <param name="rec"></param>
        /// <param name="pathen"></param>
        public Map(Texture2D txt2D, Rectangle rec)
        {
            base.type = "Map";
            base.Text2D = txt2D;
            base.Rec = rec;
            //Uses for knowing the path.
        }

        public override bool Update(ref List<GameObject> listToPrint)
        {
            return false;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
        }
    }
}
