﻿using System;
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
    public class ScreenManager {
        public List<Ielement> elements = new List<Ielement>();
        MainCharacter mainCharacter;

        public ScreenManager() {
            mainCharacter = new MainCharacter(new Tuple<int, int>(100, 100), 200, 100,  this);
        }

        public void Update(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Draw(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt); }

        public void Create(int option) {
            switch (option) {
                // Each case/level: 
                // 1. Multiple EnemyCharacters.
                // 2. At least one PickupCharacter
                // 3. One MainCharacter
                case 0:
                    elements.Add(new EnemyCharacter(new Tuple<int, int>(10, 10), 50, 30, mainCharacter, this));
                    elements.Add(new PickUpCharacter(new Tuple<int, int>(300, 300), 0, 0, this));
                    elements.Add(mainCharacter);
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
        public ScreenManager screenmanager;
        int speed;

        public Character(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager)
        {
            this.position = position;
            this.health = health;
            this.screenmanager = screenmanager;
            this.speed = speed;
        }

        public abstract void Draw(Ielementvisitor drawvisitor, float dt);
        
        public abstract void Update(Ielementvisitor updatevisitor, float dt);

        public void Move(string direction, float dt) {

            var posX = position.Item1;
            var posY = position.Item2;

            if (direction == "right") { position = new Tuple<int, int>((int)Math.Round((posX + speed * dt / 1000)), posY); }
            if (direction == "up") { position = new Tuple<int, int>(posX, (int)Math.Round(posY - speed * dt / 1000)); }
            if (direction == "down") { position = new Tuple<int, int>(posX, (int)Math.Round(posY + speed * dt / 1000)); }
            if (direction == "left") { position = new Tuple<int, int>((int)Math.Round(posX - speed * dt / 1000), posY); }
        }

    }

    // implement move/shoot/update
    public class MainCharacter : Character
    {
        public IinputManager shootmanager = new PCInputAdapter();

        public MainCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager)
        {
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onMainCharacter(this, screenmanager, dt);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onMainCharacter(this, screenmanager, dt);
        }
    }

    //Enemy character
    public class EnemyCharacter : Character {
        MainCharacter mainCharacter;
        ScreenManager screenmanager;
        public EnemyCharacter(Tuple<int, int> position, int health, int speed, MainCharacter mainCharacter, ScreenManager screenmanager) : base(position, health, speed, screenmanager) {
            this.mainCharacter = mainCharacter;
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onEnemyCharacter(this, screenmanager, dt);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onEnemyCharacter(this, screenmanager, dt);
        }

        public List<string> GetDirection() {
            var direction = new List<string>();

            var main_posX = mainCharacter.position.Item1;
            var main_posY = mainCharacter.position.Item2;

            var enemy_posX = position.Item1;
            var enemy_posY = position.Item2;

            if (main_posY - enemy_posY < 0) { direction.Add("up"); }
            else if (main_posY - enemy_posY > 0) { direction.Add("down"); }

            if (main_posX - enemy_posX < 0) { direction.Add("left"); }
            else if (main_posX - enemy_posX > 0) { direction.Add("right"); }
            return direction;
        }
    }

    //Pick-up character
    public class PickUpCharacter : Character {
        public PickUpCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager) {
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onPickUpCharacter(this, screenmanager, dt);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onPickUpCharacter(this, screenmanager, dt);
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
        }
    }

    public class FriendlyBullet : Projectile {
        public FriendlyBullet(Tuple<int, int> position, Tuple<int, int> direction) : base(position, direction) {
        }
    }
}
