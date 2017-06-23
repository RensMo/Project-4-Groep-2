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

            if(el1X >= el2X && el1Y >= el2Y && el1X <= el2X + 10 && el1Y <= el2Y + 10) {
                collision = true;
            }
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
        void onProjectile(Projectile projectile, float dt);
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

            foreach(var el in screenmanager.elements) {
                if(collisioncalculator.Collision(character, el)) {
                    
                    if(el is FriendlyBullet) {
                        screenmanager.score += 5;
                        removelist.Add(character);
                        
                        Console.WriteLine("removeEnemy;");
                    }
                }
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
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Down") {
                    var directionX = 0;
                    var directionY = 1;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Right") {
                    var directionX = 1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Left") {
                    var directionX = -1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>(directionX, directionY)));
                    break;
                }
            }
        }

        public void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager) {
            //Nothing to update. Stays at same spot.
        }

        public void onProjectile(Projectile projectile, float dt) {
            projectile.position = new Tuple<int, int>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
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
                Console.WriteLine(i.ToString());
                i += 1;
            }

            int x = 0;
            while (x < removelist.Count()) {
                screenmanager.elements.Remove(removelist[x]);
                x += 1;
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
        }

        public void onMainCharacter(MainCharacter Character, ScreenManager screenmanager)
        {
            var ScorePoint = new Microsoft.Xna.Framework.Point(375, 0);
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawMainCharacter(point, 60, 60, Colour.White);
            drawmanager.drawText("Score:" + screenmanager.score.ToString(), ScorePoint, 5, Colour.Black);
        }

        public void onPickUpCharacter(PickUpCharacter Character, ScreenManager screenmanager) {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.Pink);
        }

        public void onProjectile(Projectile Projectile, float dt)
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
