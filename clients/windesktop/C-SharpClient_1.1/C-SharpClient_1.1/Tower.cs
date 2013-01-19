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
    class Tower : GameObject
    {
        //Money it costs.
        public int price;
        //Tells the type of the tower
        public string typeOfTower;
        // skada tornet gör
        private double damage;
        //hur snabbt tornet skjuter
        private int attackCoolDown;
        //hur långt tornet ska kunna skjuta
        private double range;
        //The object for this tracking projectile
        private TrackingProjectile proj;
        public TrackingProjectile TrackingPorj { get { return proj; } set { proj = value; } }
        //to see if a tower is currently aiming at a specific monster
        public Monster myCurrentTarget;
        //The projectile towers uses
        private Texture2D trackProjtxt2D;

        private int attackUpdateCounter = 0;

        public Tower(Texture2D txt2D, Rectangle rec, int fireRate, double damage, double range, Texture2D trackProjtxt2D, int price)
        {
            this.price = price;
            attackUpdateCounter = fireRate;
            base.type = "Tower";
            base.Text2D = txt2D;
            base.Rec = rec;
            this.attackCoolDown = fireRate;
            this.damage = damage;
            this.range = range;
            this.trackProjtxt2D = trackProjtxt2D;
        }
        /// <summary>
        /// Uppdate the tower
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="listToPrint"></param>
        public override bool Update(ref List<GameObject> listToPrint)
        {
            //It gets incresed with 33 each time!
            attackUpdateCounter++;
            if (attackUpdateCounter >= FireRate())
            {
                if (SelectTarget(this))
                {
                    attackUpdateCounter -= FireRate();
                    proj = new TrackingProjectile(base.Rec.X + base.Rec.Width / 2, base.Rec.Y + base.Rec.Height / 2, 15, myCurrentTarget, damage, trackProjtxt2D);
                }
                else
                {
                    if (SelectTarget(this, listToPrint))
                    {
                        attackUpdateCounter -= FireRate();
                        proj = new TrackingProjectile(base.Rec.X + base.Rec.Width / 2, base.Rec.Y + base.Rec.Height / 2, 15, myCurrentTarget, damage, trackProjtxt2D);
                    }
                    else
                    {
                        attackUpdateCounter = FireRate();
                    }
                }
            }
            return false;
        }

        public double Damage()
        {
            return damage;
        }
        public int FireRate()
        {
            return attackCoolDown;
        }
        
        /// <summary>
        /// this functions is called when monster are on the map and a tower is not occupied
        /// check all sprites in listToPrint and computes if the monster is in tower range.
        /// The problem with tis is that a monster that is faster may not be targeted due to a monster being earlier in listToPrint 
        /// </summary>
        /// <param name="tow">tower that is loaded from tower</param>
        /// <param name="listToPrint">a list with all objects on map</param>
        public bool SelectTarget(Tower tow)
        {
            if (myCurrentTarget == null)
                return false;

            return (myCurrentTarget.Alive && DistanceToMonster(tow, myCurrentTarget) <= range);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tow"></param>
        /// <param name="listToPrint"></param>
        /// <returns></returns>
        private bool SelectTarget(Tower tow, List<GameObject> listToPrint)
        {
            foreach (GameObject item in listToPrint)
            {
                if (item.type == "Monster")
                {
                    Monster mon = (Monster)item;

                    if (DistanceToMonster(tow, mon) <= range)
                    {
                        // calls the shoot function to make sure that a shoot is fired at the fisrt update as well
                        //shoot(tow, mon);
                        //Make the tower occupied with a monster
                        myCurrentTarget = mon;
                        return true;
                    }

                }
            }
            return false;
        }

        public object Clone(int x, int y)
        {
            base.Rec = new Rectangle(x, y, base.Rec.Width, base.Rec.Height);
            return this.MemberwiseClone();
        }
        /// <summary>
        /// this method calculate the distance between two objects of any kind is possible by using pythagoras theorem
        /// </summary>
        /// <param name="tow">object tower</param>
        /// <param name="mon">object monster</param>
        /// <returns></returns>
        public double DistanceToMonster(GameObject tow, GameObject mon)
        {
            //avstånd från tornets mittpunkt till monstrets mittpunkt i X-led
            double distanceToMonsterX;
            //avstånd från tornets mittpunkt till monstrets mittpunkt i Y-led
            double distanceToMonsterY;
            
            distanceToMonsterX = (tow.Rec.X + (tow.Rec.Width / 2)) - (mon.Rec.X + (mon.Rec.Width / 2));
            distanceToMonsterY = (tow.Rec.Y + (tow.Rec.Height / 2)) - (mon.Rec.Y + (mon.Rec.Height / 2));
            double tmp = Math.Sqrt(Math.Pow(distanceToMonsterX, 2) + Math.Pow(distanceToMonsterY, 2));
            return tmp;
        }
        public override void SetGameSession(GameSession gameSession)
        {
            base.gameSession = gameSession;
        }
    }
}