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
    public partial class OrderForm : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbCon"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        DataRow dr;
        public OrderForm()
        {
            InitializeComponent();
            RefreshData();
            CustomerID();
        }

        public void ClearAllInfo()
        {
            txtOrderDate.Text = "";
            txtQuantity.Text = "";
            cmbCustomerID.SelectedValue = false;
        }

        public void RefreshData()
        {
            using (con = new SqlConnection(cs))
            {
                adapter = new SqlDataAdapter("Select * From Orders", con);
                dt = new DataTable();
                adapter.Fill(dt);
                dgViewOrderInformation.DataSource = dt;
            }
        }

        public void CustomerID()
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Select * From Customers", con);
                adapter = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adapter.Fill(dt);

                dr = dt.NewRow();
                dr.ItemArray = new object[] { 0, "--Select CustomersID--" };
                dt.Rows.InsertAt(dr, 0);

                cmbCustomerID.ValueMember = "CustomersID";
                cmbCustomerID.DisplayMember = "CustomersID";

                cmbCustomerID.DataSource = dt;
                con.Close();

            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Insert Into Orders(OrderDate,Quantity,CustomersID) VALUES(@orderdate, @quantity, @customerid)", con);
                cmd.Parameters.AddWithValue("@orderdate", txtOrderDate.Text);
                cmd.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                cmd.Parameters.AddWithValue("@customerid", cmbCustomerID.SelectedValue);
                MessageBox.Show("Data Insert Successfully");
                cmd.ExecuteNonQuery();
                con.Close();
                RefreshData();
                ClearAllInfo();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Update Orders Set OrderDate=@orderdate, Quantity=@quantity, CustomersID=@customerid Where OrdersID=@orderid", con);
                cmd.Parameters.AddWithValue("@orderid", lblOID.Text);

                cmd.Parameters.AddWithValue("@orderdate", txtOrderDate.Text);
                cmd.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                cmd.Parameters.AddWithValue("@customerid", cmbCustomerID.Text);
                MessageBox.Show("Data Updated Successfully");
                cmd.ExecuteNonQuery();
                con.Close();
                ClearAllInfo();
                RefreshData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (con = new SqlConnection(cs))
            {
                con.Open();
                cmd = new SqlCommand("Delete Orders Where OrdersID=@orderid", con);
                cmd.Parameters.AddWithValue("@orderid", lblOID.Text);
                MessageBox.Show("Data Delete Successfully");
                cmd.ExecuteNonQuery();
                con.Close();
                ClearAllInfo();
                RefreshData();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgViewOrderInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtOrderDate.Text = this.dgViewOrderInformation.CurrentRow.Cells["OrderDate"].Value.ToString();
            txtQuantity.Text = this.dgViewOrderInformation.CurrentRow.Cells["Quantity"].Value.ToString();
            cmbCustomerID.Text = this.dgViewOrderInformation.CurrentRow.Cells["CustomersID"].Value.ToString();
            lblOID.Text = this.dgViewOrderInformation.CurrentRow.Cells["OrdersID"].Value.ToString();
        }
    }
}
