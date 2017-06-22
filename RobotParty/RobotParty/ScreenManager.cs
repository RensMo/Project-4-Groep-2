using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotParty
{
    public interface Ielement
    {
        void Draw(Ielementvisitor drawvisitor);
        void Update(Ielementvisitor updatevisitor);
    }

    // implement draw, create
    public class ScreenManager
    {
        public List<Ielement> elements = new List<Ielement>();
        
        public void Update(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Draw(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Create(int option)
        {
            switch (option)
            {
                case 0:
                    elements.Add(new MainCharacter(new Tuple<int, int>(50, 50), 200));
                    break;

                    // todo add more characters when finished making those
            }
        }
        public void CreateProjectile()
            {
            elements.Add(new FriendlyBullet(new Tuple<int, int>(50,50), new Tuple<int, int>(3, 0)));
            }
        
    }

    // implement create
    public class CharacterFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    // implement move, shoot, update
    public abstract class Character : Ielement
    {
        public Tuple<int, int> position;
        int health;

        public Character(Tuple<int, int> position, int health)
        {
            this.position = position;
            this.health = health;
        }

        public abstract void Shoot(string direction, Character Character);

        public void Draw(Ielementvisitor drawvisitor)
        {
            drawvisitor.onCharacter(this);
            
        }

        public void Update(Ielementvisitor updatevisitor) {
                updatevisitor.onCharacter(this);
            }

        public abstract void Move(string direction);

    }

    // implement move/shoot/update
    public class MainCharacter : Character
    {
        ProjectileFactory ProjectileFactory = new ProjectileFactory();
        public MainCharacter(Tuple<int, int> position, int health) : base(position, health)
        {

        }

        public override void Shoot(string direction, Character Character)
        {

            if (direction == "right") { ProjectileFactory.Create(0,this); }
            if (direction == "up") { ProjectileFactory.Create(1,this);  }
            if (direction == "down") { ProjectileFactory.Create(2,this); }
            if (direction == "left") { ProjectileFactory.Create(3,this); }
        }

        public override void Move(string direction) {

            var posX = position.Item1;
            var posY = position.Item2;

            if (direction == "right") { position = new Tuple<int, int>(posX + 1, posY); }
            if (direction == "up") { position = new Tuple<int, int>(posX, posY - 1); }
            if (direction == "down") { position = new Tuple<int, int>(posX, posY + 1); }
            if (direction == "left") { position = new Tuple<int, int>(posX - 1, posY); }
        }
    }

    public class ProjectileFactory
    {
        public void Create(int direction,Character Character) {
            switch (direction)
            {
                case 0:
                    new FriendlyBullet(new Tuple<int, int>(Character.position.Item1, Character.position.Item2), new Tuple<int, int>(1, 0));
                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
            }
      
        }
    }

    // implement update
    public abstract class Projectile : Ielement {
        public Tuple<int, int> position;
        public Tuple<int, int> direction;
        
        

        public Projectile(Tuple<int, int> position, Tuple<int, int> direction) {
            this.position = position;
            this.direction = direction;
        }
        
        public void Draw(Ielementvisitor drawvisitor) {
            drawvisitor.onProjectile(this);
            
        }

        public void Update(Ielementvisitor updatevisitor) {
            updatevisitor.onProjectile(this);
        }
    }

    public class FriendlyBullet : Projectile
    {
        public FriendlyBullet(Tuple<int, int> position, Tuple<int, int> direction) : base(position, direction)
        {
        }


        
    }

}
