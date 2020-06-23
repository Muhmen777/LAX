using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace Filmdatabase
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        SqlConnection SQLCONN = new SqlConnection(@"Data Source=10.0.5.104,1433;Network Library=DBMSSOCN;Initial Catalog=LAXDATABASE; User ID=Myuser; Password=password;"); //Vores stig til databasem
        SqlCommand Insert;
        SqlCommand Update;
        SqlCommand Delete;
        SqlCommand LoadDisplayTable;
        public Main()
        {
            InitializeComponent();  
            Genrebox.Items.Add("Erotic");
            Genrebox.Items.Add("Romance");
            Genrebox.Items.Add("Action");
            Genrebox.Items.Add("Crime");
            Genrebox.Items.Add("Historical");
            Genrebox.Items.Add("Paranormal Romance");
        } 

        private void InsertButton_Click(object sender, RoutedEventArgs e)               //[1] Insert knap
        {
            if (MovieTitle.Text == "" || Director.Text == "" || ReleaseDate.SelectedDate == null)   //If statement, der tjekker om felterne er tomme
            {
                MessageBox.Show("There is a empty field. Please try again");                        //Hvis det er tilfældet, skal den fremvise denne boks til bruger
            }
            else                                                                                    //Hvis der er noget input i felterne, skal den køre koden i else-delen
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to Insert", "Confirmed Insert", MessageBoxButton.YesNo, MessageBoxImage.Question);    //Besked til bruger, som spørger, om han vil indsætte det indtastede data
                if (result == MessageBoxResult.Yes)                                             //Hvis svaret er Ja, skal den køre koden i if-delen 
                {
                    SQLCONN.Open();                                                             //Åbner Database                                            
                    Insert = new SqlCommand("INSERT INTO Filmnomineringer VALUES(@MovieTitle,  @Director, @ReleaseDate, @Genrebox)", SQLCONN);  //Insert til Database
                    Insert.Prepare();                                                           //Forbereder SQL
                    Insert.Parameters.AddWithValue("@MovieTitle", MovieTitle.Text);             //Parameternavn og tilføjer værdi
                    Insert.Parameters.AddWithValue("@Director", Director.Text);                 //Parameternavn og tilføjer værdi
                    Insert.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.SelectedDate);   //Parameternavn og tilføjer værdi
                    Insert.Parameters.AddWithValue("@Genrebox", Genrebox.SelectedItem);        //Parameternavn og tilføjer værdi
                    Insert.ExecuteNonQuery();                                                   //NON Query
                    MessageBox.Show("Data Inserted");                                           //Besked til bruger
                    SQLCONN.Close();                                                            //Database lukkes
                }
            }
        }
        private void Load_Click(object sender, RoutedEventArgs e)                       //[4] Load/display/read knap
        {
            SQLCONN.Open();                                                             //Åbner Database
            string Query = "SELECT FilmID, FilmTitle, Director, ReleaseDate, Genre from Filmnomineringer"; //SELECT fra database
            LoadDisplayTable = new SqlCommand(Query, SQLCONN);                          //SQLkommando i Visual studio
            LoadDisplayTable.ExecuteNonQuery();                                         //Udfører en Transact-SQL-sætning mod forbindelsen og returnerer antallet af berørte rækker
            SqlDataAdapter SQLDA = new SqlDataAdapter(LoadDisplayTable);                //Repræsentere data kommandoer, som bruges til dataset og update SQL base
            DataTable DTA = new DataTable("Filmnomineringer");                          //Ny Linje
            SQLDA.Fill(DTA);                                                            //Fylder data
            DGW.ItemsSource = DTA.DefaultView;                                          //Visninger af database
            SQLDA.Update(DTA);                                                          //Updatere databasen
            SQLCONN.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)               //[2] Update knap
        {
            SQLCONN.Open();                                                                                             //Åbner Databasen
            Update = new SqlCommand("UPDATE Filmnomineringer SET FilmTitle = @MovieTitle, Director = @Director, ReleaseDate = @ReleaseDate, Genre = @Genrebox WHERE FilmID =" + DGW.SelectedIndex, SQLCONN);    //Update Database
            Update.Parameters.Add("@MovieTitle", SqlDbType.VarChar).Value = MovieTitle.Text;                     //Parameternavn og tilføjer værdi  
            Update.Parameters.Add("@Director", SqlDbType.VarChar).Value = Director.Text;                         //Parameternavn og tilføjer værdi
            Update.Parameters.Add("@ReleaseDate", SqlDbType.SmallDateTime).Value = ReleaseDate.DisplayDate;      //Parameternavn og tilføjer værdi
            Update.Parameters.Add("@Genrebox", SqlDbType.VarChar).Value = Genrebox.SelectedItem;                 //Parameternavn og tilføjer værdi
            Update.ExecuteNonQuery();                                                                            //NON Query
            MessageBox.Show("Data Updated");                                                                     //Besked til bruger
            SQLCONN.Close();                                                                                     //Lukker databasen

        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)               //[3] Delete knap
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete?", "Confirmed Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);    //Besked til bruger, som spørger, om han vil indsætte det indtastede data
            if (result == MessageBoxResult.Yes)
            {
                SQLCONN.Open();                                                                                             //Åbner databasen
                Delete = new SqlCommand("DELETE Filmnomineringer WHERE FilmID =" + DGW.SelectedIndex, SQLCONN);             //DELETE Tabel
                Delete.ExecuteNonQuery();                                                                                   //NON QUERY
                MessageBox.Show("Data Deleted");                                                                            //Besked til bruger
                SQLCONN.Close();  
            }
            else
            {
                MessageBox.Show("Delete request is denied");
            }
                                                                                                      
        }
        private void LogOutClick_Click_1(object sender, RoutedEventArgs e)              //Log Out knap
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to log out?", "Confirmed Log Out", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Hide();
                MainWindow MW = new MainWindow();
                MW.Show();
            }
            else
            {
                MessageBox.Show("Log out request denied");
            }         
        }

        private void DGW_MouseDoubleClick(object sender, MouseButtonEventArgs et)
        {
            SQLCONN.Open();                                                             //Åbner Database
            string Query = "SELECT FilmTitle, Director, ReleaseDate, Genre from Filmnomineringer WHERE FilmID =" + DGW.SelectedValuePath;  //string med vores select
            SqlCommand CMD = new SqlCommand(Query, SQLCONN);                            //SQL Command
            SqlDataReader SDR = CMD.ExecuteReader();                                    //Læser data

            while (SDR.Read())                                                          //While loop, der 'getter' de forskellige values
            {
                string film = SDR.GetString(0);                                         //Laver en string med navnet 'film', som har position 0
                string director = SDR.GetString(1);                                     //Laver en string med navnet 'director', som har position 1
                DateTime DT = SDR.GetDateTime(2);                                       //Laver en string med navnet 'DT', som har position 2
                string genre = SDR.GetString(3);

                MovieTitle.Text = film;                                                 //Konvertere det til deres txt
                Director.Text = director;                                               //Konvertere det til deres txt
                ReleaseDate.SelectedDate = DT;                                          //Konvertere det til deres txt
                Genrebox.SelectedItem = genre;
            }
            SQLCONN.Close();
        }
    }
}
