using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotParty
{
    public interface Ielement
    {
        void Draw(IdrawVisitor drawvisitor);
        void Update(IupdateVisitor updatevisitor);
    }

    public class ScreenManager
    {
        public List<Ielement> elements = new List<Ielement>();
        //
        public void Update(IupdateVisitor visitor, float dt) { visitor.updateScreenmanager(this, dt); }

        public void Draw() { throw new NotImplementedException(); }

        public void Create() { throw new NotImplementedException(); }
    }

    public class CharacterFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    public abstract class Character : Ielement
    {
        Tuple<int, int> position;
        int health;

        public Character(Tuple<int, int> position, int health)
        {
            this.position = position;
            this.health = health;
        }

        public abstract void Move();

        public abstract void Shoot();

        public void Draw(IdrawVisitor drawvisitor)
        {
            drawvisitor.drawCharacter(this);
        }

        public abstract void Update(IupdateVisitor updatevisitor);
    }

    public class MainCharacter : Character
    {
        public MainCharacter(Tuple<int, int> position, int health) : base(position, health)
        {
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public override void Shoot()
        {
            throw new NotImplementedException();
        }

        public override void Update(IupdateVisitor updatevisitor)
        {
            throw new NotImplementedException();
        }
    }

    public class ProjectileFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    public abstract class Projectile : Ielement {
        Tuple<int, int> position;
        Tuple<int, int> direction;

        public Projectile(Tuple<int, int> position, Tuple<int, int> direction) {
            this.position = position;
            this.direction = direction;
        }

        public void Draw(IdrawVisitor drawvisitor) {
            drawvisitor.drawProjectile(this);
        }

        public void Update(IupdateVisitor updatevisitor) {
            throw new NotImplementedException();
        }
    }
}
