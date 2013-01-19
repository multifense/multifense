using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace C_SharpClient_1._1
{
    class GamePath
    {


        public class GamePathPoint
        {
            public GamePathPoint next;
            public Vector2 dir;
            public double length;
            public Vector2 p;


            public GamePathPoint(Vector2 p)
            {
                this.p = p;
            }

            public GamePathPoint(Vector2 p, Vector2 dir, double length)
            {
                this.p = p;
                this.dir = dir;
                this.length = length;
            }

             
            public void ConnectWithPathPoint(GamePathPoint GPP)
            {
                this.next = GPP;
                this.dir = GPP.p - this.p;
                this.length = Math.Sqrt(Math.Pow((GPP.p.X - this.p.X), 2) + Math.Pow((GPP.p.Y - this.p.Y), 2));
                this.dir.Normalize();
            }


            public void SetNext(GamePathPoint GPP)
            {
                this.next = GPP;
            }
        }


        //GamePath class begins here

        GamePathPoint pathEnd;
        GamePathPoint pathStart;

        public void Initialize()
        {

        }

        public void AddPathPoint(int x, int y)
        {
            AppendPathPoint(new GamePathPoint(new Vector2(x,y)));
        }

        public bool LoadPathFromFile(string name)
        {

            return false;
        }

        private void AppendPathPoint(GamePathPoint GPP)
        {
            if (pathStart == null)
            {
                pathStart = GPP;
                pathEnd = GPP;
            }
            else
            {
                pathEnd.next = GPP;
                pathEnd = GPP;
            }


        }

    }
}
