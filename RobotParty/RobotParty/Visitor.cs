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
        void oncharacter(Character character);
        void onEnemy(Character character);
        void onProjectile(Projectile projectile);
        void onScreenmanager(ScreenManager screenmanager, float dt);
    }

    public interface elementvisitor {
        void onCharacter(Character character);
        void onEnemy(Character character);
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

        public void oncharacter(Character character)
        {
            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left"); }
                if(el == "D") { character.Move("right"); }
                if(el == "W") { character.Move("up"); }
                if(el == "S") { character.Move("down"); }

            }
        }

        public void onEnemy(Character character)
        {
            foreach (var el in inputmanager.onInput())
            {
                if (el == "A") { character.Move("left"); }
                if (el == "D") { character.Move("right"); }
                if (el == "W") { character.Move("up"); }
                if (el == "S") { character.Move("down"); }

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

        public void oncharacter(Character Character)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawCharacter(point, 60, 60, Colour.White);
        }

        public void onEnemy(Character Character)
        {
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawEnemy(point, 60, 60, Colour.White);
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
