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

            if(element1 is EnemyCharacter && element2 is FriendlyBullet) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
                    }
                }
            }
            else if (element1 is VillainCharacter && element2 is FriendlyBullet) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
                    }
                }
            }

            else if(element1 is MainCharacter && element2 is EnemyBullet) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
                    }
                }
            }

            else if(element1 is MainCharacter && element2 is PickUpCharacter) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
                    }
                }
            }
            else if (element1 is MainCharacter && element2 is EnemyCharacter) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
                    }
                }
            }

            else if (element1 is MainCharacter && element2 is VillainCharacter) {
                int VillainX = element1.getPos().Item1;
                int VillainY = element1.getPos().Item2;
                int BulletX =  element2.getPos().Item1;
                int BulletY =  element2.getPos().Item2;
                if (BulletX >= VillainX + 50 && BulletX <= (VillainX + 100))
                {
                    if (BulletY >= VillainY && BulletY <=(VillainY + 50)){
                        collision = true;
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
        void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt);
        void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt);
        void onScreenmanager(ScreenManager screenmanager, float dt);
        void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt);
        void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt);
        void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index);
    }

    // implement onchar, onproj
    public class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;
        IonCollision collisioncalculator;
        List<Ielement> newlist = new List<Ielement>();
        List<Ielement> removelist = new List<Ielement>();
        float EnemyTimeCounter = 0.0f;
        float FriendlyTimeCounter = 1000.0f;
        float lastEnemyBullet;
        float lastFriendlyBullet;

        public UpdateVisitor(IinputManager inputmanager, IonCollision collisioncalculator) {
            this.inputmanager = inputmanager;
            this.collisioncalculator = collisioncalculator;
        }

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt) {

            Tuple<int, int> characterpos = new Tuple<int, int>(character.getPos().Item1 + 25, character.getPos().Item2 + 25);

            foreach (var direction in character.GetDirection()) {
                character.Move(direction, dt);
            }


            
            EnemyTimeCounter += dt;
            if (EnemyTimeCounter > 1000.0f) {
                EnemyTimeCounter = 0.0f;
                //Console.WriteLine(dt.ToString());
                switch (character.RandomShot()) {

                    case 0:
                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(1, 0), screenmanager));
                        break;
                    case 1:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(1, 1), screenmanager));
                        break;
                    case 2:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(0, 1), screenmanager));
                        break;
                    case 3:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(-1, 0), screenmanager));
                        break;
                    case 4:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(-1, 1), screenmanager));
                        break;
                    case 5:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(0, -1), screenmanager));
                        break;
                    case 6:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(-1, -1), screenmanager));
                        break;
                    case 7:

                        newlist.Add(new EnemyBullet(characterpos, new Tuple<int, int>(1, -1), screenmanager));
                        break;
                }
            }
            
        }

        public void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt)
        {
            
            if(character.health < 0) {
                Console.WriteLine("you lose");
            }

            foreach(var el in screenmanager.elements) {
                if(collisioncalculator.Collision(character, el)) {
                    // check if it's an enemy character
                    if(el is EnemyCharacter) {
                        removelist.Add(character);
                    }

                    if(el is VillainCharacter) {
                        removelist.Add(character);
                    }
                    if(el is PickUpCharacter) {
                        character.health = 500;
                        removelist.Add(el);
                        screenmanager.score += 100;
                    }
                    // check if it's an enemy bullet
                    if (el is EnemyBullet)
                    {
                        character.health -= 50;
                        removelist.Add(el);
                    }
                }

            }

            foreach(var el in inputmanager.onInput()) {
                if(el == "A") { character.Move("left", dt); }
                if(el == "D") { character.Move("right", dt); }
                if(el == "W") { character.Move("up", dt); }
                if(el == "S") { character.Move("down", dt); }
                     
            }

            FriendlyTimeCounter += dt;
            if(FriendlyTimeCounter > 500.0f) {
                FriendlyTimeCounter = 0.0f;
                foreach (var el in inputmanager.onInput()) {

                    if (el == "UpRight") {

                        var directionX = 1;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "UpLeft") {
                        var directionX = -1;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "DownLeft") {
                        var directionX = -1;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "DownRight") {
                        var directionX = 1;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Up") {
                        var directionX = 0;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Down") {
                        var directionX = 0;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Right") {
                        var directionX = 1;
                        var directionY = 0;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Left") {
                        var directionX = -1;
                        var directionY = 0;
                        newlist.Add(new FriendlyBullet(new Tuple<int, int>(character.position.Item1 + 28, character.position.Item2 + 28), new Tuple<int, int>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                        break;
                    }
                }
            }
        }

        public void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt) {
            //Nothing to update. Stays at same spot.

        }

        public void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index)
        {
            //Nothing to update. Stays at same spot.
            // Random rnd = new Random();
            // int index = rnd.Next(1, 4);

            
           if (index >= 1 && index <= 100) {
                character.position = new Tuple<int, int>(character.position.Item1 + 1, character.position.Item2);
                
                character.IndexPlus(1);
               // if (indextwo == 10) {  }
                
            }
           else if (index >= 101 && index <= 200)
            {
                character.position = new Tuple<int, int>(character.position.Item1, character.position.Item2 - 1);
                
                character.IndexPlus(1);
               // if (indextwo == 20) {  }
            }
           else if (index >= 201 && index <= 300)
            {
                character.position = new Tuple<int, int>(character.position.Item1 - 1, character.position.Item2);
                
                character.IndexPlus(1);
               // if (indextwo == 30) {  }
            }
           else if (index >= 301 && index <= 400)
            {
                character.position = new Tuple<int, int>(character.position.Item1, character.position.Item2 + 1);
                

                character.IndexPlus(1);
            }

           else if (index>= 401 && index <= 500)
            {
                character.IndexSet(1);
            }
            
        }

        public void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt) {
            projectile.position = new Tuple<int, int>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
            foreach (var el in screenmanager.elements) {
                //if (el is EnemyCharacter) {
                //    if (collisioncalculator.Collision(el, projectile)) {
                //        removelist.Add(el);
                //    }
                //}
                if(el is VillainCharacter) {
                    if(collisioncalculator.Collision(el, projectile)) {
                        removelist.Add(el);
                        break;
                    }
                }
                if(el is EnemyCharacter) {
                    if(collisioncalculator.Collision(el, projectile)) {
                        removelist.Add(el);
                        break;
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

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt) {
            var point = new Microsoft.Xna.Framework.Point(character.position.Item1, character.position.Item2);
            drawmanager.drawEnemy(point, 60, 60, Colour.White);
        }

        public void onMainCharacter(MainCharacter Character, ScreenManager screenmanager, float dt)
        {
            var ScorePoint = new Microsoft.Xna.Framework.Point(375, 0);
            var point = new Microsoft.Xna.Framework.Point(Character.position.Item1, Character.position.Item2);
            drawmanager.drawMainCharacter(point, 60, 60, Colour.White);
            drawmanager.drawText("Score:" + screenmanager.score.ToString(), ScorePoint, 5, Colour.Black);

        }

        public void onPickUpCharacter(PickUpCharacter Character, ScreenManager screenmanager, float dt) {
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

        public void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index)
        {
            var point = new Microsoft.Xna.Framework.Point(character.position.Item1, character.position.Item2);
            drawmanager.drawEnemy(point, 60, 60, Colour.White);
        }
    }
}
