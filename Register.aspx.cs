﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection cn = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = final; User ID = sa; Password = sa123");
        cn.Open();
    
        string skills = string.Join(",", CheckBoxList1.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value));

        SqlCommand cmd = new SqlCommand("insert into [User] values(@id,@username,@email,@password,@dob,@skills,@city,@gender)", cn);
        cmd.Parameters.AddWithValue("@id", int.Parse(TextBox1.Text));
        cmd.Parameters.AddWithValue("@username", TextBox2.Text);
        cmd.Parameters.AddWithValue("@email", TextBox3.Text);
        cmd.Parameters.AddWithValue("@password", TextBox4.Text);
        cmd.Parameters.AddWithValue("@dob", TextBox5.Text);
        cmd.Parameters.AddWithValue("@skills", skills);
        cmd.Parameters.AddWithValue("@city", DropDownList1.SelectedValue);
        cmd.Parameters.AddWithValue("gender", RadioButtonList1.SelectedValue);
        cmd.ExecuteNonQuery();
        cn.Close();
        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert(`data inserted successully`)", true);


    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        SqlConnection cn = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = final; User ID = sa; Password = sa123");
        cn.Open();
        SqlCommand cmd = new SqlCommand("select * from [User]", cn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);

        GridView1.DataSource = dt;
        GridView1.DataBind();
        cn.Close();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        SqlConnection cn = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = final; User ID = sa; Password = sa123");
        cn.Open();
        SqlCommand cmd = new SqlCommand("delete [User] where id=@id",cn);
        cmd.Parameters.AddWithValue("@id", int.Parse(TextBox1.Text));
        cmd.ExecuteNonQuery();
        cn.Close();

        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert(`data deleted succesfully`)", true);
    }



    protected void Button2_Click(object sender, EventArgs e)
    {
        SqlConnection cn = new SqlConnection("Data Source =.\\SQLEXPRESS; Initial Catalog = final; User ID = sa; Password = sa123");
        cn.Open();
        string skills = string.Join(",", CheckBoxList1.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value));
       
        SqlCommand cmd = new SqlCommand("update [User]  set username=@username,email=@email,password=@password,dob=@dob,skills=@skills,city=@city,gender=@gender where id=@id", cn);
        cmd.Parameters.AddWithValue("@id", int.Parse(TextBox1.Text));
        cmd.Parameters.AddWithValue("@username", TextBox2.Text);
        cmd.Parameters.AddWithValue("@email", TextBox3.Text);
        cmd.Parameters.AddWithValue("@password", TextBox4.Text);
        cmd.Parameters.AddWithValue("@dob", TextBox5.Text);
        cmd.Parameters.AddWithValue("@skills", skills);
        cmd.Parameters.AddWithValue("@city", DropDownList1.SelectedValue);
        cmd.Parameters.AddWithValue("gender", RadioButtonList1.SelectedValue);
        cmd.ExecuteNonQuery();
        cn.Close();
        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert(`data updated successully`)", true);


    }

    protected void Button5_Click(object sender, EventArgs e)
    {
     
        // Get the ID from the TextBox
        int id = int.Parse(TextBox1.Text);
      

        // Define the connection string
        string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=final; User ID=sa; Password=sa123";

        // Create a connection and command
        using (SqlConnection cn = new SqlConnection(connectionString))
        {
            cn.Open();

            // Define the SQL command to get user data by ID
            SqlCommand cmd = new SqlCommand("SELECT username, email, password, dob, skills, city, gender FROM [User] WHERE id=@id", cn);
            cmd.Parameters.AddWithValue("@id", id);

            // Execute the command and read the data
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // If data is found, populate the TextBoxes with the data
                TextBox2.Text = reader["username"].ToString();
                TextBox3.Text = reader["email"].ToString();
                TextBox4.Text = reader["password"].ToString();
                TextBox5.Text = reader["dob"].ToString();
                // Assuming skills is a comma-separated string
                // (you may need to adjust how you handle this)
                string skills = reader["skills"].ToString();
                CheckBoxList1.Items.Cast<ListItem>().ToList().ForEach(li => li.Selected = skills.Split(',').Contains(li.Value));
                DropDownList1.SelectedValue = reader["city"].ToString();
                RadioButtonList1.SelectedValue = reader["gender"].ToString();
            }
            else
            {
                Console.WriteLine("No data found for the specified ID.");
            }

            reader.Close();
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            try
            {
                // 1. Get the file information
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string fileExtension = Path.GetExtension(fileName);
                string filePath = "~/Uploads/" + fileName;  // Server directory to save the file

                // 2. Validate file size, type, etc. (optional)

                // 3. Save the file to the server
                FileUpload1.SaveAs(Server.MapPath(filePath));

                // 4. Insert file data into the database
                string connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=final; User ID=sa; Password=sa123";
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    // SQL query to insert file details into the database
                    string query = "INSERT INTO UploadedFiles (FileName, FilePath) VALUES (@FileName, @FilePath)";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@FileName", fileName);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "File uploaded successfully!";
                        }
                        else
                        {
                            lblMessage.Text = "Error uploading file to the database.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }
        else
        {
            lblMessage.Text = "Please select a file to upload.";
        }
    
}
}
