using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "图片文件(*.jpg)|*.jpg";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label1.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image image = Image.FromFile(label1.Text);
            Bitmap bitmap = new Bitmap(image);
            List<Color> RowColors = new List<Color>();
            int[] result = new int[2000];
            int n = -1;
            for (int i = 0; i < image.Width; i += 20)
            {
                n++;
                for (int j = 0; j < image.Height; j += 20)
                {
                    var color = bitmap.GetPixel(i, j);
                    if (!RowColors.Contains(color, new ColorEquality()))
                    {
                        RowColors.Add(color);
                        result[n] += 1;
                    }

                }
                RowColors.Clear();
            }
            var txt = string.Join("", result).Trim('0', ' ');

            Console.WriteLine(HashMd5(txt));

        }
        //public bool find( Color color, int r, int g, int b)
        //{
        //    var n = trackBar4.Value;
        //    var flag = false;
        //    foreach (var item in RowColors)
        //    {
        //        if (color.ToArgb() > item.ToArgb() - n || color.ToArgb() < item.ToArgb() + n)
        //        {
        //            flag = true;
        //            break;
        //        }
        //        else {
        //            flag &= false;

        //        }
        //    }
        //    return flag;
        //}

        public class ColorEquality : IEqualityComparer<Color>
        {

            public bool Equals(Color x, Color y)
            {
                return x.ToArgb() - 1000 < y.ToArgb() && x.ToArgb() + 1000 > y.ToArgb();
            }

            public int GetHashCode(Color obj)
            {
                throw new NotImplementedException();
            }
        }

        public string HashMd5(string txt)
        {
            byte[] byt = System.Text.Encoding.Default.GetBytes(txt);
            MD5 mD5 = new MD5CryptoServiceProvider();
            var targetByt = mD5.ComputeHash(byt);
            return BitConverter.ToString(targetByt).Replace("-", "");
        }
    }
}
