using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
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

namespace MovieApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Details.xaml
    /// </summary>
    public partial class Details : Page
    {
        private Frame mainFrame;
        private Movie movie;
        private SqlConnection sqlConnection;
        public Details(Frame mainFrame, Movie m, Boolean b, SqlConnection db)
        {
            InitializeComponent();
            this.mainFrame = mainFrame;
            this.movie = m;
            this.sqlConnection = db;
            txbName.Text = this.movie.Name;
            txbDuration.Text = this.movie.Duration.ToString();
            txbSummary.Text = this.movie.Summary;
            if (b)
            {
                btnNew.Visibility = Visibility.Visible;
            }
            else
            {
                btnNew.Visibility = Visibility.Hidden;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txbDuration.Text != string.Empty && txbName.Text != string.Empty && txbSummary.Text != string.Empty && btnNew.Visibility == Visibility.Hidden)
            {
                string name = txbName.Text;
                float duration = float.Parse(txbDuration.Text);
                string summary = txbSummary.Text;

                string query = "UPDATE Movies SET Name = @Name, Duration = @Duration, Summary = @Summary WHERE Id = @MovieId";

                
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Duration", duration);
                command.Parameters.AddWithValue("@Summary", summary);
                command.Parameters.AddWithValue("@MovieId", movie.Id);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }
                

                mainFrame.Navigate(new Home(mainFrame));
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(btnNew.Visibility == Visibility.Hidden)
            {
                if (MessageBox.Show($"Are you sure to delete {movie.Name}", "Delete Movie", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    string sql = $"DELETE FROM Movies WHERE Id = {movie.Id}";
                    SqlCommand command = new SqlCommand(sql, sqlConnection);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.DeleteCommand = command;
                    adapter.DeleteCommand.ExecuteNonQuery();

                    MessageBox.Show("The movie has been deleted", "Movie deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    mainFrame.Navigate(new Home(mainFrame));
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (txbDuration.Text != string.Empty && txbName.Text != string.Empty && txbSummary.Text != string.Empty)
            {
                string name = txbName.Text;
                float duration = float.Parse(txbDuration.Text);
                string summary = txbSummary.Text;

                string sql = "INSERT INTO Movies (Name, Duration, Summary) VALUES (@Name, @Duration, @Summary)";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Duration", duration);
                command.Parameters.AddWithValue("@Summary", summary);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }

                MessageBox.Show("The movie has been added", "Movie added", MessageBoxButton.OK, MessageBoxImage.Information);
                mainFrame.Navigate(new Home(mainFrame));
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Home(mainFrame));
        }
    }
}
