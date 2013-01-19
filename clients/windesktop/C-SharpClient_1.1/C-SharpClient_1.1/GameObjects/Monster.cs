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
    class Monster : GameObject
    {
        GameSession.GamePath.GamePathPoint lastPoint;
        double currDistance = 0;

        public SpriteFont setHpBar { set { base.font1text = value; } }

        private bool isKilledByPlayer = false;
        public bool IsKileldByPlayer { get { return isKilledByPlayer; } }

        public int damage;
        public double hp;
        public int maxHP;
        private int speed;
        public int maxSpeed;
        public int cost = 0;
        public int ownerID;
        Vector2 position;
        public int Speed { get { return speed; } set { speed = value; } }
        private bool isAlive = true;
        public bool Alive { get { return isAlive; } set { isAlive = value; } }

        /// <summary>
        /// This is just for testing
        /// </summary>
        /// <param name="rec">Rectangle</param>
        /// <param name="hp">Helth Points</param>
        public Monster(Rectangle rec, double hp, int speed)
        {
            base.Rec = rec;
            base.type = "Monster";
            this.speed = speed;
            this.hp = hp;
        }

        ///  To construct a sprite you need this:
        /// </summary>
        /// <param name="txt2D">Textture 2D (Must be Loaded from content)</param>
        /// <param name="X">X position on screen</param>
        /// <param name="Y">Y Position on screen</param>
        /// <param name="map"> Monsters needs a path to walk on the map</param>
        public Monster(Texture2D txt2D, double hp, int speed)
        {
            this.maxSpeed = speed;
            this.maxHP = (int)hp;
            base.type = "Monster";
            this.speed = speed;
            base.Text2D = txt2D;
            base.Rec = new Rectangle(0, 0, txt2D.Width, txt2D.Height);
            this.hp = hp;
        }

        /// <summary>
        /// To construct a sprite you need this:
        /// </summary>
        /// <param name="txt2D">Textture 2D (Must be Loaded from content)</param>
        /// <param name="rec">Rectangle have both wigth heigth and Position</param>
        public Monster(Texture2D txt2D, Rectangle rec, double hp, int speed)
        {
            base.Text2D = txt2D;
            base.Rec = rec;
            this.maxSpeed = speed;
            this.maxHP = (int)hp;
            base.type = "Monster";
            this.speed = speed;
            this.hp = hp;
        }


        /// <summary>
        /// Lower the monster HP
        /// </summary>
        /// <param name="damage">The ammoint of damage the monster takes</param>
        /// <returns>Returns HP left</returns>
        public double ReduceHP(double damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                isAlive = false;
                isKilledByPlayer = true;
            }
            return hp;
        }

        private int nextFrame = 0;
        /// <summary>
        /// The Movement Speed of the monster
        /// </summary>
        /// <returns>The current speed of the monster</returns>
        public override bool Update(ref List<GameObject> listToPrint)
        {
            nextFrame++;
            if (nextFrame > 1)
            {
                nextFrame = 0;
                if (Math.Round(this.lastPoint.dir.X) == -1)
                    SetAnimation(3);
                if (Math.Round(this.lastPoint.dir.X) == 1)
                    SetAnimation(2);
                if (Math.Round(this.lastPoint.dir.Y) == 1)
                    SetAnimation(1);
                if (Math.Round(this.lastPoint.dir.Y) == -1)
                    SetAnimation(0);
            }

            base.strouput = Convert.ToString(hp);
            if (this.lastPoint != null)
                move();
            return !isAlive;
        }

        public Monster Clone()
        {
            return (Monster)this.MemberwiseClone();
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
            this.lastPoint = gameSession.gamePath.PathStart;
            currDistance = this.lastPoint.length;
            position = this.lastPoint.p;
            position.X -= 25F;
            position.Y -= 25F;
            base.Rec = new Rectangle((int)lastPoint.p.X -25, (int)lastPoint.p.Y -25, base.Rec.Width, base.Rec.Height);
        }

        private int currentFrame = 0;
        private void SetAnimation(int stateID)
        {
            currentFrame++;
            if (currentFrame > (base.Text2D.Width / base.ScaleRectangle.Width) - 1)
                currentFrame = 0;
            // just update to next frame
            base.ScaleRectangle = new Rectangle((currentFrame * base.ScaleRectangle.Width), (stateID * base.ScaleRectangle.Height), base.ScaleRectangle.Width, base.ScaleRectangle.Height);
        }

        // changes the monsters position
        public void move()
        {
            // Move monster position distance governed by its speed.
            moveDistance(speed);

            // If monster just passed the next waypoint
            if (currDistance <= 0)
            {
                if (lastPoint.next == null)
                {
                    // monster passed last point, time to despawn.
                    isAlive = false;
                    return;
                }
                lastPoint = lastPoint.next;
                // set new left, top to first waypoint coordinates and direction.
                position = this.lastPoint.p;
                position.X -= 25F;
                position.Y -= 25F;
                base.Rec = new Rectangle((int)this.lastPoint.p.X -25, (int)this.lastPoint.p.Y -25, base.Rec.Width, base.Rec.Height);

                moveDistance(-1 * currDistance);
                // Set currDistance to next length
                currDistance = lastPoint.length + currDistance;
            }
        }

        // unconditionally adjusts the monsters position
        public void moveDistance(double distance)
        {
            position.X += (float)distance * this.lastPoint.dir.X;
            position.Y += (float)distance * this.lastPoint.dir.Y;
            base.Rec = new Rectangle((int)position.X , (int)position.Y , base.Rec.Width, base.Rec.Height);
            // Adjusts the currDistance counter.
            currDistance = currDistance - distance;
        }
    }
}
