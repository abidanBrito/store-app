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
    public partial class EditProductForm : Form
    {
        public EditProductForm(Product product)
        {
            // Lock form size
            MinimumSize = new Size(563, 534);
            MaximumSize = new Size(563, 534);

            InitializeComponent();

            txtEditId.Text = product.id.ToString();
            txtEditName.Text = product.name;
            txtEditManufacturer.Text = product.manufacturer;
            txtEditDescription.Text = product.description;
            txtEditPrice.Text = product.price.ToString();
            txtEditQuantity.Text = product.quantity.ToString();
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            Product editedProduct = new Product();

            editedProduct.id = int.Parse(txtEditId.Text);
            editedProduct.name = txtEditName.Text;
            editedProduct.manufacturer = txtEditManufacturer.Text;
            editedProduct.description = txtEditDescription.Text;
            editedProduct.price = float.Parse(txtEditPrice.Text);
            editedProduct.quantity = int.Parse(txtEditQuantity.Text);

            MainForm mainWindow = (MainForm)this.Owner;
            mainWindow.editSelectedProduct(editedProduct, sender, e);

            this.Close();
        }
    }
}
