using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.Backend
{
    class PayPerPlay
    {
#region Properties

        /// <summary>
        /// Specifies is PayPerPlay is enforced
        /// </summary>
        public static bool PayPerPlayEnabled;

        /// <summary>
        /// Specifies the number of coins required if payperplay is enabled
        /// </summary>
        public static int CoinsRequired;

        /// <summary>
        /// Specifies the current number of coins
        /// </summary>
        public static int CurrentCoins;

        /// <summary>
        /// Speficies the allowed amount of playtime if PayPerPlay is enabled
        /// </summary>
        public static int Playtime;

        #endregion

        #region Public Methods

        /// <summary>
        /// Upon a sucuessful launch method, this method will decrement the current coin count
        /// by the number of coins required for a launch
        /// </summary>
        public static void DecrementCoins()
        {
            CurrentCoins = CurrentCoins - CoinsRequired;
        }

#endregion
    }
}
