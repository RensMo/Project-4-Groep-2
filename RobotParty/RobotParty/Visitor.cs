using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotParty.ScreenManager;

namespace RobotParty
{

    public interface IonCollision {
        bool Collision(Ielement element1, Ielement element2);
    }

    public class onCollision : IonCollision {
        bool collision;
        int el1X;
        int el1Y;
        int el2X;
        int el2Y;

        public bool Collision(Ielement element1, Ielement element2) {
            el1X = element1.getPos().Item1;
            el1Y = element1.getPos().Item2;
            el2X = element2.getPos().Item1;
            el2Y = element2.getPos().Item2;

            if(element1 is Character && element2 is Projectile) {
                el1Y += 25;
                el1X += 25;
                el2X += 2;
                el2Y += 2;
                if (el2X > el1X){
                    if (el2X < (el1X + 10)) {
                        if (el2Y > el1Y) {
                            if (el2Y < (el1Y + 20)) {
                                collision = true;
                            }
                        }
                    }
                }
            }

            //if(element1 is Character && element2 is Character) {
            //    if (el2X > el1X) {
            //        if (el2X < (el1X + 10)) {
            //            if (el2Y > el1Y) {
            //                if (el2Y < (el1Y + 20)) {
            //                    collision = true;
            //                }
            //            }
            //        }
            //    }
            //}
            // if projectile.X > character.X && projectile.X < Character.X + 60 && projectile.Y > Character.Y && projectile.Y < character.Y - 60

            //if(el1X >= el2X && el1Y >= el2Y && el1X <= el2X + 10 && el1Y <= el2Y + 10) {
            //    collision = true;
            //}
            else {
                collision = false;
            }
            return collision;
        }
    }

    public interface IinputManager
    {
        List<string> onInput();
    }

    public interface Ielementvisitor
    {
        void onMainCharacter(MainCharacter character, ScreenManager screenmanager);
        void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt);
        void onScreenmanager(ScreenManager screenmanager, float dt);
        void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager);
        void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager);
    }

    // implement onchar, onproj
    public class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;
        IonCollision collisioncalculator;
        List<Ielement> newlist = new List<Ielement>();
        List<Ielement> removelist = new List<Ielement>();

        public UpdateVisitor(IinputManager inputmanager, IonCollision collisioncalculator) {
            this.inputmanager = inputmanager;
            this.collisioncalculator = collisioncalculator;

        }

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager) {
            foreach (var direction in character.GetDirection()) {
                character.Move(direction);
            }
        }

        public void onMainCharacter(MainCharacter character, ScreenManager screenmanager)
        {

            if(character.health < 0) {
                Console.WriteLine("you lose");
            }

            foreach(var el in screenmanager.elements) {
                if(collisioncalculator.Collision(character, el)) {
                    // check if it's an enemy character
                    if(el is EnemyCharacter) {
                        character.health -= 50;
                    }
                    if(el is PickUpCharacter) {
                        character.health = 500;
                        removelist.Add(el);
                    }
                    // check if it's an enemy bullet
                }

            }

            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left"); }
                if(el == "D") { character.Move("right"); }
                if(el == "W") { character.Move("up"); }
                if(el == "S") { character.Move("down"); }
                     
            }

            foreach(var el in inputmanager.onInput()) { 
                if (el == "Up") {
                    var directionX = 0;
                    var directionY = -1;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY), screenmanager));
                    break;
                }
                if (el == "Down") {
                    var directionX = 0;
                    var directionY = 1;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY), screenmanager));
                    break;
                }
                if (el == "Right") {
                    var directionX = 1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY), screenmanager));
                    break;
                }
                if (el == "Left") {
                    var directionX = -1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY), screenmanager));
                    break;
                }
            }
        }

        public void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager) {
            //Nothing to update. Stays at same spot.

        }

        public void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt) {
            projectile.position = new Tuple<int, int>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
            foreach(var el in screenmanager.elements) {
                if (el is EnemyCharacter) { 
                    if (collisioncalculator.Collision(el, projectile)) {
                   
                        removelist.Add(el);
                        Console.WriteLine("enemy in list");
                    }
                }
            }
        }

        public void onScreenmanager(ScreenManager screenmanager, float dt)
        {
            foreach(Ielement el in screenmanager.elements) {
                if(el.getPos().Item1 < 0 || el.getPos().Item2 < 0 || el.getPos().Item1 > 800 || el.getPos().Item2 > 500) {
                    removelist.Add(el);
                }
                el.Update(this, dt);
            }
            int i = 0;
            foreach(Ielement el in newlist) {

                screenmanager.elements.Add(el);
                i += 1;
            }

            int x = 0;
            while (x < removelist.Count()) {
                screenmanager.elements.Remove(removelist[x]);
                x += 1;
                Console.WriteLine("enemy removed");

            }
            removelist = new List<Ielement>();
            newlist = new List<Ielement>();
            
        }
    }

    // implement onchar, onproj, onscreen
    class DrawVisitor : Ielementvisitor
    {
        IDrawManager drawmanager;

        public DrawVisitor(IDrawManager drawmanager) {
            this.drawmanager = drawmanager;
        }

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager) {
            var point = new Microsoft.Xna.Framework.Point(character.position.Item1, character.position.Item2);
            drawmanager.drawEnemy(point, 60, 60, Colour.Black);
            var point1 = new Microsoft.Xna.Framework.Point(point.X + 25, point.Y + 25);
            drawmanager.drawRectangle(point1, 10, 20, Colour.White);
        }

        public void onMainCharacter(MainCharacter Character, ScreenManager screenmanager)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawMainCharacter(point, 60, 60, Colour.White);
        }

        public void onPickUpCharacter(PickUpCharacter Character, ScreenManager screenmanager) {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.Pink);
        }

        public void onProjectile(Projectile Projectile, ScreenManager screenmanager, float dt)
        {
                var point = new Microsoft.Xna.Framework.Point(Projectile.position.Item1, Projectile.position.Item2);
                drawmanager.drawRectangle(point, 4, 4, Colour.White);
            
        }

        public void onScreenmanager(ScreenManager ScreenManager, float dt)
        {

            foreach (Ielement el in ScreenManager.elements) {
                el.Draw(this, dt);
            }
        }
    }
}
