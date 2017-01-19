using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serviceChatt
{
    public partial class Chat : System.Web.UI.Page
    {
        public static List<string> uporabniki = new List<string>();         // seznam prijavljenih uporabnikov --> kasneje prebrana iz baze
        public static List<string> besedilo = new List<string>();            // niz sporočil                    --> kasneje prebran iz baze
        static List<string> users = new List<string>();             // seznam uporabnikov       -->     kasneje prebran iz baze
        static List<string> passwds = new List<string>();           // seznam gesel             -->     kasneje prebran iz baze
        static List<string> imePriimek = new List<string>();        // seznam imen in priimkov  -->     kasneje prebran iz baze

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ob prvem obisku strani se zabeleži oseba, ki/če se je vpisala
                String user = (String)Session["Name"];
                preveri(user);
            }
        }
        protected void preveri(String usr)
        {
            // preveri ali je uporabnik prijavljen
            try
            {
                // če je dostopal do Chat strani brez prijave ga preusmeri na Login
                // v nasprotnem primeru se zabeleži njegovo ime in prikažejo se prijavljeni uporabniki ter sporočila
                if (usr == null || usr == "")
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    CurrentUser.Text = usr;
                    Users.Text = get();
                    update();
                }
            }
            catch { }
        }

        protected String get()
        {
            // vrne string ki ga pridobi iz baze prijavljenih uporabnikov
            uporabniki.Clear();

            // pogleda kaj je v tabeli "Pogovor" in gre čez njene vrstice ter vsebino zapisuje v sezname o registriranih uporabnikih
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT DISTINCT username FROM Pogovor", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            uporabniki.Add(reader.GetString(0));

                        }
                    }
                }
                con.Close();
            }
            return string.Join("\n", uporabniki.ToArray());
        }
        protected void update()
        {
            // posodobijo se sporočila in prijavljeni uporabniki
            Messages.Text = getMsg();
            Users.Text = get();
        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            // izbris trenutnega uporabnika in preusmeritev na Login okno
            izbrisi();
            Response.Redirect("Login.aspx");
        }
        protected void izbrisi()
        {
            // x je trenutni uporabnik
            string x = (string)Session["Name"];

            // preverjamo vse prijavljene uporabnike če so enaki trenutnemu in jih brišemo
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Pogovor WHERE username='" + x + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            // posodobimo vsa okna v Chat
            update();
        }
        public string getMsg()
        {
            // iz baze zapiše vsa sporočila v en niz
            besedilo.Clear();

            // pogleda kaj je v tabeli "Uporabnik" in gre čez njene vrstice ter vsebino zapisuje v sezname o registriranih uporabnikih
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT fullname, time, message FROM sporocila", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            besedilo.Add(reader.GetString(1)+" "+reader.GetString(0)+": "+reader.GetString(2));

                        }
                    }
                }
                con.Close();
            }
            return string.Join("\n", besedilo.ToArray());
        }
        protected void Refresh_Click(object sender, EventArgs e)
        {
            update();
        }

        protected void Send_Click(object sender, EventArgs e)
        {
            DateTime c = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
            string cas = string.Format("{0:HH:mm}", c);
            // ne pošilja praznih sporočil
            if (Message.Text != "")
            {
                string username = getUsername();
                // shrani in nato prikaže poslano sporočilo
                // insert into db
                string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string query = "INSERT INTO sporocila(username, fullname, time, message) VALUES('"+username+"', '"+(String)Session["Name"]+"', '["+ cas + "]', "+ "'"+ Message.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Message.Text = "";
                update();
            }
        }

        protected string getUsername()
        {
            getUsers();
            for(int i = 0; i< imePriimek.Count; i++)
            {
                if (imePriimek[i].Equals(Session["Name"]))
                    return users[i];
            }
            return "";
        }

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
    }
}