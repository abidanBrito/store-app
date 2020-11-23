using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreApp
{
    public partial class AddProductForm : Form
    {
        public AddProductForm()
        {
            // Lock form size
            MinimumSize = new Size(563, 534);
            MaximumSize = new Size(563, 534);

            InitializeComponent();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (!isFormFilled())
            {
                MessageBox.Show("Please, fill in everything!", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product newProduct = new Product();

            newProduct.id = int.Parse(txtAddId.Text);
            newProduct.name= txtAddName.Text;
            newProduct.manufacturer = txtAddManufacturer.Text;
            newProduct.description = txtAddDescription.Text;
            newProduct.price = float.Parse(txtAddPrice.Text);
            newProduct.quantity = int.Parse(txtAddQuantity.Text);

            MainForm mainWindow = (MainForm) this.Owner;
            mainWindow.addNewProduct(newProduct, sender, e);

            this.Close();
        }
        private bool isFormFilled()
        {
            if (txtAddId.Text == "" || txtAddName.Text == "" || 
                txtAddManufacturer.Text == "" || txtAddDescription.Text == "" || 
                txtAddPrice.Text == "" || txtAddQuantity.Text == "")
            {
                return false;
            }

            return true;
        }
    }
}
