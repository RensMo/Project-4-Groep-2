using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Class1
{
	public interface Element
	{
        void Draw();
        void Update();
	}

    public class ScreenManager
    {
        List<Element> elements = new List<Element>();
        //
        public void Update() { throw new NotImplementedException(); } 

        public void Draw() { throw new NotImplementedException(); }

        public void Create() { throw new NotImplementedException(); }
    }

    public class CharacterFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }

    public abstract class Character
    {
        

        public void Move() { throw new NotImplementedException(); }

        public void Shoot() { throw new NotImplementedException(); }

        public void Draw() { throw new NotImplementedException(); }

        public void Update() { throw new NotImplementedException(); }

    }

    public class ProjectileFactory
    {
        public void Create() { throw new NotImplementedException(); }
    }
}
