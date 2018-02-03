using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeavesDetection
{
    public partial class Leaves : Form
    {
        public Leaves()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InputImage_DoubleClick(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> colordImage = (Image<Bgr, Byte>)InputImage.Image;
            Image<Bgr, Byte> temp = colordImage.Copy();
            temp._EqualizeHist();
            temp._SmoothGaussian(9);

            Image<Gray, Byte> grayImage = temp.InRange(new Bgr(0, 40, 0), new Bgr(135, 256, 135));
            grayImage = grayImage.Erode(4);
            grayImage = grayImage.Dilate(4);

            Image<Gray, Byte> grayImage2 = grayImage.Copy();

            using (var contours = new VectorOfVectorOfPoint())
            {

                CvInvoke.FindContours(grayImage2, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

                for (int i = 0; i < contours.Size; i++)
                {
                    if (CvInvoke.ContourArea(contours[i], false) < 50000)
                    {
                        CvInvoke.DrawContours(grayImage, contours, i, new MCvScalar(), -1);
                    }
                }

            }

            grayImage = grayImage.Erode(4);
            grayImage = grayImage.Dilate(4);

            grayImage2 = grayImage.Copy();

            using (var contours = new VectorOfVectorOfPoint())
            {

                CvInvoke.FindContours(grayImage2, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

                double compactness = Math.Pow(CvInvoke.ArcLength(contours[0], true), 2) / (CvInvoke.ContourArea(contours[0], false) * 4 * Math.PI);
                if (compactness < 2.5f)
                {
                    MCvMoments moments = CvInvoke.Moments(grayImage, true);

                    if (moments.M00 < 70000)
                    {
                        textBox1.Text = "Leaf is of type 3 ";
                    }
                    else
                    {
                        textBox1.Text = "Leaf is of type 2 ";
                    }
                }


                else
                {
                    textBox1.Text = "Leaf is of type 1 ";
                }

            }

            OutputImage.Image = colordImage.Copy(grayImage);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
