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

    public interface IupdateVisitor
    {
        void updateCharacter();
        void updateProjectile();
        void updateScreenmanager(ScreenManager screenmanager, float dt);
    }
    class UpdateVisitor : IupdateVisitor
    {
        IinputManager inputmanager;

        public UpdateVisitor(IinputManager inputmanager) {
            this.inputmanager = inputmanager;
        }

        public void updateCharacter()
        {
            throw new NotImplementedException();
        }

        public void updateProjectile()
        {
            throw new NotImplementedException();
        }

        public void updateScreenmanager(ScreenManager screenmanager, float dt)
        {
            foreach(Ielement el in screenmanager.elements) {
                el.Update(this);
            }
        }
    }

    public interface IdrawVisitor
    {
        void drawCharacter(Character Character);
        void drawProjectile(Projectile Projectile);
        void drawScreenManager(ScreenManager ScreenManager);
    }

    class DrawVisitor : IdrawVisitor
    {
        IDrawManager drawmanager;

        public DrawVisitor(IDrawManager drawmanager) {
            this.drawmanager = drawmanager;
        }

        public void drawCharacter(Character Character)
        {
            throw new NotImplementedException();

        }

        public void drawProjectile(Projectile Projectile)
        {
            throw new NotImplementedException();
        }

        public void drawScreenManager(ScreenManager ScreenManager)
        {

            throw new NotImplementedException();
        }
    }
}
