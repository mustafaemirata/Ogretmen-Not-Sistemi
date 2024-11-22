using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NotSistem
{
    public partial class FrmOgrenciDetay : Form
    {
        public FrmOgrenciDetay()
        {
            InitializeComponent();
        }

        public string numara;

        // Veritabanı bağlantı dizesi
        SqlConnection baglanti = new SqlConnection(@"Data Source=EMIR\SQLEXPRESS;Initial Catalog=DbNotKayit;Integrated Security=True;TrustServerCertificate=True");

        private void FrmOgrenciDetay_Load(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci numarasını etikete yazdır
                lblNumara.Text = numara;

                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Öğrenci bilgilerini getiren sorguyu oluştur
                SqlCommand komut = new SqlCommand("SELECT * FROM TBLDERS WHERE OGRNUMARA=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", numara);

                SqlDataReader dr = komut.ExecuteReader();

                // Veritabanından gelen sonuçları oku
                if (dr.Read())
                {
                    lblIsim.Text = dr[2].ToString() + " " + dr[3].ToString(); // Ad Soyad
                    lblSinav1.Text = dr[4].ToString(); // Sınav 1
                    lblSinav2.Text = dr[5].ToString(); // Sınav 2
                    lblbSinav3.Text = dr[6].ToString(); // Sınav 3
                    lblbOrtalama.Text = dr[7].ToString(); // Ortalama

                    // Ortalama durumuna göre sonucu belirle
                    int ortalama;
                    if (int.TryParse(lblbOrtalama.Text, out ortalama))
                    {
                        lblDurum.Text = ortalama >= 50 ? "Geçti!" : "Kaldı!";
                    }
                    else
                    {
                        lblDurum.Text = "Geçersiz Ortalama!";
                    }
                }
                else
                {
                    MessageBox.Show("Öğrenci bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Kaynakları serbest bırak
                dr.Close();
            }
            catch (Exception ex)
            {
                // Hata mesajı göster
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Bağlantıyı her durumda kapat
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }
        }
    }
}
