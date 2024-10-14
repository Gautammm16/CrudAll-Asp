using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

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

    //upload photo

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        SqlConnection cn = new SqlConnection("Data Source=.\\SQLEXPRESS; Initial Catalog=final; User ID=sa; Password=sa123");
        cn.Open();
        if (FileUpload1.HasFile)
        {
            string filename = Path.GetFileName(FileUpload1.FileName); // Get just the file name
            string path = Server.MapPath("~/img/" + filename); // Create the server path
            string relativePath = "~/img/" + filename;  // Store relative path
//            string absolutePath = Server.MapPath(relativePath);
            FileUpload1.SaveAs(path); // Save the file to the server

            SqlCommand cmd = new SqlCommand("insert into fileupload(filename,filepath) values(@filename,@filepath)", cn);
            cmd.Parameters.AddWithValue("@filename", filename);
            cmd.Parameters.AddWithValue("@filepath", relativePath);
            cmd.ExecuteNonQuery();
            cn.Close();
        }
       

    }


}

