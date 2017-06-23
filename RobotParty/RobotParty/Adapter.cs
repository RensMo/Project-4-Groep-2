﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RobotParty
{

    class PCInputAdapter : IinputManager {
        List<string> keyslist;

        public List<string> onInput()
        {
            keyslist = new List<string>();
            var key = Keyboard.GetState();
            key.GetPressedKeys();
            if (key.IsKeyDown(Keys.A))
            {
                keyslist.Add("A");
            }
            if (key.IsKeyDown(Keys.W))
            {
                keyslist.Add("W");
            }
            if (key.IsKeyDown(Keys.S))
            {
                keyslist.Add("S");
            }
            if (key.IsKeyDown(Keys.D))
            {
                keyslist.Add("D");
            }
            if (key.IsKeyDown(Keys.Right))
            {
                keyslist.Add("Right");
            }
            if (key.IsKeyDown(Keys.Left))
            {
                keyslist.Add("Left");
            }
            if (key.IsKeyDown(Keys.Up))
            {
                keyslist.Add("Up");
            }
            if (key.IsKeyDown(Keys.Down))
            {
                keyslist.Add("Down");
            }

            return keyslist;

        }

    }

    public interface IDrawManager {
        void drawRectangle(Point top_left_coordinate, float width, float height, Colour color);
        void drawText(string text, Point top_left_coordinate, int size, Colour color);
        void drawImage(Point top_left_coordinate, float width, float height, Colour color);
        void drawCharacter(Point top_left_coordinate, float width, float height, Colour color);
        void drawEnemy(Point top_left_coordinate, float width, float height, Colour color);
    }

    public enum Colour { White, Black, Blue };

    public class MonoGameAdapter : IDrawManager {
        SpriteBatch sprite_batch;
        ContentManager content_manager;

        Texture2D white_pixel;

        Texture2D spriteMC;
        Texture2D spriteEC;
        
        
        SpriteFont default_font;
        Game game;
        public MonoGameAdapter(SpriteBatch sprite_batch, ContentManager content_manager) {
            this.sprite_batch = sprite_batch;
            this.content_manager = content_manager;
            white_pixel = content_manager.Load<Texture2D>("white_pixel");
            spriteMC = content_manager.Load<Texture2D>("MainCharacter");
            spriteEC = content_manager.Load<Texture2D>("EnemyCharacter");
            default_font = content_manager.Load<SpriteFont>("arial");
        }

        private Microsoft.Xna.Framework.Color convert_color(Colour color) {
            switch (color) {
                case Colour.Black:
                    return Microsoft.Xna.Framework.Color.Black;
                case Colour.White:
                    return Microsoft.Xna.Framework.Color.White;
                case Colour.Blue:
                    return Microsoft.Xna.Framework.Color.Blue;
                default:
                    return Microsoft.Xna.Framework.Color.Black;
            }
        }
        public void drawRectangle(Point top_left_coordinate, float width, float height, Colour color) {
            sprite_batch.Draw(white_pixel, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }

        public void drawText(string text, Point top_left_coordinate, int size, Colour color) {
            sprite_batch.DrawString(default_font, text, new Vector2(top_left_coordinate.X, top_left_coordinate.Y), convert_color(color));
        }

        // implement drawImage
        public void drawImage(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(white_pixel, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }

        public void drawCharacter(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(spriteMC, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }

        public void drawEnemy(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(spriteEC, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }
    }
}