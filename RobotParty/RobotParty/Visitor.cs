using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RobotParty
{

    interface IinputManager
    {
        List<string> onInput();
    }

    public interface Ielementvisitor
    {
        void onCharacter(Character character);
        void onProjectile(Projectile projectile);
        void onScreenmanager(ScreenManager screenmanager, float dt);
    }

    // implement onchar, onproj
    class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;

        public UpdateVisitor(IinputManager inputmanager) {
            this.inputmanager = inputmanager;
        }

        public void onCharacter(Character character)
        {
            throw new NotImplementedException();
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

        public void onCharacter(Character Character)
        {
            

        }

        public void onProjectile(Projectile Projectile)
        {
            throw new NotImplementedException();
        }

        public void onScreenmanager(ScreenManager ScreenManager, float dt)
        {

            throw new NotImplementedException();
        }
    }
}
