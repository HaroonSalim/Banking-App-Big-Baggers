using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace My_application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string Email = Console.ReadLine();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string Password = Console.ReadLine();
        }


        //--------------Button for registration-------//
        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Registration successful!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login successful!");
        }
    }
}