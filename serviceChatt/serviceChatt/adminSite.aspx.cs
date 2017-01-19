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
    public partial class adminSite : System.Web.UI.Page
    {
        public static List<string> uporabniki = new List<string>();         // seznam prijavljenih uporabnikov --> kasneje prebrana iz baze
        public static List<string> regUporabniki = new List<string>();      // seznam vseh registriranih uporabnikov

        protected void Page_Load(object sender, EventArgs e)
        {
            update();

            if (!IsPostBack)
            {
                // ob prvem obisku strani se zabeleži oseba, ki/če se je vpisala
                String user = (String)Session["Name"];
                preveri(user);
            }
            pravice.Visible = false;
            uporabnik.Visible = false;
        }
        public void preveri(string usr)
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
                }
            }
            catch { }
        }

        protected void izbrisi_Click(object sender, EventArgs e)
        {
            string x = imeIzbris.Text;
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM sporocila WHERE username='" + x +"'" , con);
                SqlCommand cmd2 = new SqlCommand("DELETE FROM Uporabniki WHERE uporabniskoIme='" + x + "'", con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            uporabnik.Visible = true;

        }
        protected void update()
        {
            // posodobijo se sporočila in prijavljeni uporabniki
            Users.Text = get();
            registeredUsers.Text = getReg();
        }

        protected String getReg()
        {
            regUporabniki.Clear();

            // pogleda kaj je v tabeli "Pogovor" in gre čez njene vrstice ter vsebino zapisuje v sezname o registriranih uporabnikih
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT uporabniskoIme FROM Uporabniki", con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            regUporabniki.Add(reader.GetString(0));

                        }
                    }
                }
                con.Close();
            }
            return string.Join("\n", regUporabniki.ToArray());

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

        protected void Refresh_Click(object sender, EventArgs e)
        {
            update();
        }

        protected void dodaj_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Uporabniki SET admin='1' WHERE uporabniskoIme='" + imeUpravljaj.Text + "'";
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            pravice.Text = "Pravice so bile dodane!";
            pravice.Visible = true;
        }

        protected void odstrani_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Uporabniki SET admin='0' WHERE uporabniskoIme='" + imeUpravljaj.Text + "'";
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            pravice.Text = "Pravice so bile odstranjene!";
            pravice.Visible = true;
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            logou();
            Response.Redirect("Login.aspx");
        }
        public void logou()
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
    }
}