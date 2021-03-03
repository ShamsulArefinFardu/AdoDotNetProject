using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Configuration;

namespace AdoDotNetProject_Arefin
{
    public partial class CustomerForm : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        public CustomerForm()
        {
            InitializeComponent();
        }
        public void ClearAllData()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtImageUrl.Text = "";
            picCustomer.Image = null;

        }

        public void RefreshData()
        {
            using (con = new SqlConnection(cs))
            {
                adapter = new SqlDataAdapter("Select * From Customers", con);
                dt = new DataTable();
                adapter.Fill(dt);
                dgViewCustomerInformation.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Insert Into Customers(Name, Address, CustomersImage) VALUES(@name, @address, @image)", con);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@image", txtImageUrl.Text);
                MessageBox.Show("Data Inserted Successfully");
                cmd.ExecuteNonQuery();
                con.Close();
                RefreshData();
                ClearAllData();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Update Customers Set Name=@name, Address=@address,CustomersImage=@image Where CustomersID=@customerid", con);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@image", txtImageUrl.Text);
                cmd.Parameters.AddWithValue("@customerid", lblCID.Text);

                MessageBox.Show("Data Updated Successfully");
                cmd.ExecuteNonQuery();
                con.Close();

                RefreshData();
                ClearAllData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Delete Customers Where CustomersID=@customerid", con);
                cmd.Parameters.AddWithValue("@customerid", lblCID.Text);
                MessageBox.Show("Data Delete Successfully");
                cmd.ExecuteNonQuery();
                con.Close();
                RefreshData();
                ClearAllData();
            }
        }

        private void dgViewCustomerInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = this.dgViewCustomerInformation.CurrentRow.Cells["Name"].Value.ToString();
            txtAddress.Text = this.dgViewCustomerInformation.CurrentRow.Cells["Address"].Value.ToString();
            txtImageUrl.Text = this.dgViewCustomerInformation.CurrentRow.Cells["CustomersImage"].Value.ToString();

            lblCID.Text = this.dgViewCustomerInformation.CurrentRow.Cells["CustomersID"].Value.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*jpg; *jpeg; *gif; *bmp;)|*jpg; *jpeg; *gif; *bmp;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtImageUrl.Text = open.FileName;
                picCustomer.Image = new Bitmap(open.FileName);

            }
        }
    }
}
