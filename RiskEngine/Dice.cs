using System;
using System.Collections.Generic;
using System.Text;

namespace RiskEngine
{
    public class Dice
    {
        private int value { get; set; }
        //public CommonFunctions.FactionState state { get; }
        private int maxValue = 6;

        public Dice()
        {
            //this.state = desiredState;
        }

        /// <summary>
        /// Roll the dice, generating a number between 1 and 6.
        /// </summary>
        /// <returns></returns>
        public int Roll()
        {
            Random r = new Random();
            int result = r.Next(maxValue) + 1;
            this.value = result;
            return result;
        }

        /// <summary>
        /// Examine the current face-value of the dice.
        /// </summary>
        /// <returns></returns>
        public int PeekDice()
        {
            return this.value;
        }
    }
}
