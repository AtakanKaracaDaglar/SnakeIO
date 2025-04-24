using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeIO
{
    class Input
    {
        public enum Direc {Up, Down, Left, Right}

        public static Dictionary<Keys, Direc> keyTable = new Dictionary<Keys, Direc>() 
        {
            {Keys.W, Direc.Up},
            {Keys.S, Direc.Down},
            {Keys.A, Direc.Left},
            {Keys.D, Direc.Right }
        };


        public static HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public static void KeyDown(Keys k)
        {

            if (!pressedKeys.Contains(k))
            {
                pressedKeys.Add(k);
                Console.WriteLine("BASILDI: " + k); // TEST SATIRI
            }
        }

        public static void KeyUp(Keys k)
        {
            if (pressedKeys.Contains(k))
            {
                pressedKeys.Remove(k);
                Console.WriteLine("BIRAKILDI: " + k); // Test için
            }
        }



        public static bool IsKeyPressed(Keys k)
        {
            return pressedKeys.Contains(k);
        }


    }
}
