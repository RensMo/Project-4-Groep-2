using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class Example
    {

        static void Main()
        {
            string cs = @"server=localhost;userid=root;
            password=123kaan;database=world";

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string stm = "SELECT * FROM city";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetInt32(0) + ": "
                        + rdr.GetString(1));
                }

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

                if (conn != null)
                {
                    conn.Close();
                }

            }

            Console.ReadKey();

        }

        
        
    }

    

}
