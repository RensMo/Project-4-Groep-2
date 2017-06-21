using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RobotParty
{

    interface IinputManager
    {
        void OnInput();
    }

    interface IupdateVisitor
    {
        void updateCharacter();
        void updateProjectile();
        void updateScreenmanager();
    }
    class UpdateVisitor : IupdateVisitor
    {
        int Inputmanager;

        public void updateCharacter()
        {
            throw new NotImplementedException();
        }

        public void updateProjectile()
        {
            throw new NotImplementedException();
        }

        public void updateScreenmanager()
        {
            throw new NotImplementedException();
        }
    }

    interface Idrawmanager
    {
        void drawRectangle();
        void drawText();
        void drawImage();
    }

    interface IdrawVisitor
    {
        void drawCharacter(Character Character);
        void drawProjectile(Projectile Projectile);
        void drawScreenManager(ScreenManager ScreenManager);
    }

    class drawvisitor : IdrawVisitor
    {
        int Drawmanager;

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
