using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeIO
{
    class Settings : Input
    {

        public static int Width { get; set; } = 16;
        public static int Height { get; set; } = 16;
        public static int Speed { get; set; } = 20;
        public static int Score { get; set; } = 0;
        public static int Points { get; set; } = 100;
        public static bool GameOver { get; set; } = false;
        public static Direc Directions { get; set; } = Direc.Right;



    }
}
