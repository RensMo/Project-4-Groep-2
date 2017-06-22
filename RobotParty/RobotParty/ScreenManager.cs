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
        MainCharacter mainCharacter = new MainCharacter(new Tuple<int, int>(50, 50), 200, 1);

        public void Update(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Draw(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Create(int option) {
            switch (option) {
                case 0:
                    elements.Add(mainCharacter);
                    elements.Add(new EnemyCharacter(new Tuple<int, int>(10, 10), 50, 1, mainCharacter));
                    break;
                    // todo add more characters when finished making those
            }
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
        int speed;

        public Character(Tuple<int, int> position, int health, int speed)
        {
            this.position = position;
            this.health = health;
            this.speed = speed;
        }

        public abstract void Shoot();

        public abstract void Draw(Ielementvisitor drawvisitor);

        public abstract void Update(Ielementvisitor updatevisitor);

        public void Move(string direction) {
            var posX = position.Item1;
            var posY = position.Item2;

            if (direction == "right") { position = new Tuple<int, int>(posX + 1, posY); }
            if (direction == "up") { position = new Tuple<int, int>(posX, posY - 1); }
            if (direction == "down") { position = new Tuple<int, int>(posX, posY + 1); }
            if (direction == "left") { position = new Tuple<int, int>(posX - 1, posY); }

            /*
            if (direction == "upright") { position = new Tuple<int, int>(posX + 1, posY - 1); }
            if (direction == "downright") { position = new Tuple<int, int>(posX + 1, posY + 1); }
            if (direction == "upleft") { position = new Tuple<int, int>(posX - 1, posY - 1); }
            if (direction == "downleft") { position = new Tuple<int, int>(posX - 1, posY + 1); }
            */
        }

    }

    // implement move/shoot/update
    public class MainCharacter : Character
    {
        public MainCharacter(Tuple<int, int> position, int health, int speed) : base(position, health, speed)
        {
        }

        public override void Draw(Ielementvisitor drawvisitor) {
            drawvisitor.onMainCharacter(this);
        }

        public override void Update(Ielementvisitor updatevisitor) {
            updatevisitor.onMainCharacter(this);
        }

        public override void Shoot()
        {
            
        }
    }

    //Enemy character
    public class EnemyCharacter : Character {
        MainCharacter mainCharacter;
        public EnemyCharacter(Tuple<int, int> position, int health, int speed, MainCharacter mainCharacter) : base(position, health, speed) {
            this.mainCharacter = mainCharacter;
        }

        public override void Draw(Ielementvisitor drawvisitor) {
            drawvisitor.onEnemyCharacter(this);
        }

        public override void Update(Ielementvisitor updatevisitor) {
            updatevisitor.onEnemyCharacter(this);
        }

        public override void Shoot() {

        }

        public List<string> GetDirection() {
            var direction = new List<string>();

            var main_posX = mainCharacter.position.Item1;
            var main_posY = mainCharacter.position.Item2;

            var enemy_posX = position.Item1;
            var enemy_posY = position.Item2;

            if (main_posY - enemy_posY < 0) {  direction.Add("up"); }
            else if(main_posY - enemy_posY > 0){ direction.Add("down"); } 

            if (main_posX - enemy_posX < 0) { direction.Add("left"); }
            else if (main_posX - enemy_posX > 0) { direction.Add("right"); } 
            return direction;
        }
    }

    public class ProjectileFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    // implement update
    public abstract class Projectile : Ielement {
        Tuple<int, int> position;
        Tuple<int, int> direction;

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
}
