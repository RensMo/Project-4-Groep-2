using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotParty.ScreenManager;

namespace RobotParty
{

    interface IinputManager
    {
        List<string> onInput();
    }

    public interface Ielementvisitor
    {
        void onMainCharacter(Character character);
        void onEnemyCharacter(EnemyCharacter character);
        void onProjectile(Projectile projectile);
        void onScreenmanager(ScreenManager screenmanager, float dt);
    }

    public interface elementvisitor {
        void onMainCharacter(Character character);
        void onEnemyCharacter(EnemyCharacter character);
        void onProjectile(Projectile projectile);
        void onScreenmanager(ScreenManager screenmanager);
    }

    // implement onchar, onproj
    class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;

        public UpdateVisitor(IinputManager inputmanager) {
            this.inputmanager = inputmanager;
        }

        public void onMainCharacter(Character character)
        {
            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left"); }
                if(el == "D") { character.Move("right"); }
                if(el == "W") { character.Move("up"); }
                if(el == "S") { character.Move("down"); }

            }
        }
        public void onEnemyCharacter(EnemyCharacter enemy) {
            foreach(var direction in enemy.GetDirection()) {
                Console.WriteLine(direction);
                enemy.Move(direction);
            }
        }

        public void onProjectile(Projectile projectile)
        {
            throw new NotImplementedException();
        }

        public void onScreenmanager(ScreenManager screenmanager, float dt)
        {
            foreach(Ielement el in screenmanager.elements) {
                el.Update(this);
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

        public void onMainCharacter(Character Character)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.White);
        }
        public void onEnemyCharacter(EnemyCharacter Character) {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawRectangle(point, 10, 10, Colour.Black);
        }

        public void onProjectile(Projectile Projectile)
        {
            throw new NotImplementedException();
        }

        public void onScreenmanager(ScreenManager ScreenManager, float dt)
        {

            foreach (Ielement el in ScreenManager.elements) {
                el.Draw(this);
            }
        }
    }
}
