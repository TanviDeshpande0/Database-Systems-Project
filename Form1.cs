/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;




namespace BloodDriveDatabaseFE
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=BloodDrive;Trusted_Connection=True;";

        

        public Form1()
        {
            InitializeComponent();

        }

 

        private void txtCustomQuery_TextChanged(object sender, EventArgs e)
        {
            //string customQuery = "SELECT TOP 1 * FROM Donor;";
        }

        private void gridQueryOutput_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gridQueryOutput.AutoGenerateColumns = true;
        }

        //have to make sure to update the names here once you update them on the design changes 
        private void QueryButton_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!"); // Check if the connection opens

                    string customQuery = txtCustomQuery.Text; // Get the query from the TextBox
                    SqlCommand command = new SqlCommand(customQuery, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable); // Fill the DataTable with the query results
                    gridQueryOutput.DataSource = dataTable; // Bind the DataTable to the DataGridView

                    if (dataTable.Rows.Count > 0)
                    {
                        MessageBox.Show("Query returned " + dataTable.Rows.Count + " rows.");
                    }
                    else
                    {
                        MessageBox.Show("Query returned no results.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }
        private void MainUpdateButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the full update statement from the TextBox
                    string update = txtCustomQuery.Text.Trim();
                    // string update = "UPDATE Donor SET Address = 'Test Address' WHERE Donor_ID = 12";
                    //used a hardcoded input string since there are some issues with updating the strong 
                    //issue turned out to be due to accidentally putting the button name instead of textbox name

                    // Example input: UPDATE Donor SET Blood_Type = 'B+', DOB = '1990-05-15', Address = '123 New Street, Springfield' WHERE Donor_ID = 12;

                    // Ensure the input is not empty
                    if (string.IsNullOrWhiteSpace(update))
                    {
                        MessageBox.Show("Please enter a valid SQL UPDATE statement.");
                        return;
                    }

                    // Create the SQL command using the input statement
                    SqlCommand command = new SqlCommand(update, connection);

                    // Execute the UPDATE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found or nothing was updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void DeleteDonorButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxDonor.SelectedItem.ToString();
                    string value = DeletetxtBoxDonor.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Donor ID")
                    {
                        deleteQuery = "DELETE FROM Donor WHERE Donor_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Blood Type")
                    {
                        deleteQuery = "DELETE FROM Donor WHERE Blood_Type = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else if (criterion == "Birth Date")
                    {
                        deleteQuery = "DELETE FROM Donor WHERE DOB = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", DateTime.Parse(value));
                    }
                    else if (criterion == "Address")
                    {
                        deleteQuery = "DELETE FROM Donor WHERE Address = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", DateTime.Parse(value));
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBoxDonor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DeletetxtBoxDonor_TextChanged(object sender, EventArgs e)
        {

        }

        

       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void insertButtonDonor_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = insertTxtDonor.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 4)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Donor_ID, Blood_Type, DOB, Address");
                        return;
                    }

                    // Trim the values and parse them
                    string donor_ID = inputValues[0].Trim();
                    string blood_type = inputValues[1].Trim();
                    DateTime dob;
                    string address = inputValues[3].Trim();

                    // Validate and parse the birthdate
                    if (!DateTime.TryParse(inputValues[2].Trim(), out dob))
                    {
                        MessageBox.Show("Invalid birthdate format. Please enter a valid date.");
                        return;
                    }

                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Donor (donor_ID, blood_type, dob, address) VALUES (@Donor_ID, @Blood_Type, @DOB, @Address)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Donor_ID", donor_ID);
                    command.Parameters.AddWithValue("@Blood_Type", blood_type);
                    command.Parameters.AddWithValue("@DOB", dob);
                    command.Parameters.AddWithValue("@Address", address);

                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

       

      


       
        private void quitButton_Click_1(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void InsertButtonDriver_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtDriver.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 2)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Employee_ID, Drivers_License");
                        return;
                    }

                    // Trim the values and parse them
                    string employee_ID = inputValues[0].Trim();
                    string drivers_license = inputValues[1].Trim();
                 

                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Driver (Employee_ID, Drivers_License) VALUES (@Employee_ID, @Drivers_License)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Employee_ID", employee_ID);
                    command.Parameters.AddWithValue("@Drivers_License", drivers_license);
                   

                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void InsertTxtDriver_TextChanged(object sender, EventArgs e)
        {

        }

       /* private void UpdateButtonDriver_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the full update statement from the TextBox
                    string update = UpdateTxtDriver.Text.Trim();
                    // string update = "UPDATE Donor SET Address = 'Test Address' WHERE Donor_ID = 12";
                    //used a hardcoded input string since there are some issues with updating the strong 
                    //issue turned out to be due to accidentally putting the button name instead of textbox name

                    // Example input: UPDATE Donor SET Blood_Type = 'B+', DOB = '1990-05-15', Address = '123 New Street, Springfield' WHERE Donor_ID = 12;

                    // Ensure the input is not empty
                    if (string.IsNullOrWhiteSpace(update))
                    {
                        MessageBox.Show("Please enter a valid SQL UPDATE statement.");
                        return;
                    }

                    // Create the SQL command using the input statement
                    SqlCommand command = new SqlCommand(update, connection);

                    // Execute the UPDATE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found or nothing was updated.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void UpdateTxtDriver_TextChanged(object sender, EventArgs e)
        {

        }



        Decided to block out this because previous version has it where each tab has its own but that is redunant 
       as update has the same code throughout sinve it follows the C# Sql command 
       */
        private void DeleteButtonDriver_Click(object sender, EventArgs e)
        {
          
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxDriver.SelectedItem.ToString();
                    string value = DeleteTxtDriver.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Employee ID")
                    {
                        deleteQuery = "DELETE FROM Driver WHERE Employee_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Driver's License")
                    {
                        deleteQuery = "DELETE FROM Driver WHERE Drivers_License = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteTxtDriver_TextChanged(object sender, EventArgs e)
        {

        }

        private void InsertButtonEmployee_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtEmployee.Text.Split(';');

                    // Check if we have all 3 required values
                    if (inputValues.Length != 3)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Employee_ID, Join_Date, Salary");
                        return;
                    }

                    // Trim the values and parse them
                    string employee_ID = inputValues[0].Trim();
                    string join_date = inputValues[1].Trim();
                    string salary = inputValues[2].Trim();


                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Employee (Employee_ID, Join_Date , Salary) VALUES (@Employee_ID, @Join_Date, @Salary)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Employee_ID", employee_ID);
                    command.Parameters.AddWithValue("@Join_Date", join_date);
                    command.Parameters.AddWithValue("@Salary", salary);

                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }

        private void InsertTxtEmployee_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void DeleteButtonEmployee_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxEmployee.SelectedItem.ToString();
                    string value = DeleteTxtEmployee.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Employee ID")
                    {
                        deleteQuery = "DELETE FROM Employee WHERE Employee_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Join Date")
                    {
                        deleteQuery = "DELETE FROM Employee WHERE Join_Date = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else if (criterion == "Salary")
                    {
                        deleteQuery = "DELETE FROM Employee WHERE Salary = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteTxtEmployee_TextChanged(object sender, EventArgs e)
        {

        }

        private void InsertButtonHarvests_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtHarvest.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 2)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Employee_ID, Donor_ID");
                        return;
                    }

                    // Trim the values and parse them
                    string employee_ID = inputValues[0].Trim();
                    string donor_ID = inputValues[1].Trim();


                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Harvests (Employee_ID, Donor_ID) VALUES (@Employee_ID, @Donor_ID)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Employee_ID", employee_ID);
                    command.Parameters.AddWithValue("@Donor_ID", donor_ID);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }

        private void comboBoxEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DeleteButtonHarvests_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxHarvests.SelectedItem.ToString();
                    string value = DeleteTxtHarvests.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Employee ID")
                    {
                        deleteQuery = "DELETE FROM Harvests WHERE Employee_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Donor ID")
                    {
                        deleteQuery = "DELETE FROM Harvests WHERE Donor_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void InsertButtonManager_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtManager.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 2)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Employee_ID, Location");
                        return;
                    }

                    // Trim the values and parse them
                    string employee_ID = inputValues[0].Trim();
                    string location = inputValues[1].Trim();


                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Manager (Employee_ID, Location) VALUES (@Employee_ID, @Location)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Employee_ID", employee_ID);
                    command.Parameters.AddWithValue("@Location", location);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteButtonManager_Click(object sender, EventArgs e)
        {
     
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxManager.SelectedItem.ToString();
                    string value = DeleteTxtManager.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Employee ID")
                    {
                        deleteQuery = "DELETE FROM Manager WHERE Employee_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Location")
                    {
                        deleteQuery = "DELETE FROM Manager WHERE Location = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void InsertButtonPhleb_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtPhleb.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 2)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Employee_ID, License_ID");
                        return;
                    }

                    // Trim the values and parse them
                    string employee_ID = inputValues[0].Trim();
                    string license_ID = inputValues[1].Trim();


                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Phlebotomist (Employee_ID, License_ID) VALUES (@Employee_ID, @License_ID)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Employee_ID", employee_ID);
                    command.Parameters.AddWithValue("@License_ID", license_ID);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteButtonPhleb_Click(object sender, EventArgs e)
        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxPhleb.SelectedItem.ToString();
                    string value = DeleteTxtPhleb.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Employee ID")
                    {
                        deleteQuery = "DELETE FROM Phlebotomist WHERE Employee_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "License_ID")
                    {
                        deleteQuery = "DELETE FROM Phlebotomist WHERE License_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }

        private void InsertButtonTransfusion_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtTransfusion.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 5)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Donor_ID, Date, Temperature, WBC, Hemoglobin");
                        return;
                    }
                    //Donor_ID, Date, Temperature, WBC, Hemoglobin
                    // Trim the values and parse them
                    string donor_ID = inputValues[0].Trim();
                    DateTime date;
                    string temperature = inputValues[2].Trim();
                    string wbc = inputValues[3].Trim();
                    string hemoglobin = inputValues[4].Trim();

                    // Validate and parse the birthdate
                    if (!DateTime.TryParse(inputValues[1].Trim(), out date))
                    {
                        MessageBox.Show("Invalid birthdate format. Please enter a valid date.");
                        return;
                    }
                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Transfusion (Donor_ID, Date, Temperature, WBC, Hemoglobin) VALUES (@Donor_ID, @Date, @Temperature, @WBC, @Hemoglobin)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Donor_ID", donor_ID);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Temperature", temperature);
                    command.Parameters.AddWithValue("@WBC", wbc);
                    command.Parameters.AddWithValue("@Hemoglobin", hemoglobin);



                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        }

        private void DeleteButtonTransfusion_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxTransfusion.SelectedItem.ToString();
                    string value = DeleteTxtTransfusion.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Donor ID")
                    {
                        deleteQuery = "DELETE FROM Transfusion WHERE Donor_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Date")
                    {
                        deleteQuery = "DELETE FROM Transfusion WHERE Date = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else if (criterion == "Temperature")
                    {
                        deleteQuery = "DELETE FROM Transfusion WHERE Temperature = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else if (criterion == "WBC")
                    {
                        deleteQuery = "DELETE FROM Transfusion WHERE WBC = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else if (criterion == "Hemoglobin")
                    {
                        deleteQuery = "DELETE FROM Transfusion WHERE Hemoglobin = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void InsertButtonTruck_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Split the input using a symbol that you won't use in the text
                    string[] inputValues = InsertTxtTruck.Text.Split(';');

                    // Check if we have all four required values
                    if (inputValues.Length != 2)
                    {
                        // Debugging: Show the number of values parsed and the content
                        MessageBox.Show($"Parsed {inputValues.Length} values. Please enter all values in the format: Truck_ID, Inspection_Date");
                        return;
                    }

                    // Trim the values and parse them
                    string truck_ID = inputValues[0].Trim();
                    DateTime inspection_Date;
                

                    // Validate and parse the inspection date
                    if (!DateTime.TryParse(inputValues[1].Trim(), out inspection_Date))
                    {
                        MessageBox.Show("Invalid Inspection Date format. Please enter a valid date.");
                        return;
                    }


                    // Create the INSERT SQL query
                    string insertQuery = "INSERT INTO Truck (Truck_ID, Inspection_Date) VALUES (@Truck_ID, @Inspection_Date)";

                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Truck_ID", truck_ID);
                    command.Parameters.AddWithValue("@Inspection_Date", inspection_Date);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteButtonTruck_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get the selected criterion and value from the user
                    string criterion = comboBoxTruck.SelectedItem.ToString();
                    string value = DeleteTxtTruck.Text;

                    // Build the DELETE query based on the selected criterion
                    string deleteQuery;
                    SqlCommand command;

                    if (criterion == "Truck ID")
                    {
                        deleteQuery = "DELETE FROM Truck WHERE Truck_ID = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", int.Parse(value));
                    }
                    else if (criterion == "Inspection Date")
                    {
                        deleteQuery = "DELETE FROM Truck WHERE Inspection_Date = @Value";
                        command = new SqlCommand(deleteQuery, connection);
                        command.Parameters.AddWithValue("@Value", value);
                    }
                    else
                    {
                        MessageBox.Show("Invalid criterion selected.");
                        return;
                    }

                    // Execute the DELETE command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified criterion.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
