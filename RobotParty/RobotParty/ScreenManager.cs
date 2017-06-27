using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotParty
{

    // interface for all elements in the game. at the very least an element is updatable, drawable and you can get/set a position
    public interface Ielement
    {
        void Draw(Ielementvisitor drawvisitor, float dt);
        void Update(Ielementvisitor updatevisitor, float dt);
        Tuple<int, int> getPos();
        void setPos(Tuple<int, int> newpos);
    }

    // screenmanager is made to have a list with all the elements for the current state of the game.
    // the screenmanager keeps track of the score and which level should be displayed
    public class ScreenManager {
        public List<Ielement> elements = new List<Ielement>();
        MainCharacter mainCharacter;
        public int score = 0;
        public int lives = 3;
        public List<text> Top5Score = new List<text>();

        public ScreenManager() {
            mainCharacter = new MainCharacter(new Tuple<int, int>(100, 100), 200, 100,  this);
        }

        public void Update(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt,mainCharacter); }

        public void Draw(Ielementvisitor visitor, float dt) { visitor.onScreenmanager(this, dt,mainCharacter); }

        public void Create(int option) {
            switch (option) {
                // Each case/level: 
                // 1. Multiple EnemyCharacters.
                // 2. At least one PickupCharacter
                // 3. One MainCharacter
                case 0:
                    //elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(10, 10), 50, 30, this, mainCharacter));
                    elements.Add(new CircleEnemyCharacter(new Tuple<int, int>(300, 300), 50, 30, this, 100));
                    //elements.Add(new VillainCharacter(new Tuple<int, int>(400, 300), 50, 1, this));

                    elements = new List<Ielement>();
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(100, 10), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(10, 100), 50, 30,this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(50, 10), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(10, 50), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(100, 300), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(150, 10), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(10, 102), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(300, 300), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(10, 250), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(50, 150), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(0, 50), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(500, 300), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(150, 400), 50, 30, this, mainCharacter));
                    elements.Add(new FollowEnemyCharacter(new Tuple<int, int>(250, 102), 50, 30, this, mainCharacter));

                    elements.Add(new CircleEnemyCharacter(new Tuple<int, int>(300, 300), 50, 30, this, 100));

                    elements.Add(new VillainCharacter(new Tuple<int, int>(300, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(200, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(100, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(500, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(400, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(400, 300), 50, 1, this));

                    elements.Add(new PickUpCharacter(new Tuple<int, int>(300, 300), 50, 1, this));
                    elements.Add(new PickUpCharacter(new Tuple<int, int>(400, 300), 50, 1, this));
                    elements.Add(new PickUpCharacter(new Tuple<int, int>(500, 300), 50, 1, this));
                    elements.Add(mainCharacter);
                    break;
                case 1:
                    elements = new List<Ielement>();
                    elements.Add(new PickUpCharacter(new Tuple<int, int>(100, 300), 50, 1, this));
                    elements.Add(mainCharacter);
                    break;
                case 10:
                    elements = new List<Ielement>();
                    elements.Add(new text(new Tuple<int, int>(350, 200), this, "press ESC to quit."));
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

    // this is a simple implementation of Ielement to show text on screen.
    public class text : Ielement {
        Tuple<int, int> position;
        ScreenManager screenmanager;
        public string Text;

        public text(Tuple<int,int> position, ScreenManager screenmanager, string text) {
            this.position = position;
            this.screenmanager = screenmanager;
            this.Text = text;
        }
        public void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onText(this, screenmanager);
        }

        public Tuple<int, int> getPos() {
            return position;
        }

        public void setPos(Tuple<int, int> newpos) {
            position = newpos;
        }

        public void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onText(this, screenmanager);
        }
    }

    // abstract class character, here we define everything all characters have in common, and below we add certain character-type specific behaviour
    public abstract class Character : Ielement
    {
        public Tuple<int, int> position;
        public int health;
        public ScreenManager screenmanager;
        int speed;

        public Character(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager)
        {
            this.position = position;
            this.health = health;
            this.screenmanager = screenmanager;
            this.speed = speed;
        }

        public Tuple<int,int> getPos() { return position; }
        public void setPos(Tuple<int, int> newpos) { position = newpos; }

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

    // this is where we create a maincharacter class. it contains all normal character logic and calls proper visitors
    public class MainCharacter : Character
    {
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
    public abstract class EnemyCharacter : Character
    {
        public EnemyCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager)
        {
        }

        public int RandomShot()
        {
            Random rnd = new Random();
            int RandomDirection = rnd.Next(0, 8);

            return RandomDirection;
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt)
        {
            drawvisitor.onEnemyCharacter(this, screenmanager, dt);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt)
        {
            updatevisitor.onEnemyCharacter(this, screenmanager, dt);
        }

        public abstract List<string> GetDirection();
    }

    public class FollowEnemyCharacter : EnemyCharacter
    {
        MainCharacter mainCharacter;

        public FollowEnemyCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager, MainCharacter mainCharacter) : base(position, health, speed, screenmanager)
        {
            this.mainCharacter = mainCharacter;
        }

        public override List<string> GetDirection()
        {
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

    public class CircleEnemyCharacter : EnemyCharacter
    {
        int speed;
        int radius;
        Tuple<int, int> position_0; //initial position
        Tuple<int, int> circleStep;
        int step;

        public CircleEnemyCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager, int radius) : base(position, health, speed, screenmanager)
        {
            this.radius = radius;
            this.speed = speed;

            position_0 = new Tuple<int,int>(position.Item1 - radius, position.Item2);
            step = 1;
            circleStep = GetTarget();

        }

        private Tuple<int, int> GetTarget() {
            Tuple<int, int> target;
            
            var x_0 = position_0.Item1;
            var y_0 = position_0.Item2;
            
            var x_1 = radius * Math.Cos(2* Math.PI / 360 * step) + x_0;
            var y_1 = radius * Math.Sin(2 * Math.PI / 360 * step) + y_0;

            target = new Tuple<int, int>((int)Math.Round(x_1), (int)Math.Round(y_1));
            if (step == 360) step = 0;
            step++;
            return target;
        }

        public override List<string> GetDirection()
        {
            var direction = new List<string>();
            //Console.WriteLine("---");
            //Console.WriteLine("Position: " + position);
            if (circleStep.Item1 == position.Item1 && circleStep.Item2 == position.Item2) {
                circleStep = GetTarget();
                Console.WriteLine("Target: " + circleStep);
            }
            

            var positionX = position.Item1;
            var positionY = position.Item2;

            var targetX = circleStep.Item1;
            var targetY = circleStep.Item2;

            if (positionX > targetX) { direction.Add("left"); }
            else if (positionX < targetX) { direction.Add("right"); }

            if (positionY > targetY) { direction.Add("up"); }
            else if (positionY < targetY) { direction.Add("down"); }

            Console.WriteLine("Direction: ");
            foreach (var shit in direction) { Console.WriteLine(shit); };          

            return direction;
        }
    }

    //Pick-up character is the character that needs to be picked up by the main character. it is only drawn and gets removed from the list once the maincharacter hovers over it.
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

    // villaincharacter is a different enemy ccharacter, this one walks in squares and kill the maincharacter on collisiion
    public class VillainCharacter : Character
    {

        int index = 1;

        public VillainCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager)
        {
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt)
        {
            drawvisitor.onVillainCharacter(this, screenmanager, dt, index);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt)
        {

            updatevisitor.onVillainCharacter(this, screenmanager, dt, index);
        }
        public void IndexSet(int i)
        {
            this.index = i;
        }

        public void IndexPlus(int i)
        {
            this.index += i;
        }

        

    }

    public class ProjectileFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    // projectile is an abstract class that describes behaviour of all projectiles
    public abstract class Projectile : Ielement {
        public Tuple<int, int> position;
        public Tuple<int, int> direction;
        ScreenManager screenmanager;

        public Projectile(Tuple<int, int> position, Tuple<int, int> direction, ScreenManager screenmanager) {
            this.position = position;
            this.direction = direction;
            this.screenmanager = screenmanager;
        }

        public Tuple<int, int> getPos() { return position; }
        public void setPos(Tuple<int, int> newpos) { position = newpos; }

        public void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onProjectile(this, screenmanager, dt);
        }

        public void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onProjectile(this, screenmanager, dt);
        }
    }

    // these are extensions of projectiles so that we can implement differen collision logic. 
    public class FriendlyBullet : Projectile {
        public FriendlyBullet(Tuple<int, int> position, Tuple<int, int> direction, ScreenManager screenmanager) : base(position, direction, screenmanager) {
        }
    }
    public class EnemyBullet : Projectile
    {
        public EnemyBullet(Tuple<int, int> position, Tuple<int, int> direction,ScreenManager screenmanager) : base(position, direction,screenmanager)
        {
        }
    }
}
