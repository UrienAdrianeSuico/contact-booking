using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace contact_booking
{
    public partial class Form1 : Form
    {
        string filePath = @"D:\contactDB.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtContactNum.Text != "")
            {
                string newEntry = $"{txtName.Text} {txtContactNum.Text}";

                if (IsDuplicateEntry(newEntry))
                {
                    MessageBox.Show("Error, duplicate number not allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(newEntry);
                    }
                    MessageBox.Show("Contact saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving the contact: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    clearInputs();
                }
            }
            else
            {
                MessageBox.Show("Inputs can't be Empty", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsDuplicateEntry(string newEntry)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Equals(newEntry, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void Search_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtName.Text;
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(' ');
                            if (parts.Length == 2 && parts[0].Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
                            {
                                txtName.Text = parts[0];
                                txtContactNum.Text = parts[1];
                                Savebtn.Enabled = true;
                                return;
                            }
                            else if (parts.Length == 3 && searchTerm.Equals($"{parts[0]} {parts[1]}"))
                            {
                                txtName.Text = $"{parts[0]} {parts[1]}";
                                txtContactNum.Text = parts[2];
                                return;
                            }
                            else if (parts.Length == 4 && searchTerm.Equals($"{parts[0]} {parts[1]} {parts[2]}"))
                            {
                                txtName.Text = $"{parts[0]} {parts[1]} {parts[2]}";
                                txtContactNum.Text = parts[3];
                                return;
                            }
                        }
                    }
                    MessageBox.Show("Contact not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No contacts found.", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching for the contact: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void clearInputs()
        {
            txtName.Text = string.Empty;
            txtContactNum.Text = string.Empty;
        }
    }
    
    
}
