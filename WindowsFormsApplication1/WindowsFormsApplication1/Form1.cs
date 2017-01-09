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
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();


        }
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private BackgroundWorker bw;

        int _LoadGame_time = 120000;
        int _Click_delay = 10000;
        int xx, yy;
        public void DoMouseClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public void DoMouseRightClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Thread.Sleep(500);

            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                   Screen.PrimaryScreen.Bounds.Height,
                                                   PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            //Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);
            //bmpScreenshot = new Bitmap(bmpScreenshot, new Size(800, 500));

            //Rectangle cropRect = new Rectangle(80, 200, 1250, 550);
            Bitmap src = Image.FromFile("Screenshot.png") as Bitmap;
            Bitmap target = new Bitmap(src.Width, src.Height);

            // using(Graphics g = Graphics.FromImage(target))
            //{
            //     g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            //} 

            target.Save("target.png", ImageFormat.Png);

            Image<Bgr, Byte> sc = new Image<Bgr, Byte>("target.png");

            imageBox1.Image = sc;
            sc.Dispose();
            src.Dispose();

            this.WindowState = FormWindowState.Normal;
        }

        #region clear_screen
        private void button2_Click(object sender, EventArgs e)
        {
            if (imageBox1.Image != null)
            {
                imageBox1.Image.Dispose();
                imageBox1.Image = null;
            }
            else
                MessageBox.Show("This box already empty");

        }
        #endregion
        #region take_a_sample
        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Thread.Sleep(500);

            var screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                                   Screen.PrimaryScreen.Bounds.Height,
                                                   PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(screenshot);


            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            screenshot.Save("sample.png", ImageFormat.Png);

            Rectangle cropRect = new Rectangle(80, 200, 1250, 550);
            Bitmap src = Image.FromFile("sample.png") as Bitmap;
            Bitmap sample = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(sample))
            {
                g.DrawImage(src, new Rectangle(0, 0, sample.Width, sample.Height), cropRect, GraphicsUnit.Pixel);
            }

            src.Dispose();
            sample.Save("sample.png", ImageFormat.Png);

            //Image<Bgr, Byte> spl = new Image<Bgr, Byte>("sample.png");

            this.WindowState = FormWindowState.Normal;
            MessageBox.Show("sample has been taken");
            //CvInvoke.Imshow("sample", spl);
            //CvInvoke.WaitKey(0);


        }
        #endregion
        private void button5_Click(object sender, EventArgs e)
        {
            //_LoadGame_time = Int32.Parse(LoadGame_time.Text) * 1000;
            //_Click_delay = Int32.Parse(Click_delay.Text) * 1000;
            this.WindowState = FormWindowState.Minimized;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void auto_click()
        {
            Random rd1 = new Random();
            Random rd2 = new Random();
            int xx = rd1.Next(1250, 1414);
            int yy = rd2.Next(700, 880);
            DoMouseRightClick(xx, yy);

            string path = @"C:\Users\BarryAllen\Documents\Visual Studio 2013\Projects\emgu\WindowsFormsApplication1\WindowsFormsApplication1\bin\Debug\SoTran\step.txt";
            string toado;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                StreamReader rd = new StreamReader(fs, Encoding.UTF8);
                toado = rd.ReadToEnd();
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(toado + "\r\n" + xx.ToString() + "-" + yy.ToString() + "\r\n");
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            #region code cũ
            
            /*
            //MessageBox.Show(_LoadGame_time.ToString());
            //MessageBox.Show(_Click_delay.ToString());
            int so_tran = 0;
            int count = 0;
            while (count == 0)
            {   
                
                int hang_cho = 0;
                while (hang_cho == 0)
                {
                    #region hàng chờ
                    #region chup_anh, lúc này đang ở hàng chờ game, count = 0
                    Thread.Sleep(500);
                    var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                    var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);

                    Rectangle cropRect = new Rectangle(80, 200, 1250, 550);
                    Bitmap src = Image.FromFile("Screenshot.png") as Bitmap;
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                    }

                    target.Save("target.png", ImageFormat.Png);

                    src.Dispose();
                    #endregion
                    #region kiểm tra xem đã có trận hay chưa
                    Image<Bgr, Byte> D = new Image<Bgr, Byte>("sample.png");
                    Image<Bgr, Byte> D1 = new Image<Bgr, Byte>("target.png");
                    Image<Bgr, Byte> Duyen = D - D1;
                    imageBox1.Image = Duyen;
                    Bitmap a = new Bitmap(Duyen.ToBitmap());
                    Color den = System.Drawing.ColorTranslator.FromHtml("#000000");


                    if (a.GetPixel(638, 399) != den)
                    {
                        DoMouseClick(625, 479);
                        Thread.Sleep(10000);
                        if (check_pick_tuong() == 1)
                        {
                            count = 1;
                            hang_cho = 1;
                        }
                    }
                }
                    #endregion
                    #endregion có trận, chuyển sang pick tướng, count = 1

                    #region pick tướng
                if (count == 1)
                {
                    DoMouseClick(406, 240); //ấn nút random;
                    Thread.Sleep(1000);
                    DoMouseClick(1000, 555); //ấn nút lock
                    count = 2;
                }
                #endregion chờ vào game, khi này count = 2

                    #region vào game
                if (count == 2)
                {
                    Thread.Sleep(_LoadGame_time);
                    DoMouseClick(500, 500);
                    #region kiểm tra đội xanh hay đỏ
                    //1270 883 mã (163,44,44) = #A32C2C
                    Color team_red = System.Drawing.ColorTranslator.FromHtml("#172634");
                    Color team_blue = System.Drawing.ColorTranslator.FromHtml("#2d2125");
                    var inGame_Screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                    var inGame_gfxScreenshot = Graphics.FromImage(inGame_Screenshot);
                    inGame_gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    inGame_Screenshot.Save("Screenshot.png", ImageFormat.Png);
                    inGame_Screenshot.Dispose();
                    Bitmap src_2 = Image.FromFile("Screenshot.png") as Bitmap;

                    if (src_2.GetPixel(1227, 881) == team_red)
                    {
                        //đội đỏ
                        //MessageBox.Show("đội đỏ");
                        xx = 1227;
                        yy = 881;
                        //MessageBox.Show(xx.ToString() + " " + yy.ToString());  
                        // textBox1.Text = "xác định là đội xanh";
                    }
                    if (src_2.GetPixel(1423, 685) == team_blue)
                    {
                        //MessageBox.Show("đội xanh");
                        xx = 1421;
                        yy = 691;
                        //MessageBox.Show(xx.ToString() + " " + yy.ToString());
                    }

                    #endregion

                    #region auto click
                    int m = 0;
                    while (m == 0)
                    {
                        #region click
                        DoMouseRightClick(xx, yy);
                        //auto_click();
                        #endregion
                        #region kiểm tra xem kết thúc game chưa
                        #region chụp màn hình
                        var check_endgame_Screenshot_1 = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                        var check_endgame_gfxScreenshot_1 = Graphics.FromImage(check_endgame_Screenshot_1);
                        check_endgame_gfxScreenshot_1.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                        check_endgame_Screenshot_1.Save("Screenshot_1.png", ImageFormat.Png);


                        #endregion
                        #region kiểm tra

                        Bitmap test = Image.FromFile("Screenshot_1.png") as Bitmap;
                        Color[,] pic = new Color[test.Width, test.Height];
                        for (int i = 680; i < 760; i++)
                        {
                            for (int j = 535; j < 560; j++)
                            {
                                pic[i, j] = test.GetPixel(i, j);
                            }
                        }

                        Bitmap b = new Bitmap(80, 25);
                        for (int i = 0; i < 80; i++)
                        {
                            for (int j = 0; j < 25; j++)
                            {
                                b.SetPixel(i, j, pic[i + 680, j + 535]);
                            }
                        }
                        b.Save("b.png", ImageFormat.Png);
                        Image<Bgr, Byte> b_1 = new Image<Bgr, Byte>("b.png");


                        Bitmap endgame = Image.FromFile("b.png") as Bitmap; //endgame là hình chụp màn hình đã được cắt nhỏ ở phần chữ "Tiếp tục"

                        Bitmap sample_endgame = Image.FromFile("endgame.png") as Bitmap;

                        int dem = 0;
                        for (int i = 0; i < 80; i++)
                        {
                            for (int j = 0; j < 25; j++)
                            {
                                if (endgame.GetPixel(i, j) == sample_endgame.GetPixel(i + 680, j + 535))
                                {
                                    dem += 1;
                                }
                            }
                        }

                        #region đọc file log ghi số trận
                        StreamReader sr = File.OpenText(@"SoTran\SoTran.txt");
                        so_tran = Int32.Parse(sr.ReadLine()) + 1;
                        sr.Dispose();
                        #endregion

                        if (dem > 1800)
                        {
                            DoMouseClick(720, 545);
                            //MessageBox.Show("Finish");
                            m = 1;
                            count = 0;
                            Thread.Sleep(10000);
                            take_screen_shot("Tran " + so_tran);
                            write_log_file(so_tran.ToString());
                            //so_tran += 1;
                        }
                        #endregion
                        #endregion

                        sample_endgame.Dispose();
                        check_endgame_Screenshot_1.Dispose();
                        test.Dispose();
                        endgame.Dispose();
                        //src.Dispose();
                        Thread.Sleep(_Click_delay); //20s
                    }
                    DoMouseClick(1080, 769);
                    //Thread.Sleep(5000);

                    #region click lại vào đánh máy
                    //DoMouseClick(705, 57);
                    //Thread.Sleep(500);
                    //DoMouseClick(410, 207);
                    //Thread.Sleep(500);
                    //DoMouseClick(607, 186);
                    //Thread.Sleep(500);
                    //DoMouseClick(802, 186);
                    //Thread.Sleep(500);
                    //DoMouseClick(1020, 237);
                    //Thread.Sleep(500);
                    //DoMouseClick(830, 740);
                    #endregion

                    src_2.Dispose();
                    #endregion
                }
                #endregion kết thúc game, count quay về 0 để đánh tiếp
            }
            */
            #endregion
            int so_tran = 0;
            //this.WindowState = FormWindowState.Minimized;
            int hang_cho = 0;
            while (hang_cho == 0)
            {
                if (check_co_tran() == true)
                {
                    DoMouseClick(625, 479);
                    Thread.Sleep(10000);
                    if (check_pick_tuong() == 1)
                    {
                        DoMouseClick(406, 240); //ấn nút random;
                        Thread.Sleep(1000);
                        DoMouseClick(1000, 555); //ấn nút lock
                        while (check_thanh_menu() == false)
                        {
                            if (check_vao_game() == true)
                            {
                                //MessageBox.Show("vào game rồi");
                                #region kiểm tra đội xanh hay đỏ
                                //1270 883 mã (163,44,44) = #A32C2C
                                Color team_red = System.Drawing.ColorTranslator.FromHtml("#172634");
                                Color team_blue = System.Drawing.ColorTranslator.FromHtml("#2d2125");
                                var inGame_Screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                                var inGame_gfxScreenshot = Graphics.FromImage(inGame_Screenshot);
                                inGame_gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                                inGame_Screenshot.Save("Screenshot.png", ImageFormat.Png);
                                inGame_Screenshot.Dispose();
                                Bitmap src_2 = Image.FromFile("Screenshot.png") as Bitmap;

                                if (src_2.GetPixel(1227, 881) == team_red)
                                {
                                    //đội đỏ
                                    //MessageBox.Show("đội đỏ");
                                    xx = 1227;
                                    yy = 881;
                                    //MessageBox.Show(xx.ToString() + " " + yy.ToString());  
                                    // textBox1.Text = "xác định là đội xanh";
                                }
                                if (src_2.GetPixel(1423, 685) == team_blue)
                                {
                                    //MessageBox.Show("đội xanh");
                                    xx = 1421;
                                    yy = 691;
                                    //MessageBox.Show(xx.ToString() + " " + yy.ToString());
                                }

                                #endregion
                                #region auto click
                                int m = 0;
                                while (m == 0)
                                {
                                    #region click
                                    DoMouseRightClick(xx, yy);
                                    //auto_click();
                                    #endregion
                                    #region kiểm tra xem kết thúc game chưa
                                    #region chụp màn hình
                                    var check_endgame_Screenshot_1 = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                                    var check_endgame_gfxScreenshot_1 = Graphics.FromImage(check_endgame_Screenshot_1);
                                    check_endgame_gfxScreenshot_1.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                                    check_endgame_Screenshot_1.Save("Screenshot_1.png", ImageFormat.Png);


                                    #endregion
                                    #region kiểm tra

                                    Bitmap test = Image.FromFile("Screenshot_1.png") as Bitmap;
                                    Color[,] pic = new Color[test.Width, test.Height];
                                    for (int i = 680; i < 760; i++)
                                    {
                                        for (int j = 535; j < 560; j++)
                                        {
                                            pic[i, j] = test.GetPixel(i, j);
                                        }
                                    }

                                    Bitmap b = new Bitmap(80, 25);
                                    for (int i = 0; i < 80; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            b.SetPixel(i, j, pic[i + 680, j + 535]);
                                        }
                                    }
                                    b.Save("b.png", ImageFormat.Png);
                                    Image<Bgr, Byte> b_1 = new Image<Bgr, Byte>("b.png");


                                    Bitmap endgame = Image.FromFile("b.png") as Bitmap; //endgame là hình chụp màn hình đã được cắt nhỏ ở phần chữ "Tiếp tục"

                                    Bitmap sample_endgame = Image.FromFile("endgame.png") as Bitmap;

                                    int dem = 0;
                                    for (int i = 0; i < 80; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            if (endgame.GetPixel(i, j) == sample_endgame.GetPixel(i + 680, j + 535))
                                            {
                                                dem += 1;
                                            }
                                        }
                                    }

                                    #region đọc file log ghi số trận
                                    StreamReader sr = File.OpenText(@"SoTran\SoTran.txt");
                                    so_tran = Int32.Parse(sr.ReadLine()) + 1;
                                    sr.Dispose();
                                    #endregion

                                    if (dem > 1800)
                                    {
                                        DoMouseClick(720, 545);
                                        //MessageBox.Show("Finish");
                                        m = 1;
                                        Thread.Sleep(10000);
                                        take_screen_shot("Tran " + so_tran);
                                        write_log_file(so_tran.ToString());
                                        //so_tran += 1;
                                    }
                                    #endregion
                                    #endregion

                                    sample_endgame.Dispose();
                                    check_endgame_Screenshot_1.Dispose();
                                    test.Dispose();
                                    endgame.Dispose();
                                    //src.Dispose();
                                    Thread.Sleep(20000); //20s
                                }
                                DoMouseClick(1080, 769);
                                //Thread.Sleep(5000);
                                #endregion
                                src_2.Dispose();
                            }
                        }
                        //MessageBox.Show("trở về hàng chờ nhé");
                    }
                }
            }

        }
        private bool button6WasClicked = false;
        private bool button7WasClicked = false;

        private void button6_Click(object sender, EventArgs e)
        {
            button6WasClicked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int so_tran = 0;
            //this.WindowState = FormWindowState.Minimized;
            int hang_cho = 0;
            while (hang_cho == 0)
            {
                if (check_co_tran() == true)
                {
                    DoMouseClick(625, 479);
                    Thread.Sleep(10000);
                    if (check_pick_tuong() == 1)
                    {
                        DoMouseClick(406, 240); //ấn nút random;
                        Thread.Sleep(1000);
                        DoMouseClick(1000, 555); //ấn nút lock
                        while (check_thanh_menu() == false)
                        {
                            if (check_vao_game() == true)
                            {
                                //MessageBox.Show("vào game rồi");
                                #region kiểm tra đội xanh hay đỏ
                                //1270 883 mã (163,44,44) = #A32C2C
                                Color team_red = System.Drawing.ColorTranslator.FromHtml("#172634");
                                Color team_blue = System.Drawing.ColorTranslator.FromHtml("#2d2125");
                                var inGame_Screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                                var inGame_gfxScreenshot = Graphics.FromImage(inGame_Screenshot);
                                inGame_gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                                inGame_Screenshot.Save("Screenshot.png", ImageFormat.Png);
                                inGame_Screenshot.Dispose();
                                Bitmap src_2 = Image.FromFile("Screenshot.png") as Bitmap;

                                if (src_2.GetPixel(1227, 881) == team_red)
                                {
                                    //đội đỏ
                                    //MessageBox.Show("đội đỏ");
                                    xx = 1227;
                                    yy = 881;
                                    //MessageBox.Show(xx.ToString() + " " + yy.ToString());  
                                    // textBox1.Text = "xác định là đội xanh";
                                }
                                if (src_2.GetPixel(1423, 685) == team_blue)
                                {
                                    //MessageBox.Show("đội xanh");
                                    xx = 1421;
                                    yy = 691;
                                    //MessageBox.Show(xx.ToString() + " " + yy.ToString());
                                }

                                #endregion
                                #region auto click
                                int m = 0;
                                while (m == 0)
                                {
                                    #region click
                                    DoMouseRightClick(xx, yy);
                                    //auto_click();
                                    #endregion
                                    #region kiểm tra xem kết thúc game chưa
                                    #region chụp màn hình
                                    var check_endgame_Screenshot_1 = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                                    var check_endgame_gfxScreenshot_1 = Graphics.FromImage(check_endgame_Screenshot_1);
                                    check_endgame_gfxScreenshot_1.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                                    check_endgame_Screenshot_1.Save("Screenshot_1.png", ImageFormat.Png);


                                    #endregion
                                    #region kiểm tra

                                    Bitmap test = Image.FromFile("Screenshot_1.png") as Bitmap;
                                    Color[,] pic = new Color[test.Width, test.Height];
                                    for (int i = 680; i < 760; i++)
                                    {
                                        for (int j = 535; j < 560; j++)
                                        {
                                            pic[i, j] = test.GetPixel(i, j);
                                        }
                                    }

                                    Bitmap b = new Bitmap(80, 25);
                                    for (int i = 0; i < 80; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            b.SetPixel(i, j, pic[i + 680, j + 535]);
                                        }
                                    }
                                    b.Save("b.png", ImageFormat.Png);
                                    Image<Bgr, Byte> b_1 = new Image<Bgr, Byte>("b.png");


                                    Bitmap endgame = Image.FromFile("b.png") as Bitmap; //endgame là hình chụp màn hình đã được cắt nhỏ ở phần chữ "Tiếp tục"

                                    Bitmap sample_endgame = Image.FromFile("endgame.png") as Bitmap;

                                    int dem = 0;
                                    for (int i = 0; i < 80; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            if (endgame.GetPixel(i, j) == sample_endgame.GetPixel(i + 680, j + 535))
                                            {
                                                dem += 1;
                                            }
                                        }
                                    }

                                    #region đọc file log ghi số trận
                                    StreamReader sr = File.OpenText(@"SoTran\SoTran.txt");
                                    so_tran = Int32.Parse(sr.ReadLine()) + 1;
                                    sr.Dispose();
                                    #endregion

                                    if (dem > 1800)
                                    {
                                        DoMouseClick(720, 545);
                                        //MessageBox.Show("Finish");
                                        m = 1;
                                        Thread.Sleep(10000);
                                        take_screen_shot("Tran " + so_tran);
                                        write_log_file(so_tran.ToString());
                                        //so_tran += 1;
                                    }
                                    #endregion
                                    #endregion

                                    sample_endgame.Dispose();
                                    check_endgame_Screenshot_1.Dispose();
                                    test.Dispose();
                                    endgame.Dispose();
                                    //src.Dispose();
                                    Thread.Sleep(10000); //20s
                                }
                                DoMouseClick(1080, 769);
                                //Thread.Sleep(5000);

                             

                             
                                #endregion
                                src_2.Dispose();
                            }
                        }
                        //MessageBox.Show("trở về hàng chờ nhé");
                    }
                }
            }


        }
        private void write_log_file(string text)
        {
            string path = @"SoTran\SoTran.txt";
            StreamWriter sw = File.CreateText(path);
            sw.Write(text);
            sw.Dispose();
        }

        private void take_screen_shot(string file_name_to_save)
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            string tpm = file_name_to_save + ".png";

            string subPath = @"SoTran\";

            System.IO.Directory.CreateDirectory(subPath);

            bmpScreenshot.Save(@"SoTran\" + file_name_to_save + ".png", ImageFormat.Png);
            bmpScreenshot.Dispose();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (button6WasClicked == true)
                this.timer1.Stop();
            //if (button7WasClicked == true)
            //    this.timer1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private int check_pick_tuong()
        {
            int so_pixel = 0;

            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save("Screenshot.png", ImageFormat.Png);

            Color[,] pic = new Color[56, 56];
            Bitmap sample_question_mark = Image.FromFile("question_mark.png") as Bitmap;
            Bitmap src = Image.FromFile("Screenshot.png") as Bitmap;
            for (int i = 376; i < 432; i++)
            {
                for (int j = 211; j < 267; j++)
                {
                    try
                    {
                        pic[i - 376, j - 211] = src.GetPixel(i, j);
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.ToString());
                        //MessageBox.Show(i + " " + j);
                    }

                }
            }
            for (int i = 0; i < sample_question_mark.Height; i++)
            {
                for (int j = 0; j < sample_question_mark.Width; j++)
                {
                    if (pic[i, j] == sample_question_mark.GetPixel(i, j))
                    {
                        so_pixel += 1;
                    }
                }
            }
            #region test
            //Bitmap _pic = new Bitmap(56, 56);
            //for (int i = 0; i < 56; i++)
            //{
            //    for (int j = 0; j < 56; j++)
            //    {
            //        _pic.SetPixel(i, j, pic[i,j]);
            //    }
            //}
            //_pic.Save("_pic.png", ImageFormat.Png);
            //Image<Bgr, Byte> D = new Image<Bgr, Byte>("_pic.png");
            //imageBox1.Image = D;

            //Bitmap tam = new Bitmap(56, 56);
            //for (int i = 0; i < tam.Height; i++)
            //{
            //    for (int j = 0; j < tam.Width; j++)
            //    {
            //        tam.SetPixel(i,j,src.GetPixel(i+376, j+211));
            //    }
            //}
            //tam.Save("tam.png", ImageFormat.Png);
            //Image<Bgr, Byte> D = new Image<Bgr, Byte>("tam.png");
            //imageBox1.Image = D;.
            //MessageBox.Show(so_pixel.ToString());
            #endregion
            src.Dispose();
            bmpScreenshot.Dispose();
            if (so_pixel == 56 * 56)
                return 1;
            else return 0;
        }
        private bool check_co_tran()
        {
            using (var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb))
            {
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmpScreenshot.Save("Screenshot_cotran.png", ImageFormat.Png);

                Rectangle cropRect = new Rectangle(80, 200, 1250, 550);
                Bitmap src = Image.FromFile("Screenshot_cotran.png") as Bitmap;
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                }

                target.Save("target.png", ImageFormat.Png);

                src.Dispose();


                Image<Bgr, Byte> D = new Image<Bgr, Byte>("sample.png");
                Image<Bgr, Byte> D1 = new Image<Bgr, Byte>("target.png");
                Image<Bgr, Byte> Duyen = D - D1;
                imageBox1.Image = Duyen;
                Bitmap a = new Bitmap(Duyen.ToBitmap());
                Color den = System.Drawing.ColorTranslator.FromHtml("#000000");

                bmpScreenshot.Dispose();
                src.Dispose();
                if (a.GetPixel(638, 399) != den)
                {
                    //DoMouseClick(625, 479);
                    //Thread.Sleep(10000);
                    //if (check_pick_tuong() == 1)
                    //{
                    //    //count = 1;
                    //   // hang_cho = 1;
                    //}
                    return true;
                }
                else
                    return false;
            }

        }
        private bool check_thanh_menu()
        {
            int so_pixel = 0;

            using (var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb))
            {

                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmpScreenshot.Save("Screenshot_menu.png", ImageFormat.Png);

                Color[,] pic = new Color[361, 25];
                Bitmap sample_question_mark = Image.FromFile("menu.png") as Bitmap;
                Bitmap src = Image.FromFile("Screenshot_menu.png") as Bitmap;
                for (int i = 927; i < 1288; i++)
                {
                    for (int j = 68; j < 93; j++)
                    {
                        try
                        {
                            pic[i - 927, j - 68] = src.GetPixel(i, j);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                            MessageBox.Show(i + " " + j);
                        }

                    }
                }
                for (int i = 0; i < sample_question_mark.Width; i++)
                {
                    for (int j = 0; j < sample_question_mark.Height; j++)
                    {
                        if (pic[i, j] == sample_question_mark.GetPixel(i, j))
                        {
                            so_pixel += 1;
                        }
                    }
                }
                Bitmap _pic = new Bitmap(361, 25);
                for (int i = 0; i < 361; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        _pic.SetPixel(i, j, pic[i, j]);
                    }
                }
                //_pic.Save("_menu.png", ImageFormat.Png);
                //Image<Bgr, Byte> D = new Image<Bgr, Byte>("_menu.png");
                //imageBox1.Image = D;

                //Bitmap tam = new Bitmap(361, 25);
                //for (int i = 0; i < tam.Width; i++)
                //{
                //    for (int j = 0; j < tam.Height; j++)
                //    {
                //        tam.SetPixel(i,j,src.GetPixel(i+927, j+68));
                //    }
                //}
                //tam.Save("tam.png", ImageFormat.Png);
                //Image<Bgr, Byte> D1 = new Image<Bgr, Byte>("tam.png");
                //imageBox1.Image = D1;
                //MessageBox.Show(so_pixel.ToString());

                src.Dispose();
                bmpScreenshot.Dispose();
                if (so_pixel == 361 * 25)
                    return true;
                else return false;
            }
        }
        private bool check_vao_game()
        {
            int so_pixel = 0;

            using (var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb))
            {

                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmpScreenshot.Save("Screenshot_vaogame.png", ImageFormat.Png);

                Color[,] pic = new Color[21, 21];
                Bitmap sample_question_mark = Image.FromFile("vaogame.png") as Bitmap;
                Bitmap src = Image.FromFile("Screenshot_vaogame.png") as Bitmap;
                for (int i = 1220; i < 1241; i++)
                {
                    for (int j = 679; j < 700; j++)
                    {
                        try
                        {
                            pic[i - 1220, j - 679] = src.GetPixel(i, j);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                            MessageBox.Show(i + " " + j);
                        }

                    }
                }
                for (int i = 0; i < sample_question_mark.Width; i++)
                {
                    for (int j = 0; j < sample_question_mark.Height; j++)
                    {
                        if (pic[i, j] == sample_question_mark.GetPixel(i, j))
                        {
                            so_pixel += 1;
                        }
                    }
                }
                Bitmap _pic = new Bitmap(21, 21);
                for (int i = 0; i < 21; i++)
                {
                    for (int j = 0; j < 21; j++)
                    {
                        _pic.SetPixel(i, j, pic[i, j]);
                    }
                }

                src.Dispose();
                bmpScreenshot.Dispose();
                if (so_pixel == 21 * 21)
                    return true;
                else return false;

            }
        }
    }
}
