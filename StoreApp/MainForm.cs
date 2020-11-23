using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace StoreApp
{
    public partial class MainForm : Form
    {
        Products myProducts;
        string filePath;

        public MainForm()
        {
            // Lock form size
            MinimumSize = new Size(837, 482);
            MaximumSize = new Size(837, 482);
            
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string startupPath = Directory.GetCurrentDirectory() + ".\\data";
            
            OpenFileDialog productsFileDialog = new OpenFileDialog();
            productsFileDialog.InitialDirectory = startupPath;

            DialogResult result = productsFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath = productsFileDialog.FileName;
                txtProductsFilePath.Text = filePath;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(isCatalogue())
            {
                StringBuilder sb = new StringBuilder();
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("iso-8859-1")))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sb.AppendLine(line);
                        }
                    }
                }

                if (lstCatalogue.Items.Count != 0)
                {
                    lstCatalogue.Items.Clear();
                }

                myProducts = JsonConvert.DeserializeObject<Products>(sb.ToString());
                foreach (Product p in myProducts.products)
                {
                    lstCatalogue.Items.Add(p.name);
                }
            }
        }

        private void lstCatalogue_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateProductDetails();
        }

        private void updateProductDetails()
        {
            Product selectedProduct = getCurrentProduct();
            txtProductDetails.Text = buildProductString(selectedProduct);
        }

        private string buildProductString(Product product) 
        {       
            StringBuilder productDetails = new StringBuilder();

            productDetails.Append("ID: " + product.id + Environment.NewLine);
            productDetails.Append("Name: " + product.name + Environment.NewLine);
            productDetails.Append("Manufacturer: " + product.manufacturer + Environment.NewLine);
            productDetails.Append("Description: " + product.description + Environment.NewLine);
            productDetails.Append("Price: " + product.price + " $" + Environment.NewLine);
            productDetails.Append("In stock: " + product.quantity + " units" + Environment.NewLine);

            return productDetails.ToString();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportForm export = new ExportForm();
            export.ShowDialog(this);
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog(this);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (!isCatalogue() || isCatalogueEmpty() || !isItemSelected())
            {
                return;
            }

            Product selectedProduct = getCurrentProduct();
            myProducts.products.Remove(selectedProduct);
            writeJson();

            btnLoad_Click(sender, e);
            txtProductDetails.Clear();
        }

        private void btnApplyDiscount(object sender, EventArgs e)
        {
            if (!isCatalogue() || isCatalogueEmpty())
            {
                return;
            }

            if (txtDiscount.Text.Equals(""))
            {
                MessageBox.Show("Specify a discount value, please.", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Misc.validateDiscount(txtDiscount.Text))
            {
                int selectedItemIndex = lstCatalogue.SelectedIndex;

                int discount = int.Parse(txtDiscount.Text);
                foreach (Product p in myProducts.products)
                {
                    p.price -= p.price * discount / 100;
                    p.price = (float) Math.Round(p.price, 2);
                }

                writeJson();
                btnLoad_Click(sender, e);

                if (selectedItemIndex >= 0)
                {
                    lstCatalogue.SelectedIndex = selectedItemIndex;
                }

                return;
            }

            MessageBox.Show("Enter an integer number between 0 and 100, please." +
                            Environment.NewLine + "Note that 0 and 100 are excluded from the range.",
                            "INVALID INPUT!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool isCatalogue()
        {
            if (filePath == null)
            {
                MessageBox.Show("No catalogue was selected!", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private bool isCatalogueEmpty()
        {
            if (myProducts == null)
            {
                MessageBox.Show("Either the catalogue is empty or it has not been loaded!", "WARNING",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }

            return false;
        }

        private bool isItemSelected()
        {
            if (lstCatalogue.SelectedIndex < 0)
            {
                MessageBox.Show("No item selected!", "WARNING", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!isCatalogue() || isCatalogueEmpty() || !isItemSelected())
            {
                return;
            }

            Product selectedProduct = getCurrentProduct();
            int selectedItemIndex = lstCatalogue.SelectedIndex;

            EditProductForm editProduct = new EditProductForm(selectedProduct);
            editProduct.ShowDialog(this);

            lstCatalogue.SelectedIndex = selectedItemIndex;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!isCatalogue())
            {
                return;
            }

            AddProductForm addProduct = new AddProductForm();
            addProduct.ShowDialog(this);
        }

        public void addNewProduct(Product product, object sender, EventArgs e)
        {
            myProducts.products.Add(product);
            writeJson();

            btnLoad_Click(sender, e);
        }
        public void editSelectedProduct(Product product, object sender, EventArgs e)
        {
            int productIndex = lstCatalogue.SelectedIndex;

            myProducts.products.ElementAt(productIndex).id = product.id;
            myProducts.products.ElementAt(productIndex).name = product.name;
            myProducts.products.ElementAt(productIndex).manufacturer = product.manufacturer;
            myProducts.products.ElementAt(productIndex).description = product.description;
            myProducts.products.ElementAt(productIndex).price = product.price;
            myProducts.products.ElementAt(productIndex).quantity = product.quantity;

            writeJson();
            btnLoad_Click(sender, e);
        }

        private Product getCurrentProduct()
        {
            int selection = lstCatalogue.SelectedIndex;
            if (selection >= 0)
            {
                return myProducts.products.ElementAt(selection);
            }

            return null;
        }

        private void writeJson()
        {
            string jsonData = JsonConvert.SerializeObject(myProducts);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
