using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_SharpClient_1._1
{
    class Economy
    {
        //income multi and single player is increased later on by their respective function 10 is the amount of money recieved after first round + little extra
        public int sPIncome = 10;
        public int mPIncome = 10;
        //extra amount of money players income is increased for buying monsters in multiplayer
        // kanske ha senare private int mPMonsterMoney;

        public int getSPIncome(int rounds)
        {
            return sPIncome += rounds -1;
        }

        /// <summary>
        /// Get the ammount of money you get from one monster!
        /// </summary>
        /// <param name="mon">The monster you want to generate money from</param>
        /// <returns></returns>
        public int getMonsterMoney(Monster mon)
        {
            return Convert.ToInt32(Math.Pow(((double)mon.maxHP + (mon.maxHP / 3) * mon.maxSpeed) / 104, 0.8) * 3 - 2);
        }

        /// <summary>
        /// Increase the income based on what monster was bought 
        /// going to fix the buy function
        /// </summary>
        /// <param name="mon">the monster that was bought</param>
        /// <returns>the increased ammount of money</returns>
        public int BuyMonster(Monster mon)
        {
            return mPIncome += Convert.ToInt32(Math.Pow(((double)mon.maxHP + (mon.maxHP / 3 * mon.maxSpeed)) / 104, 0.8));
        }

        // return the income to a player done in begining or end of each round
        public int GetMultiPlayerIncome()
        {
            return mPIncome;
        }
    }
}
