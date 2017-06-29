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
using Microsoft.Xna.Framework.Input.Touch;
using MySql.Data.MySqlClient;
using System.Data;

namespace RobotDroid
{
    public interface IonCollision
    {
        bool Collision(Ielement element1, Ielement element2);
    }
    // simple collision class, takes 2 elements and check if they collide. return true/false.
    public class onCollision : IonCollision
    {
        bool collision;
        float el1X;
        float el1Y;
        float el2X;
        float el2Y;


        public bool Collision(Ielement element1, Ielement element2)
        {
            el1X = element1.getPos().Item1;
            el1Y = element1.getPos().Item2;
            el2X = element2.getPos().Item1;
            el2Y = element2.getPos().Item2;
            collision = false;

            if (element1 is EnemyCharacter && element2 is FriendlyBullet)
            {
                el1Y += 30;
                el1X += 40;
                el2X += 2;
                el2Y += 2;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;
                                Console.WriteLine("enemychar");

                            }
                        }
                    }
                }
            }
            else if (element1 is VillainCharacter && element2 is FriendlyBullet)
            {
                el1Y += 30;
                el1X += 40;
                el2X += 2;
                el2Y += 2;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;
                                Console.WriteLine("villainchar");

                            }
                        }
                    }
                }
            }

            else if (element1 is MainCharacter && element2 is EnemyBullet)
            {
                el1X += 40;
                el1Y += 40;
                el2X += 2;
                el2Y += 2;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;

                            }
                        }
                    }
                }
            }

            else if (element1 is MainCharacter && element2 is PickUpCharacter)
            {
                el1Y += 30;
                el1X += 50;
                el2X += 10;
                el2Y += 10;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;
                                Console.WriteLine("main/pickup");

                            }
                        }
                    }
                }
            }
            else if (element1 is MainCharacter && element2 is EnemyCharacter)
            {
                el1Y += 40;
                el1X += 40;
                el2X += 50;
                el2Y += 50;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;
                                Console.WriteLine("main/enemy");

                            }
                        }
                    }
                }
            }

            else if (element1 is MainCharacter && element2 is VillainCharacter)
            {
                el1Y += 40;
                el1X += 40;
                el2X += 50;
                el2Y += 50;
                if (el2X > el1X)
                {
                    if (el2X < (el1X + 30))
                    {
                        if (el2Y > el1Y)
                        {
                            if (el2Y < (el1Y + 50))
                            {
                                collision = true;
                                Console.WriteLine("main/villain");

                            }
                        }
                    }
                }
            }


            else
            {
                collision = false;

            }
            return collision;
        }
    }

    public interface IinputManager
    {
        List<string> onInput();
    }

    public interface Ielementvisitor
    {
        void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt);
        void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt);
        void onScreenmanager(ScreenManager screenmanager, float dt, MainCharacter MainCharacter);
        void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt);
        void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt);
        void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index);
        void onText(text text, ScreenManager screenmanager);
        void onButton(Button element, ScreenManager screenmanager, float dt, MainCharacter character);
    }

    // updatevisitor is called by each element. here we describe the update logic for each element. 
    public class UpdateVisitor : Ielementvisitor
    {
        IinputManager inputmanager;
        IonCollision collisioncalculator;
        List<Ielement> newlist = new List<Ielement>();
        List<Ielement> removelist = new List<Ielement>();
        float EnemyTimeCounter = 0.0f;
        float FriendlyTimeCounter = 1000.0f;
        float lastEnemyBullet;
        float lastFriendlyBullet;
        bool pickupchar = true;
        int level = 0;
        bool database = true;
        bool GetTop5 = true;
        int score;
        int scoreindex = 1;




        public UpdateVisitor(IinputManager inputmanager, IonCollision collisioncalculator)
        {
            this.inputmanager = inputmanager;
            this.collisioncalculator = collisioncalculator;
        }

        // on enemy character first changes the position, and then fires a bullet every second in a random direction.
        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt)
        {

            Tuple<float, float> characterpos = new Tuple<float, float>(character.getPos().Item1 + 25, character.getPos().Item2 + 25);

            foreach (var direction in character.GetDirection())
            {
                character.Move(direction, dt);
            }



            EnemyTimeCounter += dt;
            if (EnemyTimeCounter > 1000.0f)
            {
                EnemyTimeCounter = 0.0f;

                switch (character.RandomShot())
                {

                    case 0:
                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(1, 0), screenmanager));
                        break;
                    case 1:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(1, 1), screenmanager));
                        break;
                    case 2:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(0, 1), screenmanager));
                        break;
                    case 3:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(-1, 0), screenmanager));
                        break;
                    case 4:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(-1, 1), screenmanager));
                        break;
                    case 5:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(0, -1), screenmanager));
                        break;
                    case 6:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(-1, -1), screenmanager));
                        break;
                    case 7:

                        newlist.Add(new EnemyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>(1, -1), screenmanager));
                        break;

                }
            }

        }

        // here we update maincharacter. check if health is below 0, check for any collisions with opponents, and check for key input and respond properly
        public void onMainCharacter(MainCharacter character, ScreenManager screenmanager, float dt)
        {

            if (character.health < 0)
            {
                Console.WriteLine("you lose");
                removelist.Add(character);
            }

            foreach (var el in screenmanager.elements)
            {
                if (el is EnemyBullet)
                {

                    if (collisioncalculator.Collision(character, el))
                    {

                        character.health -= 50;
                        removelist.Add(el);
                        break;
                    }
                }

                else if (el is EnemyCharacter)
                {

                    if (collisioncalculator.Collision(character, el))
                    {
                        removelist.Add(character);
                        Console.WriteLine("coll enemy");
                        break;
                    }
                }

                else if (el is VillainCharacter)
                {

                    if (collisioncalculator.Collision(character, el))
                    {
                        removelist.Add(character);
                        Console.WriteLine("coll villain");
                        break;
                    }
                }

                else if (el is PickUpCharacter)
                {

                    if (collisioncalculator.Collision(character, el))
                    {
                        removelist.Add(el);
                        screenmanager.score += 100;
                        Console.WriteLine("coll pickup character");
                        break;
                    }
                }
            }




            foreach (var el in inputmanager.onInput())
            {
                if (el == "A") { character.Move("left", dt); }
                if (el == "D") { character.Move("right", dt); }
                if (el == "W") { character.Move("up", dt); }
                if (el == "S") { character.Move("down", dt); }

            }

            FriendlyTimeCounter += dt;
            if (FriendlyTimeCounter > 500.0f)
            {
                FriendlyTimeCounter = 0.0f;
                foreach (var el in inputmanager.onInput())
                {

                    if (el == "UpRight")
                    {

                        var directionX = 1;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2 ), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "UpLeft")
                    {
                        var directionX = -1;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "DownLeft")
                    {
                        var directionX = -1;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }

                    if (el == "DownRight")
                    {
                        var directionX = 1;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Up")
                    {
                        var directionX = 0;
                        var directionY = -1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Down")
                    {
                        var directionX = 0;
                        var directionY = 1;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Right")
                    {
                        var directionX = 1;
                        var directionY = 0;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1, character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                        break;
                    }
                    if (el == "Left")
                    {
                        var directionX = -1;
                        var directionY = 0;
                        newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 , character.position.Item2), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                        break;
                    }
                }
            }
        }


        public void onPickUpCharacter(PickUpCharacter character, ScreenManager screenmanager, float dt)
        {
            //Nothing to update. Stays at same spot.

        }

        public void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index)
        {
            // Random rnd = new Random();
            // int index = rnd.Next(1, 4);


            if (index >= 1 && index <= 100)
            {
                character.position = new Tuple<float, float>(character.position.Item1 + 1, character.position.Item2);

                character.IndexPlus(1);
                // if (indextwo == 10) {  }

            }
            else if (index >= 101 && index <= 200)
            {
                character.position = new Tuple<float, float>(character.position.Item1, character.position.Item2 - 1);

                character.IndexPlus(1);
                // if (indextwo == 20) {  }
            }
            else if (index >= 201 && index <= 300)
            {
                character.position = new Tuple<float, float>(character.position.Item1 - 1, character.position.Item2);

                character.IndexPlus(1);
                // if (indextwo == 30) {  }
            }
            else if (index >= 301 && index <= 400)
            {
                character.position = new Tuple<float, float>(character.position.Item1, character.position.Item2 + 1);


                character.IndexPlus(1);
            }

            else if (index >= 401 && index <= 500)
            {
                character.IndexSet(1);
            }

        }

        // here we check for collision between projectiles and enemies, if so, remove them
        public void onProjectile(Projectile projectile, ScreenManager screenmanager, float dt)
        {
            projectile.position = new Tuple<float, float>(projectile.position.Item1 + projectile.direction.Item1, projectile.position.Item2 + projectile.direction.Item2);
            foreach (var el in screenmanager.elements)
            {
                if (el is VillainCharacter)
                {
                    if (collisioncalculator.Collision(el, projectile))
                    {
                        screenmanager.score += 5;
                        removelist.Add(el);
                        break;
                    }
                }
                else if (el is EnemyCharacter)
                {
                    if (collisioncalculator.Collision(el, projectile))
                    {
                        screenmanager.score += 15;
                        removelist.Add(el);
                        break;
                    }
                }
            }
        }
        // foreach element in the list, update. foreach element in list of new elements, add them, foreach element in list of removed element, remove them.
        // also check if elements are out of the borders. 
        public void onScreenmanager(ScreenManager screenmanager, float dt, MainCharacter MainCharacter)
        {
            pickupchar = false;
            bool mainchar = false;
            foreach (Ielement el in screenmanager.elements)
            {
                if (el.getPos().Item1 < 0 || el.getPos().Item2 < 0 || el.getPos().Item1 > 1080 || el.getPos().Item2 > 1330)
                {
                    if (el is Projectile) { removelist.Add(el); }
                }
                if (el is Character)
                {
                    // check for borders
                    var pos = el.getPos();
                    if (pos.Item1 < -20) { el.setPos(new Tuple<float, float>(-19, pos.Item2)); }
                    if (pos.Item1 > 1040) { el.setPos(new Tuple<float, float>(1039, pos.Item2)); }
                    if (pos.Item2 < 5) { el.setPos(new Tuple<float, float>(pos.Item1, 6)); }
                    if (pos.Item2 > 1290) { el.setPos(new Tuple<float, float>(pos.Item1, 1289)); }
                }


                if (el is PickUpCharacter)
                {
                    pickupchar = true;
                }

                if (el is MainCharacter)
                {
                    mainchar = true;
                }

                el.Update(this, dt);
            }
            // change level logic
            if (pickupchar == false)
            {
                level += 1;
                screenmanager.Create(level);
            }
            // end of the game logic
            if (mainchar == false)
            {
                if (screenmanager.lives != 0)
                {
                    screenmanager.lives -= 1;
                    MainCharacter.health = 200;
                    screenmanager.Create(level);



                }

                else
                {
                    if (database)
                    {
                        // set up a database connection and add a score
                        string cs = @"server=185.114.157.171;userid=specific_groep2;password=123kaan;database=specific_project4";
                        MySqlConnection con = null;
                        MySqlDataReader rdr = null;

                        try
                        {
                            con = new MySqlConnection(cs);
                            con.Open();

                            string stm = "INSERT INTO score values (" + screenmanager.score.ToString() + ")";
                            MySqlCommand cmd = new MySqlCommand(stm, con);
                            rdr = cmd.ExecuteReader();

                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine("Error: {0}", ex.ToString());

                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                            }

                            if (con != null)
                            {
                                con.Close();
                            }
                            database = false;

                        }
                    }


                    if (GetTop5)
                    {
                        // set up a database connection and add a score
                        string cs = @"server=185.114.157.171;userid=specific_groep2;password=123kaan;database=specific_project4";
                        MySqlConnection con = null;
                        MySqlDataReader rdr = null;

                        try
                        {
                            con = new MySqlConnection(cs);
                            con.Open();

                            string stm = "SELECT * FROM score ORDER BY score DESC LIMIT 5";
                            MySqlCommand cmd = new MySqlCommand(stm, con);
                            rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                score = rdr.GetInt32("score");
                                screenmanager.Top5Score.Add(new text(new Tuple<float, float>(390, 220 + (scoreindex * 100)), screenmanager, "Score " + scoreindex.ToString() + ":  " + score.ToString()));
                                scoreindex += 1;
                            }
                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine("Error: {0}", ex.ToString());

                        }
                        finally
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                            }

                            if (con != null)
                            {
                                con.Close();
                            }
                            GetTop5 = false;

                        }
                    }
                    level = 10;
                    screenmanager.Create(level);


                }
            }


            int i = 0;
            foreach (Ielement el in newlist)
            {

                screenmanager.elements.Add(el);
                i += 1;
            }

            int x = 0;
            while (x < removelist.Count())
            {
                Console.WriteLine(removelist[x].ToString());
                screenmanager.elements.Remove(removelist[x]);
                x += 1;
            }
            removelist = new List<Ielement>();
            newlist = new List<Ielement>();

        }

        public void onText(text text, ScreenManager screenmanager)
        {
            // nothiing to do here
        }

        public void onButton(Button element, ScreenManager screenmanager, float dt, MainCharacter character)
        {
            var touchCollection = TouchPanel.GetState();
            FriendlyTimeCounter += dt;
            foreach (TouchLocation tl in touchCollection)
            {

                //Only Fire Select Once it's been released
                if (touchCollection[0].State == TouchLocationState.Moved || touchCollection[0].State == TouchLocationState.Pressed)
                {

                    if (element.is_intersecting(tl.Position))
                    {
                        if (element.text == "right")
                        {

                            character.Move("right", dt);
                        }
                        if (element.text == "left")
                        {
                            character.Move("left", dt);
                        }
                        if (element.text == "up")
                        {
                            character.Move("up", dt);
                        }
                        if (element.text == "down")
                        {
                            
                            character.Move("down", dt);
                        }
                        if (element.text == "upleft")
                        {
                            character.Move("upleft", dt);
                        }
                        if (element.text == "downleft")
                        {
                            character.Move("downleft", dt);
                        }
                        if (element.text == "upright")
                        {
                            character.Move("upright", dt);
                        }
                        if (element.text == "downright")
                        {
                            character.Move("downright", dt);
                        }

                        if (element.text == "shootup")
                        {
                            
                            
                                var directionX = 0;
                                var directionY = -1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                            
                        }
                        if (element.text == "shootdown")
                        {
                            
                            
                                var directionX = 0;
                                var directionY = 1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                            
                        }
                        if (element.text == "shootright")
                        {
                            
                            
                                var directionX = 1;
                                var directionY = 0;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                            
                        }
                        if (element.text == "shootleft")
                        {
                            
                            
                                var directionX = -1;
                                var directionY = 0;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY * dt / 1000))), screenmanager));
                            
                        }
                        if (element.text == "shootupright")
                        {
                            
                                var directionX = 1;
                                var directionY = -1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                            
                        }

                        if (element.text == "shootupleft")
                        {
                            
                                var directionX = -1;
                                var directionY = -1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY - 1000 * dt / 1000))), screenmanager));
                            
                        }

                        if (element.text == "shootdownleft")
                        {
                           
                                var directionX = -1;
                                var directionY = 1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX - 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                            
                        }

                        if (element.text == "shootdownright")
                        {
                            
                                var directionX = 1;
                                var directionY = 1;
                                newlist.Add(new FriendlyBullet(new Tuple<float, float>(character.position.Item1 + 55, character.position.Item2 + 55), new Tuple<float, float>((int)(Math.Round(directionX + 1000 * dt / 1000)), (int)(Math.Round(directionY + 1000 * dt / 1000))), screenmanager));
                            
                        }
                    }

                }
            



                //element.setPos(new Tuple<int, int>(element.top_left_corner.Item1, element.top_left_corner.Item2));

            
        }
    }

    }

    // this is where we describe the draw logic of each element, just like we update them in the updatevisitor
    class DrawVisitor : Ielementvisitor
    {
        IDrawManager drawmanager;

        public DrawVisitor(IDrawManager drawmanager)
        {
            this.drawmanager = drawmanager;
        }

        public void onButton(Button element, ScreenManager screenmanager, float dt, MainCharacter character)
        {
            var point = new Microsoft.Xna.Framework.Point(element.position.Item1, element.position.Item2);
            var width = element.width;
            var height = element.height;
            drawmanager.drawRectangle(point, width, height, Colour.White);
    }

        public void onEnemyCharacter(EnemyCharacter character, ScreenManager screenmanager, float dt)
        {
            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(character.position.Item1), (int)Math.Round(character.position.Item2));
            drawmanager.drawEnemy(point, 120, 120, Colour.White);
        }

        public void onMainCharacter(MainCharacter Character, ScreenManager screenmanager, float dt)
        {

            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(Character.position.Item1), (int)Math.Round(Character.position.Item2));
            drawmanager.drawMainCharacter(point, 120, 120, Colour.White);


        }

        public void onPickUpCharacter(PickUpCharacter Character, ScreenManager screenmanager, float dt)
        {
            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(Character.position.Item1), (int)Math.Round(Character.position.Item2));
            drawmanager.drawRectangle(point, 20, 20, Colour.Pink);
        }



        public void onProjectile(Projectile Projectile, ScreenManager screenmanager, float dt)
        {
            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(Projectile.position.Item1), (int)Math.Round(Projectile.position.Item2));
            drawmanager.drawRectangle(point, 4, 4, Colour.White);

        }

        public void onScreenmanager(ScreenManager ScreenManager, float dt, MainCharacter MainCharacter)
        {
            var LivesPoint = new Microsoft.Xna.Framework.Point(0, 0);
            var ScorePoint = new Microsoft.Xna.Framework.Point(430, 0);
            var HealthPoint = new Microsoft.Xna.Framework.Point(875, 0);
            drawmanager.drawText("health:" + MainCharacter.health.ToString(), HealthPoint, 10, Colour.Black);
            drawmanager.drawText("Score:" + ScreenManager.score.ToString(), ScorePoint, 10, Colour.Black);
            drawmanager.drawText("Lives left:" + ScreenManager.lives.ToString(), LivesPoint, 10, Colour.Black);
            foreach (text text in ScreenManager.Top5Score)
            {
                text.Draw(this, dt);
            }
            foreach (Ielement el in ScreenManager.elements)
            {
                el.Draw(this, dt);
            }
        }

        public void onText(text text, ScreenManager screenmanager)
        {
            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(text.getPos().Item1), (int)Math.Round(text.getPos().Item2));
            drawmanager.drawText(text.Text, point, 50, Colour.Black);
        }

        public void onVillainCharacter(VillainCharacter character, ScreenManager screenmanager, float dt, int index)
        {
            var point = new Microsoft.Xna.Framework.Point((int)Math.Round(character.position.Item1), (int)Math.Round(character.position.Item2));
            drawmanager.drawEnemy(point, 120, 120, Colour.White);
        }
    }
}
