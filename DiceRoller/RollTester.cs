/*
 * =========================================================
 *     William Pyle 2020 (wpyle.com)
 * =========================================================
 */
using UnityEngine;

namespace DiceRoller
{
    public class RollTester : MonoBehaviour
    {
        public int num;
        public DieSize size;
        public int modifier;

        [Space]
        
        
        public int result;
        public int minPossible;
        public int maxPossible;
        

        [EditorButton]
        private void Roll()
        {
            var rollInfo = DiceRoller.GetRollInfo(num, size, modifier);
            result = rollInfo.result;
            minPossible = rollInfo.minRoll;
            maxPossible = rollInfo.maxRoll;
        }
    }
}
