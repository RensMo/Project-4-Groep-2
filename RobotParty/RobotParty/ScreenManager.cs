using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotParty
{
    public interface Ielement
    {
        void Draw(Ielementvisitor drawvisitor, float dt);
        void Update(Ielementvisitor updatevisitor, float dt);
    }

    // implement draw, create
    public class ScreenManager
    {
        public List<Ielement> elements = new List<Ielement>();
        
        public void Update(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Draw(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Create(int option) {
            switch (option) {
                case 0:
                    elements.Add(new MainCharacter(new Tuple<int, int>(50, 50), 200, this));
                    break;
                    // todo add more characters when finished making those
            }
        }

        public void addElement(Ielement element) {
            elements.Add(element);
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
        public ScreenManager screenmanager;

        public Character(Tuple<int, int> position, int health, ScreenManager screenmanager)
        {
            this.position = position;
            this.health = health;
            this.screenmanager = screenmanager;
        }

        public void Draw(Ielementvisitor drawvisitor, float dt)
        {
            drawvisitor.onCharacter(this, screenmanager);
        }

        public void Update(Ielementvisitor updatevisitor, float dt) {
                updatevisitor.onCharacter(this, screenmanager);
            }

        public abstract void Move(string direction);

    }

    // implement move/shoot/update
    public class MainCharacter : Character
    {
        public IinputManager shootmanager = new PCInputAdapter();

        public MainCharacter(Tuple<int, int> position, int health, ScreenManager screenmanager) : base(position, health, screenmanager)
        {
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
        public void Create() { throw new NotImplementedException(); }
    }

    // implement update
    public abstract class Projectile : Ielement {
        public Tuple<int, int> position;
        public Tuple<int, int> direction;

        public Projectile(Tuple<int, int> position, Tuple<int, int> direction) {
            this.position = position;
            this.direction = direction;
        }

        public void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onProjectile(this, dt);
        }

        public void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onProjectile(this, dt);
            Console.WriteLine("hi");
        }
    }

    public class FriendlyBullet : Projectile {
        public FriendlyBullet(Tuple<int, int> position, Tuple<int, int> direction) : base(position, direction) {
        }
    }
}
