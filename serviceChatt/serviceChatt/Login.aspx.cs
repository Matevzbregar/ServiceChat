using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serviceChatt
{
    public partial class Login : System.Web.UI.Page
    {
        static List<string> users = new List<string>();             // seznam uporabnikov       -->     kasneje prebran iz baze
        static List<string> passwds = new List<string>();           // seznam gesel             -->     kasneje prebran iz baze
        static List<string> imePriimek = new List<string>();        // seznam imen in priimkov  -->     kasneje prebran iz baze

        static Boolean h, g, k = false;


        protected void Page_Load(object sender, EventArgs e)
        {
            // ob zagonu strani še ni prijavljenega uporabnika zato je Ime v seji prazno
            Session["Name"] = "";

            // opozorila se skrijejo
            registracija.Visible = false;
            napacnoG.Visible = false;
            napacno.Visible = false;
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            // kliče funkcijo, ki napolni tabele o uporabniških računih
            getUsers();
            Boolean t = false;

            // ko kliknemo gumb "Prijava" preverimo če vnešen uporabnik ustreza kateremu iz tabele registriranih uporabnikov
            for (int i = 0; i < users.Count; i++)
            {
                // iz tabele ločimo podatke --> uporabniško ime in geslo
                String uIme = users[i];
                String geslo = passwds[i];

                // če vnešen uporabnik ustreza kateremu iz baze se ta uporabnik zapiše v listo prijavljenih uporabnikov
                if (uIme == Username.Text && geslo == convertToMD5(Password.Text))
                {
                    // insert into DB
                    string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();
                        string query = "INSERT INTO Pogovor(username) VALUES('" + imePriimek[i] + "')";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    // zapomnimo si tudi njegovo ime
                    Session["Name"] = imePriimek[i];
                    t = true;
                }
            }
            // vnesen uporabnik je pravilen, zato sledi preusmeritev na stran Chat
            if (t)
            {
                h = false;
                registracija.Visible = false;
                napacno.Visible = false;
                Response.Redirect("Chat.aspx");
            }
            // vnešen uporabnik ni pravilen, zato spremenimo izpisana opozorila za napačno geslo in pobrišemo textboxe
            else
            {
                napacno.Visible = true;
                pobrisi();
            }
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

        protected void pobrisi()
        {
            ime.Text = "";
            uporabniskoI.Text = "";
            geslo1.Text = "";
            geslo2.Text = "";
            Username.Text = "";
            Password.Text = "";
        }

        protected void loginAdmin_Click(object sender, EventArgs e)
        {
            Boolean admin = false;
            // kliče funkcijo, ki napolni tabele o uporabniških računih
            getUsers();

            // ko kliknemo gumb "Prijava" preverimo če vnešen uporabnik ustreza kateremu iz tabele registriranih uporabnikov
            for (int i = 0; i < users.Count; i++)
            {
                // iz tabele ločimo podatke --> uporabniško ime in geslo
                String uIme = users[i];
                String geslo = passwds[i];

                // če vnešen uporabnik ustreza kateremu iz baze se ta uporabnik zapiše v listo prijavljenih uporabnikov
                if (uIme == Username.Text && geslo == convertToMD5(Password.Text))
                {
                    // preveri ali je uporabnik admin
                    string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                    string query = "SELECT admin FROM Uporabniki WHERE uporabniskoIme='" + uIme + "' AND geslo='" + geslo + "'";
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(query, con))
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                admin = reader.GetBoolean(0);
                            }
                        }
                        con.Close();
                    }
                    if (admin)
                    {
                        Session["Name"] = imePriimek[i];
                        h = false;
                        registracija.Visible = false;
                        napacno.Visible = false;
                        Response.Redirect("adminSite.aspx");
                    }
                }
            }
            napacno.Visible = true;
            pobrisi();

        }

        protected void regBtn_Click(object sender, EventArgs e)
        {
            // če sta vneseni gesli za registracijo ustrezni se zapišejo podatki v bazo
            if (preveriGeslo())
            {
                // nastavijo se pogoji za izpis obvestil
                registracija.Visible = true;
                napacnoG.Visible = false;

                // insert into DB
                string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    string query = "INSERT INTO Uporabniki(uporabniskoIme, imePriimek, geslo, admin) VALUES('" + uporabniskoI.Text + "', '" + ime.Text + "', '" + convertToMD5(geslo1.Text) + "', '0')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            // v nasprotem primeru do zapisa v bazo ne pride, nastavijo se pravilni izpisi obvestil
            else
            {
                napacnoG.Visible = true;
                registracija.Visible = false;
            }
            // pobriše textboxe
            pobrisi();
        }
        public Boolean preveriGeslo()
        {
            // če je geslo prekratko ali če se gesli ne ujemata takoj vrne pogoj za napačno vnesene podatke o registraciji
            if (geslo1.Text.Length < 8 || geslo1.Text != geslo2.Text) { return false; }
            Boolean ss;

            // nato preverja velike črke in številke
            if (geslo1.Text.Count(c => char.IsUpper(c)) > 1 && geslo1.Text.Count(c => char.IsNumber(c)) > 1)
            {
                // če je število velikih črk in številk ustrezno preveri še znake
                if (geslo1.Text.Contains("?") || geslo1.Text.Contains(".") || geslo1.Text.Contains("*") || geslo1.Text.Contains("!") || geslo1.Text.Contains(":")) { ss = true; }
                else { ss = false; }
            }
            else { ss = false; }

            return ss;
        }
    }
}