using System.Security.Cryptography;
using System.Text;
using System.Windows;


namespace sha1_Hashing;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void hashBtn_Click(object sender, RoutedEventArgs e)
    {
        saltTxtb.Text = Guid.NewGuid().ToString("N");
        outputTxtb.Text = SHA1(SHA1(inputTxtb.Text+saltTxtb.Text));
    }

    private string SHA1(string input)
    {
        byte[] hash;
        using(var sha1 = new SHA1CryptoServiceProvider())
        {
            hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
        }

        var sb = new StringBuilder();
        foreach(byte b in hash)
        {
            sb.AppendFormat("{0:x2}",b);
        }

        return sb.ToString();
    }
}