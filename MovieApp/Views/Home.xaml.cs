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
            this.mainFrame = mainFrame;
            dbConnection = new SqlConnection(Helpers.Constants.DbConnectionString);
            connect();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            connect();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(txbName.Text != string.Empty)
            {
                string name = txbName.Text;
                LoadData($"SELECT * FROM Movies WHERE Name LIKE '{name}%' ORDER BY Name");
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = new Movie();
            if(dbConnection != null)
            {
                mainFrame.Navigate(new Details(mainFrame, movie, true, dbConnection));
            }
        }

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

        private void dgrMovies_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (dgrMovies.SelectedItem != null)
            {
                Movie movie = dgrMovies.SelectedItem as Movie;
                if (movie != null)
                {
                    mainFrame.Navigate(new Details(mainFrame, movie, false, dbConnection));
                }
            }
        }

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
