using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;

namespace serviceChatt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        static List<string> users = new List<string>();             // seznam uporabnikov       -->     kasneje prebran iz baze
        static List<string> passwds = new List<string>();           // seznam gesel             -->     kasneje prebran iz baze
        static List<string> imePriimek = new List<string>();        // seznam imen in priimkov  -->     kasneje prebran iz baze
        public static List<string> besedilo = new List<string>();

        public void getUsers()
        {
            users.Clear();
            passwds.Clear();
            imePriimek.Clear();
            // pogleda kaj je v tabeli "Uporabnik" in gre čez njene vrstice ter vsebino zapisuje v sezname o registriranih uporabnikih
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Uporabniki", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(reader.GetString(0));
                        passwds.Add(reader.GetString(2));
                        imePriimek.Add(reader.GetString(1));
                    }
                }
                con.Close();
            }
        }

        public String convertToMD5(String niz)
        {
            // pretvorimo in z StringBulider-jem združimo v String
            MD5 gesloMD5 = MD5.Create();
            byte[] b = gesloMD5.ComputeHash(Encoding.UTF8.GetBytes(niz));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private String[] currentUser()
        {
            WebOperationContext ctx = WebOperationContext.Current;
            string[] empty = new string[1] { "Napaka" };
            string authHeader = ctx.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            if (authHeader == null)
                return empty;

            string[] loginData = authHeader.Split(':');
            return loginData;
        }

        private bool AuthenticateUser()
        {
            getUsers();
            string[] loginData = currentUser();
            if (loginData.Length == 2)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Equals(loginData[0]) && passwds[i].Equals(convertToMD5(loginData[1])))
                        return true;
                }
            }
            return false;
        }

        public bool Login()
        {
            return AuthenticateUser();
        }

        public List<Message> Messages()
        {
            if (!AuthenticateUser())
                throw new FaultException("Napačno uporabniško ime ali geslo.");

            var retVal = new List<Message>();
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT username, time, message, ID FROM sporocila", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            retVal.Add(new Message { Username = reader.GetString(0), Time = reader.GetString(1), Text = reader.GetString(2), Id = reader.GetInt32(3) });
                        }
                    }
                }
                con.Close();
            }
            return retVal; 
        }

        public List<Message> Messages(string id)
        {
            if (!AuthenticateUser())
                throw new FaultException("Napačno uporabniško ime ali geslo.");

            var retVal = new List<Message>();
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT username, time, message, ID FROM sporocila WHERE ID >= "+ id+"", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            retVal.Add(new Message { Username = reader.GetString(0), Time = reader.GetString(1), Text = reader.GetString(2), Id = reader.GetInt32(3) });
                        }
                    }
                }
                con.Close();
            }
            return retVal;
        }

        public void Send(string message)
        {
            if (!AuthenticateUser())
                throw new FaultException("Napačno uporabniško ime ali geslo.");

            message = message.Replace('ß', ' ');

            WebOperationContext ctx = WebOperationContext.Current;
            string authHeader = ctx.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            string[] loginData = authHeader.Split(':');

            DateTime c = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
            string cas = string.Format("{0:HH:mm}", c);

            getUsers();
            String fullname = "";
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Equals(loginData[0]))
                    fullname = imePriimek[i];
            }
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string query = "INSERT INTO sporocila(username, fullname, time, message) VALUES('" + loginData[0] + "', '" + fullname + "', '[" + cas + "]', " + "'" + message + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
