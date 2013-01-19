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
    class TrackingProjectile : GameObject
    {
        double speed;
        Monster target;
        double damage;

        //Just for calculations in c#
        Vector2 bulletPosition;
        Vector2 bulletDirection;
        private int bulletSpeed;

        /// <summary>
        /// This one is just for testing
        /// </summary>
        public TrackingProjectile(Monster mo, float towX, float towY, double speed, double damage, int bulletSpeed)
        {
            this.bulletSpeed = bulletSpeed;
            this.target = mo;
            base.type = "Projectile";
            base.Rec = new Rectangle((int)towX, (int)towY, 20, 20);
            this.speed = speed;
            this.damage = damage;
        }

        public TrackingProjectile(float towX, float towY, double speed, Monster Target, double dmg, Texture2D texture, int bulletSpeed)
        {
            this.bulletSpeed = bulletSpeed;
            base.type = "Projectile";
            base.Rec = new Rectangle((int)towX, (int)towY, 20, 20);
            this.speed = speed;
            this.target = Target;
            this.damage = dmg;
            bulletPosition = new Vector2(towX, towY);
            base.Text2D = texture;
        }

        /// <summary>
        /// Update position of the 
        /// </summary>
        /// <returns>Returns true if hit monster and shall get removed</returns>
        public override bool Update(ref List<GameObject> listToPrint)
        {
            bulletDirection = new Vector2(target.Rec.X + target.Rec.Width/2, target.Rec.Y +target.Rec.Height/2);
            Vector2 tmpVector = bulletDirection - bulletPosition;
            tmpVector.Normalize();
            bulletDirection.Normalize();
            bulletPosition += (int)speed * tmpVector;
            base.Rec = new Rectangle((int)bulletPosition.X, (int)bulletPosition.Y, 20, 20);

            if (target.Rec.Intersects(base.Rec))
            {
                target.ReduceHP(damage);
                return true;
            }
            else
                return false;
        }

        private double getDistance(Monster Target)
        {
            double distance = Math.Sqrt((Math.Pow((Target.Rec.X + (Target.Rec.Width / 2)) - (base.Rec.X + (base.Rec.Width/2)), 2)) + (Math.Pow((Target.Rec.Y + (Target.Rec.Height / 2)) - (base.Rec.Y + (base.Rec.Height/2)), 2)));
            return distance;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
        }
    }
}
