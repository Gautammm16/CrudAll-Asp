using System;
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
}