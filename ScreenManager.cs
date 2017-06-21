using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Robotparty {
    public interface Ielement {
        void Draw();
        void Update();
    }

    public class ScreenManager {
        List<Ielement> elements = new List<Ielement>();
        //
        public void Update() { throw new NotImplementedException(); }

        public void Draw() { throw new NotImplementedException(); }

        public void Create() { throw new NotImplementedException(); }
    }

    public class CharacterFactory {
        public void Create() { throw new NotImplementedException(); }
    }

    public abstract class Character {
        Tuple<int, int> position;
        int health;
        
        public Character(Tuple<int, int> position, int health) {
            this.position = position;
            this.health = health;
        }

        public abstract void Move();

        public abstract void Shoot();

        public void Draw() {
            // drawvisitor.drawCharacter(this);
        }

        public abstract void Update();
    }

    public class MainCharacter : Character {
        public MainCharacter(Tuple<int, int> position, int health) : base(position, health) {
        }

        public override void Move() {
            throw new NotImplementedException();
        }

        public override void Shoot() {
            throw new NotImplementedException();
        }

        public override void Update() {
            throw new NotImplementedException();
        }
    }

    public class ProjectileFactory {
        public void Create() { throw new NotImplementedException(); }
    }
}
