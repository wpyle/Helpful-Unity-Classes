/*
 * =========================================================
 *     Simple class for rolling dice of traditional sizes
 *     William Pyle 2020 (wpyle.com)
 * =========================================================
 */

using System.Collections.Generic;
using UnityEngine;

namespace DiceRoller
{
    public enum DieSize : int
    {
        d2   = 2,
        d4   = 4,
        d6   = 6,
        d8   = 8,
        d10  = 10,
        d12  = 12,
        d20  = 20,
        d100 = 100,
    }

    /// <summary>
    /// Object for passing around data about a given roll
    /// </summary>
    public class RollInfo
    {
        public readonly int dieNum;
        public readonly DieSize dieSize;
        public readonly int modifier;
        public readonly int minRoll;
        public readonly int maxRoll;
        public readonly int result;
        
        public RollInfo(int dieNum, DieSize dieSize, int modifier)
        {
            this.dieNum = dieNum;
            this.dieSize = dieSize;
            this.modifier = modifier;
            
            minRoll = dieNum + modifier;
            maxRoll = ((int)dieSize * dieNum) + modifier;
            result = DiceRoller.RollDice(dieNum, dieSize, modifier);
        }
    }
    
    public static class DiceRoller
    {
        
        /// <summary>
        /// Roll (num) dice of size (size).
        /// </summary>
        /// <param name="num">Number of dice to roll.</param>
        /// <param name="size">Size of dice to roll.</param>
        /// <returns></returns>
        public static int RollDice(int num, DieSize size)
        {
            var value = 0;
            for (var i = 0; i < num; i++)
            {
                value += GetRollResult(size);
            }
            return value;
        }
        
        /// <summary>
        /// Roll (num) dice of size (size) plus (modifier).
        /// </summary>
        /// <param name="num">Number of dice to roll.</param>
        /// <param name="size">Size of dice to roll.</param>
        /// <param name="modifier">Modifier to add to roll.</param>
        /// <returns></returns>
        public static int RollDice(int num, DieSize size, int modifier) 
            => RollDice(num, size) + modifier;
        
        /// <summary>
        /// Returns a class that contains more info about a roll.
        /// Can get min / max values from roll as well as the result of the roll.
        /// </summary>
        /// <param name="num">Number of dice to roll.</param>
        /// <param name="size">Size of dice to roll.</param>
        /// <param name="modifier">Modifier to add to roll.</param>
        /// <returns></returns>
        public static RollInfo GetRollInfo(int num, DieSize size, int modifier) 
            => new RollInfo(num, size, modifier);
        
        /// <summary>
        /// Returns if a roll passed a certain DC check. Will return false
        /// if the roll result is below the DC.
        /// </summary>
        /// <param name="num">Number of dice to roll.</param>
        /// <param name="size">Size of dice to roll.</param>
        /// <param name="modifier">Modifier to add to roll.</param>
        /// <param name="DC">Difficulty of the check.</param>
        /// <returns></returns>
        public static bool RollAgainstDC(int num, DieSize size, int modifier, int DC) 
            => RollDice(num, size, modifier) >= DC;
        
        //Private class for performing individual rolls.
        private static int GetRollResult(DieSize size) 
            => UnityEngine.Random.Range(1, (int)size + 1);
        
    }
}