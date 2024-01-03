using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classe
{
    public class MessageAccueil
    {
        public static void Menu()
        {
            string text = @"

  ______                 _____                 
 |  ____|               / ____|                
 | |__   __ _ ___ _   _| (___   __ ___   _____ 
 |  __| / _` / __| | | |\___ \ / _` \ \ / / _ \
 | |___| (_| \__ \ |_| |____) | (_| |\ V /  __/
 |______\__,_|___/\__, |_____/ \__,_| \_/ \___|
                   __/ |                       
                  |___/                        
";
            Console.WriteLine(text);
        }
    }
}
