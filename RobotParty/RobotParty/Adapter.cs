using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotParty
{

    interface inputManager
    {
        List<string> onInput();
    }

    class PCInputAdapter : inputManager
    {
        List<string> keyslist;

        public List<string> onInput()
        {
            keyslist = new List<string>();
            var key = Keyboard.GetState();
            key.GetPressedKeys();
            if (key.IsKeyDown(Keys.A))
            {
                keyslist.Add("A");
            }
            if (key.IsKeyDown(Keys.W))
            {
                keyslist.Add("W");
            }
            if (key.IsKeyDown(Keys.S))
            {
                keyslist.Add("S");
            }
            if (key.IsKeyDown(Keys.D))
            {
                keyslist.Add("D");
            }
            if (key.IsKeyDown(Keys.Right))
            {
                keyslist.Add("Right");
            }
            if (key.IsKeyDown(Keys.Left))
            {
                keyslist.Add("Left");
            }
            if (key.IsKeyDown(Keys.Up))
            {
                keyslist.Add("Up");
            }
            if (key.IsKeyDown(Keys.Down))
            {
                keyslist.Add("Down");
            }

            return keyslist;

        }

    }
}