using System;
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
                if (key.IsKeyDown(Keys.Up))
                {
                    keyslist.Add("UpRight");
                }
                else if (key.IsKeyDown(Keys.Down))
                {
                    keyslist.Add("DownRight");
                }
                else
                {
                    keyslist.Add("Right");
                }
            }

            if (key.IsKeyDown(Keys.Left))
            {
                if (key.IsKeyDown(Keys.Up))
                {
                    keyslist.Add("UpLeft");
                }
                else if (key.IsKeyDown(Keys.Down))
                {
                    keyslist.Add("DownLeft");
                }
                else
                {
                    keyslist.Add("Left");
                }
            }

            if (key.IsKeyDown(Keys.Up))
            {
                if (key.IsKeyDown(Keys.Right))
                {
                    keyslist.Add("UpRight");
                }
                else if (key.IsKeyDown(Keys.Left))
                {
                    keyslist.Add("UpLeft");
                }
                else { keyslist.Add("Up"); }
            }

            if (key.IsKeyDown(Keys.Down))
            {
                if (key.IsKeyDown(Keys.Right))
                {
                    keyslist.Add("DownRight");
                }
                else if (key.IsKeyDown(Keys.Left))
                {
                    keyslist.Add("DownLeft");
                }
                else
                {
                    keyslist.Add("Down");
                }
            }

            return keyslist;

        }

    }

    public interface IDrawManager {
        void drawRectangle(Point top_left_coordinate, float width, float height, Colour color);
        void drawText(string text, Point top_left_coordinate, int size, Colour color);
        void drawImage();
        void drawMainCharacter(Point top_left_coordinate, float width, float height, Colour color);
        void drawEnemy(Point top_left_coordinate, float width, float height, Colour color, int index);
    }

    public enum Colour { White, Black, Blue, Pink };

    public class MonoGameAdapter : IDrawManager {
        SpriteBatch sprite_batch;
        ContentManager content_manager;
        Texture2D spriteMC, spriteEC,
            spriteEC2, spriteEC3, spriteVC,
            spriteVC2, spriteVC3,
            spriteMC2, spriteMC3, SSJ;

        Texture2D[] spritesenemy;
        Texture2D[] spritesmain;
        Texture2D[] spritesvillain;
        Texture2D white_pixel;
        SpriteFont default_font;
        Random rnd = new Random();
        int b;

        Game game;
        public MonoGameAdapter(SpriteBatch sprite_batch, ContentManager content_manager) {
            this.sprite_batch = sprite_batch;
            this.content_manager = content_manager;
            white_pixel = content_manager.Load<Texture2D>("white_pixel");
            default_font = content_manager.Load<SpriteFont>("arial");
            spriteMC = content_manager.Load<Texture2D>("MainCharacter");
            spriteMC2 = content_manager.Load<Texture2D>("spriteMC2");
            spriteMC3 = content_manager.Load<Texture2D>("spriteMC3");
            spriteEC = content_manager.Load<Texture2D>("EnemyCharacter");
            spriteEC2 = content_manager.Load<Texture2D>("spriteEC2");
            spriteEC3 = content_manager.Load<Texture2D>("spriteEC3");
            SSJ = content_manager.Load<Texture2D>("SSJ");

            spriteVC = content_manager.Load<Texture2D>("VillainCharacter");
            spriteVC2 = content_manager.Load<Texture2D>("spriteVC2");
            spriteVC3 = content_manager.Load<Texture2D>("spriteVC3");

            spritesenemy = new Texture2D[3] { spriteEC2, spriteEC3, spriteEC };
            spritesmain = new Texture2D[4] { spriteMC2, spriteMC3, spriteMC, SSJ };
            spritesvillain = new Texture2D[3] { spriteVC2, spriteVC3, spriteVC };
            
        }

        private Microsoft.Xna.Framework.Color convert_color(Colour color) {
            switch (color) {
                case Colour.Black:
                    return Microsoft.Xna.Framework.Color.Black;
                case Colour.White:
                    return Microsoft.Xna.Framework.Color.White;
                case Colour.Blue:
                    return Microsoft.Xna.Framework.Color.Blue;
                case Colour.Pink:
                    return Microsoft.Xna.Framework.Color.Pink;
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

        public void drawMainCharacter(Point top_left_coordinate, float width, float height, Colour color) {
            // press num lock for Super Saiyan
            var key = Keyboard.GetState();
            int a;
            
            if (key.NumLock)
            {
                 a = rnd.Next(3, 4);
            }
            else if (key.IsKeyDown(Keys.W) || key.IsKeyDown(Keys.D) || key.IsKeyDown(Keys.A) || key.IsKeyDown(Keys.S))  {  a = rnd.Next(0, 2); }
            else { a = rnd.Next(2, 3); }
            

            sprite_batch.Draw(spritesmain[a], new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }

        public void drawEnemy(Point top_left_coordinate, float width, float height, Colour color, int index) {
            int a = rnd.Next(0, 3);
            Texture2D[][] een = new Texture2D[][] { spritesenemy, spritesvillain };
            
            
            sprite_batch.Draw(een[index][a], new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
        }

        // implement drawImage
        public void drawImage() {
            throw new NotImplementedException();
        }
    }
}
