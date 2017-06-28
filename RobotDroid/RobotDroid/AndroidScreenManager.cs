using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;

namespace RobotDroid {
    public interface Ielement {
        void Draw(Ielementvisitor drawvisitor, float dt);
        void Update(Ielementvisitor updatevisitor, float dt);
        Tuple<int, int> getPos();
    }

    // implement draw, create
    public class ScreenManager {
        public List<Ielement> elements = new List<Ielement>();
        MainCharacter mainCharacter;
        public int score = 0;

        public ScreenManager() {
            mainCharacter = new MainCharacter(new Tuple<int, int>(100, 600), 200, 200, this);
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
                    
                    elements.Add(mainCharacter);
                    elements.Add(new EnemyCharacter(new Tuple<int, int>(10, 10), 50, 50, mainCharacter, this));
                    elements.Add(new PickUpCharacter(new Tuple<int, int>(300, 300), 50, 1, this));
                    elements.Add(new VillainCharacter(new Tuple<int, int>(400, 300), 50, 1, this));
                    elements.Add(new Button("left", new Tuple<int, int>(100, 1800), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("right", new Tuple<int, int>(500,1800), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("up", new Tuple<int, int>(300, 1600), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("down", new Tuple<int, int>(300, 2000), 200, 200, "links", this, mainCharacter));

                    elements.Add(new Button("shootleft", new Tuple<int, int>(800, 1800), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("shootright", new Tuple<int, int>(1200, 1800), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("shootup", new Tuple<int, int>(1000, 1600), 200, 200, "links", this, mainCharacter));
                    elements.Add(new Button("shootdown", new Tuple<int, int>(1000, 2000), 200, 200, "links", this, mainCharacter));
                    break;
                    // todo add more characters when finished making those
            }
        }
    }

    // implement create
    public class CharacterFactory {
        public void Create() { throw new NotImplementedException(); }
    }

    // implement move, shoot, update
    public abstract class Character : Ielement {
        public Tuple<int, int> position;
        public int health;
        public ScreenManager screenmanager;
        int speed;

        public Character(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) {
            this.position = position;
            this.health = health;
            this.screenmanager = screenmanager;
            this.speed = speed;
        }

        public Tuple<int, int> getPos() { return position; }

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
    public class MainCharacter : Character {
        public IinputManager shootmanager = new PCInputAdapter();

        public MainCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager) {
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
        // ScreenManager screenmanager;
        public EnemyCharacter(Tuple<int, int> position, int health, int speed, MainCharacter mainCharacter, ScreenManager screenmanager) : base(position, health, speed, screenmanager) {
            this.mainCharacter = mainCharacter;
            // this.screenmanager = screenmanager;
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onEnemyCharacter(this, screenmanager, dt);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onEnemyCharacter(this, screenmanager, dt);
        }
        public int RandomShot() {

            Random rnd = new Random();
            int RandomDirection = rnd.Next(0, 8);

            return RandomDirection;
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

    public class VillainCharacter : Character {

        int index = 1;

        public VillainCharacter(Tuple<int, int> position, int health, int speed, ScreenManager screenmanager) : base(position, health, speed, screenmanager) {
        }

        public override void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onVillainCharacter(this, screenmanager, dt, index);
        }

        public override void Update(Ielementvisitor updatevisitor, float dt) {

            updatevisitor.onVillainCharacter(this, screenmanager, dt, index);
        }
        public void IndexSet(int i) {
            this.index = i;
        }

        public void IndexPlus(int i) {
            this.index += i;
        }



    }

    public class ProjectileFactory {
        public void Create() { throw new NotImplementedException(); }
    }

    // implement update
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

        public void Draw(Ielementvisitor drawvisitor, float dt) {
            drawvisitor.onProjectile(this, screenmanager, dt);
        }

        public void Update(Ielementvisitor updatevisitor, float dt) {
            updatevisitor.onProjectile(this, screenmanager, dt);
        }
    }

    public class FriendlyBullet : Projectile {
        public FriendlyBullet(Tuple<int, int> position, Tuple<int, int> direction, ScreenManager screenmanager) : base(position, direction, screenmanager) {
        }
    }
    public class EnemyBullet : Projectile {
        public EnemyBullet(Tuple<int, int> position, Tuple<int, int> direction, ScreenManager screenmanager) : base(position, direction, screenmanager) {
        }
    }

    // Android input

    public class Button : Ielement
    {
        public float width, height;
        //public string action;
        
        public string text; //new
        public MainCharacter character;
        public Tuple<int,int> top_left_corner;
        ScreenManager screenmanager;

        public Button(string text, Tuple<int, int> top_left_corner, float width, float height, string action, ScreenManager screenmanager, MainCharacter character)
        //MISSING CODE
        {
          //  this.action = action;
            this.width = width;
            this.height = height;
            
            this.text = text;
            this.character = character;
            this.top_left_corner = top_left_corner;
            this.screenmanager = screenmanager;
        }


        public void Draw(Ielementvisitor drawvisitor, float dt)
        {
            drawvisitor.onButton(this, screenmanager, dt, character);
        }

        public Tuple<int, int> getPos()
        {
            return top_left_corner;
        }

        public void setPos(Tuple<int, int>position)
        {
            top_left_corner = position;
        }
        public bool is_intersecting(Vector2 point)
        {
            return point.X > top_left_corner.Item1 && point.Y > top_left_corner.Item2 &&
             point.X < top_left_corner.Item1 + width && point.Y < top_left_corner.Item2 + height;
        }
        

        public void Update(Ielementvisitor updatevisitor, float dt)
        {
            updatevisitor.onButton(this, screenmanager, dt, character);
        }
    }


}