using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Recognition1
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage;
        private Image<Gray, byte> cannyImage;
        private Image<Gray, byte> grayImage;
        private Image<Bgr, byte> cellShadingImage;
        private Image<Gray, byte> thresholdingImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void CelShadingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeCellShading();
            imageBox2.Image = cellShadingImage.Resize(300, 300, Inter.Linear);
        }

        private void ChooseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);
                grayImage = sourceImage.Convert<Gray, byte>();

                imageBox1.Image = sourceImage.Resize(300, 300, Inter.Linear);
                
            }
        }

        public void MakeCannyImage(double thresh = 200.0, double threshLink = 50.0)
        {

            cannyImage = grayImage.Canny(thresh, threshLink);
            imageBox2.Image = cannyImage.Resize(300, 300, Inter.Linear);
        }

        private void MakeCellShading()
        {
            MakeCannyImage();
            var cannyEdgesBgr = cannyImage.Convert<Bgr, byte>();
            cellShadingImage = sourceImage.Sub(cannyEdgesBgr); // попиксельное вычитание
                                                              //обход по каналам
            for (int channel = 0; channel < cellShadingImage.NumberOfChannels; channel++)
                for (int x = 0; x < cellShadingImage.Width; x++)
                    for (int y = 0; y < cellShadingImage.Height; y++) // обход по пискелям
                    {
                        // получение цвета пикселя
                        byte color = cellShadingImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 180;
                        else if (color <= 200)
                            color = 210;
                        else
                            color = 255;
                        cellShadingImage.Data[y, x, channel] = color; // изменение цвета пикселя
                    }

        }

        private void CannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeCannyImage();
            
        }

        public void MakeThresholdingImage(double threshold = 50.0, double maxValue = 255.0)
        {
            thresholdingImage = new Image<Gray, byte>(grayImage.Width, grayImage.Height, new Gray(0));

            CvInvoke.Threshold(grayImage, thresholdingImage, threshold, maxValue, ThresholdType.Binary);
            imageBox2.Image = thresholdingImage.Resize(300, 300, Inter.Linear);
        }

        private void ThresholdingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeThresholdingImage();
        }

        private void CannyControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CannyController cannyController = new Recognition1.CannyController(this);
            cannyController.StartPosition = FormStartPosition.CenterParent;
            cannyController.Show();
        }

        private void ThresholdingControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThresholdingController thresholdingController = new ThresholdingController(this);
            thresholdingController.StartPosition = FormStartPosition.CenterParent;
            thresholdingController.Show();
        }
    }
}
