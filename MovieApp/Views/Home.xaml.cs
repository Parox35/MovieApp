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
using System.Windows.Navigation;
using System.Windows.Shapes;

using MovieApp.Helpers;
using MovieApp.Models;
using MovieApp.Views;

using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Collections.ObjectModel;

namespace MovieApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private Frame mainFrame;

        SqlConnection dbConnection;

        public Home(Frame mainFrame)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;             //Get the frame of the MainWindow
            dbConnection = new SqlConnection(Helpers.Constants.DbConnectionString);     //Create connection to database
            connect();               //Connect to database
        }

        //Function to search movie by their name
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(txbName.Text != string.Empty)
            {
                string name = txbName.Text;     //Get the text enter by user
                LoadData($"SELECT * FROM Movies WHERE Name LIKE '%{name}%' ORDER BY Name"); // Function load to show all the movie which contains the text enter by the user
            }
        }

        // Navigate to details page to create a movie
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = new Movie();
            if(dbConnection != null)
            {
                mainFrame.Navigate(new Details(mainFrame, movie, true, dbConnection));
            }
        }

        // Show the movie in the datagrid. Take in parameter the text of the query
        public void LoadData(string sql)
        {
            SqlCommand command = new SqlCommand(sql, dbConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(reader);

                List<Movie> list = new List<Movie>();

                foreach (DataRow row in dt.Rows)
                {
                    Movie m = new Movie()
                    {
                        Id = int.Parse(row["Id"].ToString()),
                        Name = row["Name"].ToString(),
                        Duration = float.Parse(row["Duration"].ToString()),
                        Summary = row["Summary"].ToString(),
                    };
                    list.Add(m);
                }

                dgrMovies.ItemsSource = new ObservableCollection<Movie>(list);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            LoadData("SELECT * FROM Movies  ORDER BY Name");
        }

        //When you click on a item (movie) in the grid. It redirect you to the details page
        private void dgrMovies_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Check if there is an item selected (movie)
            if (dgrMovies.SelectedItem != null)
            {
                Movie movie = dgrMovies.SelectedItem as Movie;
                if (movie != null)
                {
                    mainFrame.Navigate(new Details(mainFrame, movie, false, dbConnection));
                }
            }
        }

        //Connect to the database
        private void connect()
        {
            try
            {
                dbConnection.Open();
                //MessageBox.Show("Database connection was successful!");

                LoadData("SELECT * FROM Movies  ORDER BY Name");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
