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

namespace NotSistem
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=EMIR\SQLEXPRESS;Initial Catalog=DbNotKayit;Integrated Security=True;TrustServerCertificate=True");


        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'dbNotKayitDataSet1.TBLDERS' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet1.TBLDERS);

        }

        private void ogrenciKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLDERS(OGRNUMARA, OGRAD, OGRSOYAD) values (@p1,@p2,@p3)",baglanti);
            komut.Parameters.AddWithValue("@p1", mskNo.Text);
            komut.Parameters.AddWithValue("@p2", mskAd.Text);
            komut.Parameters.AddWithValue("@p3", mskSoyad.Text);
            // sorguyu çalıştırmak anlamına geliyor.

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci sisteme eklendi.");
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet1.TBLDERS);

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            mskNo.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            mskAd.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            mskSoyad.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();

            txtSinav1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtSinav2.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            txtSinav3.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();

        }

        private void guncelleBtn_Click(object sender, EventArgs e)
        {
            double ortalama, s1, s2, s3;
            string durum;
            s1=Convert.ToDouble(txtSinav1.Text);
            s2 = Convert.ToDouble(txtSinav2.Text);
            s3 = Convert.ToDouble(txtSinav3.Text);

            ortalama=(s1+s2+s3)/3;
            lblOrtalama.Text = ortalama.ToString(); 

            if(ortalama>=50)
            {
                durum = "True";
            }
            else
            {
                durum = "False";
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("update TBLDERS set OGRS1=@P1,OGRS2=@P2,OGRS3=@P3,ORTALAMA=@P4,DURUM=@P5" +
                " WHERE OGRNUMARA=@P6",baglanti);

            komut.Parameters.AddWithValue("@P1", txtSinav1.Text);
            komut.Parameters.AddWithValue("@P2", txtSinav2.Text);
            komut.Parameters.AddWithValue("@P3", txtSinav3.Text);
            komut.Parameters.AddWithValue("@P4", decimal.Parse(lblOrtalama.Text));
            komut.Parameters.AddWithValue("@P5", durum);
            komut.Parameters.AddWithValue("@P6", mskNo.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Notları güncellendi.");

            baglanti.Open();
            SqlCommand gecen = new SqlCommand("SELECT COUNT (*) FROM TBLDERS WHERE DURUM='TRUE'", baglanti);
            SqlCommand kalan = new SqlCommand("SELECT COUNT (*) FROM TBLDERS WHERE DURUM='FALSE'", baglanti);
            int gecenSayisi = (int)gecen.ExecuteScalar();
            int kalanSayisi = (int)kalan.ExecuteScalar();
            lblGecenSayisi.Text = gecenSayisi.ToString();
            lblKalanSayisi.Text = kalanSayisi.ToString();


            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet1.TBLDERS);




        }
    }
}
