using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotParty.ScreenManager;

namespace RobotParty
{

    public interface IinputManager
    {
        List<string> onInput();
    }

    public interface Ielementvisitor
    {
        void onCharacter(Character character, ScreenManager screenmanager);
        void onProjectile(Projectile projectile, float dt);
        void onScreenmanager(ScreenManager screenmanager, float dt);
    }

    public interface elementvisitor {
        void onCharacter(Character character);
        void onProjectile(Projectile projectile);
        void onScreenmanager(ScreenManager screenmanager);
    }

    // implement onchar, onproj
    public class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;
        List<Ielement> newlist = new List<Ielement>();

        public UpdateVisitor(IinputManager inputmanager) {
            this.inputmanager = inputmanager;
        }

        public void onCharacter(Character character, ScreenManager screenmanager)
        {
            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left"); }
                if(el == "D") { character.Move("right"); }
                if(el == "W") { character.Move("up"); }
                if(el == "S") { character.Move("down"); }
                if (el == "Up") {
                    Console.WriteLine("up");
                    var directionX = 0;
                    var directionY = -1;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    directionX = 0;
                    directionY = 0;
                }
                if (el == "Down") {
                    Console.WriteLine("down");

                    var directionX = 0;
                    var directionY = 1;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    directionX = 0;
                    directionY = 0;
                }
                if (el == "Right") {
                    Console.WriteLine("right");

                    var directionX = 1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    directionX = 0;
                    directionY = 0;
                }
                if (el == "Left") {
                    Console.WriteLine("left");

                    var directionX = -1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    directionX = 0;
                    directionY = 0;
                }            
            }
        }

        public void onProjectile(Projectile projectile, float dt) {
            projectile.position = new Tuple<int, int>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
        }

        public void onScreenmanager(ScreenManager screenmanager, float dt)
        {
            foreach(Ielement el in screenmanager.elements) {
                el.Update(this, dt);
                Console.WriteLine(el.ToString());
            }
            foreach(Ielement el in newlist) {
                screenmanager.addElement(el);
            }
        }

        
    }

    // implement onchar, onproj, onscreen
    class DrawVisitor : Ielementvisitor
    {
        IDrawManager drawmanager;

        public DrawVisitor(IDrawManager drawmanager) {
            this.drawmanager = drawmanager;
        }

        public void onCharacter(Character Character, ScreenManager screenmanager)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.White);
        }

        public void onProjectile(Projectile Projectile, float dt)
        {
                var point = new Microsoft.Xna.Framework.Point(Projectile.position.Item1, Projectile.position.Item2);
                drawmanager.drawRectangle(point, 5, 5, Colour.White);
            
        }

        public void onScreenmanager(ScreenManager ScreenManager, float dt)
        {

            foreach (Ielement el in ScreenManager.elements) {
                el.Draw(this, dt);
            }
        }
    }
}
