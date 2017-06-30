using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RobotParty
{
    public class UdpHandler
    {
        UdpClient udp;
        int port;

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        bool listening;
        Thread listeningThread;

        List<string> keys;

        public UdpHandler(int port)
        {
            this.port = port;
            udp = new UdpClient(this.port);
            StartListener();
        }

        public void Send(string json)
        {
            if (RemoteIpEndPoint.Address != IPAddress.Any && RemoteIpEndPoint.Port != 0)
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(json);
                udp.Send(sendBytes, sendBytes.Length, RemoteIpEndPoint.Address.ToString(), RemoteIpEndPoint.Port);
            }
        }

        public void StartListener()
        {
            if (!listening)
            {
                listeningThread = new Thread(Listen);
                this.listening = true;
                listeningThread.Start();
                listeningThread.IsBackground = false; //True?
            }
        }

        public void StopListener()
        {
            listening = false;
        }

        private void Listen()
        {
            while (listening)
            {
                //Console.WriteLine("Waiting to receive");
                Byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine(receiveString);
            }
        }

        public List<string> Getkeys()
        {
            List<string> returnlist = keys;
            keys.RemoveRange(0, returnlist.Count);
            return returnlist;

        }

    }

    class PCInputAdapter : IinputManager
    {
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

    public interface IDrawManager
    {
        void drawRectangle(Point top_left_coordinate, float width, float height, Colour color);
        void drawText(string text, Point top_left_coordinate, int size, Colour color);
        void drawImage();
        void drawMainCharacter(Point top_left_coordinate, float width, float height, Colour color);
        void drawEnemy(Point top_left_coordinate, float width, float height, Colour color);
    }

    public enum Colour { White, Black, Blue, Pink };

    public class MonoGameAdapter : IDrawManager
    {
        SpriteBatch sprite_batch;
        ContentManager content_manager;
        Texture2D spriteMC;
        Texture2D spriteEC;
        Texture2D white_pixel;
        SpriteFont default_font;
        Game game;
        UdpHandler udphandler;
       
        public MonoGameAdapter(SpriteBatch sprite_batch, ContentManager content_manager, UdpHandler udphandler)
        {
            this.sprite_batch = sprite_batch;
            this.content_manager = content_manager;
            white_pixel = content_manager.Load<Texture2D>("white_pixel");
            default_font = content_manager.Load<SpriteFont>("arial");
            spriteMC = content_manager.Load<Texture2D>("MainCharacter");
            spriteEC = content_manager.Load<Texture2D>("EnemyCharacter");
            this.udphandler = udphandler;
        }

        private Microsoft.Xna.Framework.Color convert_color(Colour color)
        {
            switch (color)
            {
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
        public void drawRectangle(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(white_pixel, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
            string json = String.Format("{{\"func\":\"{0}\", \"args\":[\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",,\"{6}\"]}}",
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                "white_pixel",
                top_left_coordinate.X,
                top_left_coordinate.Y,
                width,
                height,
                color
                );
             udphandler.Send(json);
        }

        public void drawText(string text, Point top_left_coordinate, int size, Colour color)
        {
            sprite_batch.DrawString(default_font, text, new Vector2(top_left_coordinate.X, top_left_coordinate.Y), convert_color(color));
            string json = String.Format("{{\"func\":\"{0}\", \"args\":[\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]}}",
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                text,
                top_left_coordinate.X,
                top_left_coordinate.Y,
                size,
                color
                );
           udphandler.Send(json);
        }

        public void drawMainCharacter(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(spriteMC, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
            string json = String.Format("{{\"func\":\"{0}\", \"args\":[\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]}}",
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                top_left_coordinate.X,
                top_left_coordinate.Y,
                width,
                height,
                color
                );
            udphandler.Send(json);
        }

        public void drawEnemy(Point top_left_coordinate, float width, float height, Colour color)
        {
            sprite_batch.Draw(spriteEC, new Rectangle((int)top_left_coordinate.X, (int)top_left_coordinate.Y, (int)width, (int)height), convert_color(color));
            string json = String.Format("{{\"func\":\"{0}\", \"args\":[\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]}}",
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                top_left_coordinate.X,
                top_left_coordinate.Y,
                width,
                height,
                color
                );
        }

        // implement drawImage
        public void drawImage()
        {
            throw new NotImplementedException();
        }
    }
}