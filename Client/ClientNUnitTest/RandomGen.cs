using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientNUnitTest
{
    internal class RandomGen
    {
        public Random Rand { get; set; }
        /// <summary>
        /// setting up a instans of random to be used 
        /// inted of createing one every time a metod is called
        /// </summary>
        public RandomGen()
        {
            this.Rand = new Random(DateTime.Now.Millisecond);
        }
        /// <summary>
        /// Returns a string of the lentgt given as a inparamter
        /// The string is random and consists of askii characters
        /// </summary>
        /// <param name="length"> the lengt of the string to be returned</param>
        /// <returns> a string of random chars, of lengt given</returns>
        public string Gen_String(int length)
        {
            string randomStr = string.Empty;
            for (int i = 0; i < length; i++)
            {
                randomStr.Append(Convert.ToChar(Gen_Int()));
            }
            return randomStr;
        }
        /// <summary>
        /// Returns a random int betwan 0-127
        /// </summary>
        /// <returns>int betwan 0-127</returns>
        public int Gen_Int()
        {
            int random = Rand.Next(0, 127);
            return random;
        }


    }
}
