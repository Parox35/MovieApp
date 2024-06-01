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
            this.mainFrame = mainFrame; //Get the frame of the MainWindow
            this.movie = m;                 //Get the movie 
            this.sqlConnection = db;            //Get the connection to database

            //Put all the information about the movie we click on it to see from the home page
            txbName.Text = this.movie.Name; 
            txbDuration.Text = this.movie.Duration.ToString();
            txbSummary.Text = this.movie.Summary;
            //Check if the user click on new button (true) or on the movie in datagrid
            //If it is true, it show only the New button and return other wise it show the button Edit and Delete with the return button
            if (b)
            {
                btnNew.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Hidden;
                btnEdit.Visibility = Visibility.Hidden;
            }
            else
            {
                btnNew.Visibility = Visibility.Hidden;
            }
        }

        // Check if all the text are fill and after update the movie in the database to return in the home page in the end when it is done
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txbDuration.Text != string.Empty && txbName.Text != string.Empty && txbSummary.Text != string.Empty && btnNew.Visibility == Visibility.Hidden)
            {
                string name = txbName.Text;
                float duration = float.Parse(txbDuration.Text);
                string summary = txbSummary.Text;

                string query = "UPDATE Movies SET Name = @Name, Duration = @Duration, Summary = @Summary WHERE Id = @MovieId";

                
                SqlCommand command = new SqlCommand(query, sqlConnection);
                //Use parameterized query to protect against SQL injection  
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

        // Ask the user if he agrre to delete the movie from the database
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

                    // Show a message telling that the movie has been deleted and go back to the home page
                    MessageBox.Show("The movie has been deleted", "Movie deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    mainFrame.Navigate(new Home(mainFrame));
                }
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //Check if all text are fill
            if (txbDuration.Text != string.Empty && txbName.Text != string.Empty && txbSummary.Text != string.Empty)
            {
                //Get all information about the new movie
                string name = txbName.Text;
                float duration = float.Parse(txbDuration.Text);
                string summary = txbSummary.Text;

                //Create the text of the query  
                string sql = "INSERT INTO Movies (Name, Duration, Summary) VALUES (@Name, @Duration, @Summary)";

                SqlCommand command = new SqlCommand(sql, sqlConnection);
                //Use parameterized query to protect against SQL injection  
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Duration", duration);
                command.Parameters.AddWithValue("@Summary", summary);

                //Put in the database
                try
                {
                    command.ExecuteNonQuery();
                    //Show a messagebox to tell the user that the movie has been created and go back in the home page
                    MessageBox.Show("The movie has been added", "Movie added", MessageBoxButton.OK, MessageBoxImage.Information);
                    mainFrame.Navigate(new Home(mainFrame));
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                }

                
            }
        }

        //Button to return in the home page without editing a movie
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Home(mainFrame));
        }
    }
}
