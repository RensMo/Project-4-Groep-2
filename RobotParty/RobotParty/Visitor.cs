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
        void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt);
        void onProjectile(Projectile projectile, float dt);
        void onScreenmanager(ScreenManager screenmanager, float dt);
        void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt);
        void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt);
    }

    // implement onchar, onproj
    public class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;
        List<Ielement> newlist = new List<Ielement>();

        public UpdateVisitor(IinputManager inputmanager) {
            this.inputmanager = inputmanager;
        }

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt) {
            foreach (var direction in character.GetDirection()) {
                Console.WriteLine(direction);
                character.Move(direction, dt);
            }
        }

        public void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt)
        {

            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left", dt); }
                if(el == "D") { character.Move("right", dt); }
                if(el == "W") { character.Move("up", dt); }
                if(el == "S") { character.Move("down", dt); }
                     
            }

            foreach(var el in inputmanager.onInput()) { 
                if (el == "Up") {
                    var directionX = 0;
                    var directionY = -1;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Down") {
                    var directionX = 0;
                    var directionY = 1;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Right") {
                    var directionX = 1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    break;
                }
                if (el == "Left") {
                    var directionX = -1;
                    var directionY = 0;
                    newlist.Add(new FriendlyBullet(character.position, new Tuple<int, int>(directionX, directionY)));
                    break;
                }
            }
        }

        public void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt) {
            //Nothing to update. Stays at same spot.
        }

        public void onProjectile(Projectile projectile, float dt) {
            projectile.position = new Tuple<int, int>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
        }

        public void onScreenmanager(ScreenManager screenmanager, float dt)
        {
            foreach(Ielement el in screenmanager.elements) {
                el.Update(this, dt);
            }
            int i = 0;
            foreach(Ielement el in newlist) {

                screenmanager.elements.Add(el);
                Console.WriteLine(i.ToString());
                i += 1;
            }
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

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt) {
            var point = new Microsoft.Xna.Framework.Point(character.position.Item1, character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.Black);
        }

        public void onMainCharacter(MainCharacter Character, ScreenManager screenmanager, float dt)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.White);
        }

        public void onPickUpCharacter(PickUpCharacter Character, ScreenManager screenmanager, float dt) {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.Pink);
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
