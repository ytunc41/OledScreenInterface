using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SerialPortLib;
using NLog;
using UINT8 = System.Byte;
using System.Threading;
using System.Collections;

namespace OledScreen
{
    public partial class MainForm : Form
    {
        private readonly object seriport_rx = new object();
        private readonly object paket_coz = new object();
        CommPro commPro = new CommPro();
        private static SerialPortInput serialPort;

        public MainForm()
        {
            InitializeComponent();
            this.Text += " - " + Versiyon.getVS;
            serialPort = new SerialPortInput();
            serialPort.ConnectionStatusChanged += SerialPort_ConnectionStatusChanged;
            serialPort.MessageReceived += SerialPort_MessageReceived;
            Helper.SerialPortDetect();
            chbox128x64.Checked = true;
        }

        // SerialPort Eventlari
        void SerialPort_MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            lock (paket_coz)
            {
                PaketCoz(args.Data);
            }
        }
        void SerialPort_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs args)
        {
            if (args.Connected)
            {
                BaglantiPaketOlustur();
            }
            else
            {
                if (lblStatus.InvokeRequired || progressBar.InvokeRequired)
                {
                    lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Connection failed!"));
                    progressBar.Invoke(new Action(() => progressBar.Value = progressBar.Minimum));
                }
                else
                {
                    lblStatus.Text = "Status: Connection failed!";
                    progressBar.Value = progressBar.Minimum;
                }
            }
        }

        // PaketOlustur Metotlari
        private void BaglantiPaketOlustur()
        {
            byte paket_sayaci = 0;

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (byte)PACKET_TYPE.BAGLANTI_REQUEST;

            PaketGonder(commPro);
        }
        private void VeriPaketOlustur(int pageNum)
        {
            byte paket_sayaci = 0;
            // 1 hucre satiri yani 1 page gonderilmektedir. (1hucre = 1byte, 1page = 128byte)
            int resWid = ResizeImage.Width;
            for (int i = 0; i < resWid; i++)
            {
                Paket_Islemleri_LE.UINT8_ayir(ref SendPacket.data, ref paket_sayaci, ResizeImage.ImageBuffer[i + pageNum * resWid]);
            }

            SendPacket.dataSize = paket_sayaci;
            SendPacket.packetType = (byte)PACKET_TYPE.PROGRAM_REQUEST;

            PaketGonder(commPro);
        }

        // PaketTopla Metotlari
        private void BaglantiOkPaketTopla()
        {
            if (lblStatus.InvokeRequired || progressBar.InvokeRequired)
            {
                lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Connection successfully!"));
                progressBar.Invoke(new Action(() => progressBar.Value = progressBar.Maximum));
            }
            else
            {
                lblStatus.Text = "Status: Connection successfully!";
                progressBar.Value = progressBar.Maximum;
            }
        }
        private void ProgramOkPaketTopla()
        {
            byte paket_sayaci = 0;
            uint crc32 = 0;

            Paket_Islemleri_LE.UINT32_birlestir(ReceivedPacket.data, ref paket_sayaci, ref crc32);

            if (ResizeImage.CRC32 == crc32)
            {
                if (lblStatus.InvokeRequired || progressBar.InvokeRequired)
                {
                    lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Data packet sent successfully!"));
                    progressBar.Invoke(new Action(() => progressBar.Value = progressBar.Maximum));
                }
                else
                {
                    lblStatus.Text = "Status: Data packet sent successfully!";
                    progressBar.Value = progressBar.Maximum;
                }
                    
            }
        }

        // PaketGonder Metodu
        private void PaketGonder(CommPro commPro)
        {
            commPro.txBuffer.Clear();
            commPro.txBuffer.Add(SendPacket.sof1);
            commPro.txBuffer.Add(SendPacket.sof2);
            commPro.txBuffer.Add(SendPacket.packetType);
            commPro.txBuffer.Add(++SendPacket.packetCounter);
            commPro.txBuffer.Add(SendPacket.dataSize);
            for (int i = 0; i < SendPacket.dataSize; i++)
                commPro.txBuffer.Add(SendPacket.data[i]);
            commPro.txBuffer.Add(SendPacket.eof1);
            commPro.txBuffer.Add(SendPacket.eof2);

            serialPort.SendMessage(commPro.txBuffer.ToArray());
        }

        // PaketCoz Metodu
        private void PaketCoz(UINT8[] data)
        {
            UINT8 VERI_BOYUTU = 0;
            lock (seriport_rx)
            {
                foreach (UINT8 byte_u8 in data)
                {
                    #region switch (commPro.packet_status)
                    switch (commPro.packet_status)
                    {
                        case PACKET_STATUS.SOF1:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.SOF1)
                                {
                                    ReceivedPacket.sof1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF2;
                                }
                                else
                                {
                                    commPro.Error.sof1++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.SOF2:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.SOF2)
                                {
                                    ReceivedPacket.sof2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.PACKET_TYPE;
                                }
                                else
                                {
                                    commPro.Error.sof2++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.PACKET_TYPE:
                            {
                                ReceivedPacket.packetType = byte_u8;
                                commPro.packet_status = PACKET_STATUS.PACKET_COUNTER;
                                break;
                            }
                        case PACKET_STATUS.PACKET_COUNTER:
                            {
                                ReceivedPacket.packetCounter = byte_u8;
                                commPro.packet_status = PACKET_STATUS.DATA_SIZE;
                                break;
                            }
                        case PACKET_STATUS.DATA_SIZE:
                            {
                                ReceivedPacket.dataSize = byte_u8;

                                if (ReceivedPacket.dataSize == 0)
                                {
                                    commPro.packet_status = PACKET_STATUS.EOF1;
                                    break;
                                }
                                commPro.packet_status = PACKET_STATUS.DATA;
                                break;
                            }
                        case PACKET_STATUS.DATA:
                            {
                                ReceivedPacket.data[VERI_BOYUTU++] = byte_u8;

                                if (VERI_BOYUTU == ReceivedPacket.dataSize)
                                {
                                    commPro.packet_status = PACKET_STATUS.EOF1;
                                    VERI_BOYUTU = 0;
                                }
                                break;
                            }
                        case PACKET_STATUS.EOF1:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.EOF1)
                                {
                                    ReceivedPacket.eof1 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.EOF2;
                                }
                                else
                                {
                                    commPro.Error.eof1++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        case PACKET_STATUS.EOF2:
                            {
                                if (byte_u8 == (UINT8)CHECK_STATUS.EOF2)
                                {
                                    ReceivedPacket.eof2 = byte_u8;
                                    commPro.packet_status = PACKET_STATUS.SOF1;

                                    commPro.PAKET_HAZIR_FLAG = true;
                                }
                                else
                                {
                                    commPro.Error.eof2++;
                                    commPro.packet_status = PACKET_STATUS.SOF1;
                                }
                                break;
                            }
                        default:
                            {
                                commPro.packet_status = PACKET_STATUS.SOF1;
                                break;
                            }

                    } /* switch (commPro.packet_status) */
                    #endregion

                    #region if (commPro.PAKET_HAZIR_FLAG)
                    if (commPro.PAKET_HAZIR_FLAG)
                    {
                        commPro.PACKET_TYPE_FLAG.ClearAll();

                        switch (ReceivedPacket.packetType)
                        {
                            case (UINT8)PACKET_TYPE.BAGLANTI_REQUEST:
                                {

                                    commPro.PACKET_TYPE_FLAG.BAGLANTI_REQUEST = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.BAGLANTI_OK:
                                {
                                    BaglantiOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.BAGLANTI_OK = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.PROGRAM_REQUEST:
                                {

                                    commPro.PACKET_TYPE_FLAG.PROGRAM_REQUEST = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.PROGRAM_OK:
                                {
                                    ProgramOkPaketTopla();
                                    commPro.PACKET_TYPE_FLAG.PROGRAM_OK = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.READ_REQUEST:
                                {

                                    commPro.PACKET_TYPE_FLAG.READ_REQUEST = true;
                                    break;
                                }
                            case (UINT8)PACKET_TYPE.READ_OK:
                                {

                                    commPro.PACKET_TYPE_FLAG.READ_OK = true;
                                    break;
                                }
                            
                            default:
                                break;
                        }

                        commPro.PAKET_HAZIR_FLAG = false;
                    } /* if(commPro.PAKET_HAZIR_FLAG) */
                    #endregion

                } /* foreach (UINT8 byte_u8 in data) */

            } /*  lock (seriport_rx) */

        }

        // SeriPortForm Eventlari
        private void SeriPortForm_VisibleChanged(object sender, EventArgs e)
        {
            SerialPortForm seriPortForm = (SerialPortForm)sender;
            if (!seriPortForm.Visible)
            {
                string comName = seriPortForm.ReturnText;
                string comVal = comName.Substring(comName.IndexOf("(COM") + 1, comName.IndexOf(")") - (comName.IndexOf("(COM") + 1));
                serialPort.SetPort(comVal, 115200);
                serialPort.Connect();
                seriPortForm.Dispose();
                if (lblStatus.InvokeRequired)
                    lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Trying to connect..."));
                else
                    lblStatus.Text = "Status: Trying to connect...";
                Helper.stopWatch.Start();
                Helper.threadIsConnect = new Thread(IsConnect);
                Helper.threadIsConnect.Start();
            }
        }

        private void IsConnect()
        {
            int timeout = 5000, timemin = 500;
            while (true)
            {
                if (commPro.PACKET_TYPE_FLAG.BAGLANTI_OK)
                {
                    commPro.PACKET_TYPE_FLAG.BAGLANTI_OK = false;
                    Helper.stopWatch.Reset();
                    Helper.threadIsConnect.Abort();
                }
                else
                {
                    if (Helper.stopWatch.ElapsedMilliseconds >= timemin)
                    {
                        string str = string.Format("Status: Trying to connect... {0} ms", timeout + timemin - Helper.stopWatch.ElapsedMilliseconds);
                        if (lblStatus.InvokeRequired)
                        {
                            lblStatus.EndInvoke(lblStatus.BeginInvoke(new Action(() => lblStatus.Text = str)));
                        }
                        else
                            lblStatus.Text = str;

                        if (Helper.stopWatch.ElapsedMilliseconds >= timeout + timemin)
                        {
                            serialPort.Disconnect();
                            Helper.stopWatch.Reset();
                            Helper.threadIsConnect.Abort();
                        }
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Helper.SerialPortDetect();
            List<string> comNames = Helper.comNames;

            if (!serialPort.IsConnected)
            {
                if (comNames.Count != 0)
                {
                    string device = string.Empty;
                    foreach (var item in comNames)
                    {
                        if (item.IndexOf("STM") != -1 || item.IndexOf("USB Seri Cihaz") != -1)
                        {
                            device = item;
                            break;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(device))
                    {
                        string comVal = device.Substring(device.IndexOf("(COM") + 1, device.IndexOf(")") - (device.IndexOf("(COM") + 1));
                        serialPort.SetPort(comVal, 115200);
                        serialPort.Connect();
                        if (lblStatus.InvokeRequired)
                            lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Trying to connect..."));
                        else
                            lblStatus.Text = "Status: Trying to connect...";
                        Helper.stopWatch.Start();
                        Helper.threadIsConnect = new Thread(IsConnect);
                        Helper.threadIsConnect.Start();
                    }
                    else
                    {
                        var retVal = MessageBox.Show("The ST device was not found automatically!\n\nWould you like to choose the com port?", "Serial Port Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (retVal.ToString() == "Yes")
                        {
                            Helper.SerialPortDetect();
                            SerialPortForm seriPortForm = new SerialPortForm(comNames);
                            seriPortForm.VisibleChanged += SeriPortForm_VisibleChanged;
                            seriPortForm.Show();
                        }
                        else
                        {
                            string text = "Status: Com ports connected to the computer were found but ST device was not found automatically!";
                            if (lblStatus.InvokeRequired)
                                lblStatus.Invoke(new Action(() => lblStatus.Text = text));
                            else
                                lblStatus.Text = text;
                        }
                    }
                }
                else
                {
                    if (lblStatus.InvokeRequired)
                        lblStatus.Invoke(new Action(() => lblStatus.Text = "Status: Com port connected to computer not found!"));
                    else
                        lblStatus.Text = "Status: Com port connected to computer not found!";
                    MessageBox.Show("The ST device not detected!", "Serial Port Connection", 0, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The device is already connected!", "Serial Port Connection", 0, MessageBoxIcon.Information);
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (serialPort.IsConnected)
                serialPort.Disconnect();
            else
                MessageBox.Show("The ST device is not already connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Buton Eventlari
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            progressBar.Value = progressBar.Minimum;

            pcbox1.Image = null;
            pcbox2.Image = null;
            pcbox3.Image = null;
            pcbox4.Image = null;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "jpg image (*.jpg)|*.jpg|jpeg image (*.jpeg)|*.jpeg|bmp image (*.bmp)|*.bmp|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    ResizeImage.filePath = openFileDialog.FileName;
                else
                    return;
            }

            Bitmap bmpOriginalRGB = Helper.ImageResize(new Bitmap(ResizeImage.filePath), 512, 384);
            pcbox1.Image = bmpOriginalRGB;

            progressBar.Value = progressBar.Maximum;
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            progressBar.Value = progressBar.Minimum;

            if (ResizeImage.filePath == null)
            {
                return;
            }

            int ResizeWidth, ResizeHeight;
            Dictionary<bool, Point> dict = returnProcess();
            if (dict.ContainsKey(true))
            {
                return;
            }
            else
            {
                ResizeWidth = dict[false].X;
                ResizeHeight = dict[false].Y;
                pcbox2.Size = new Size(ResizeImage.MaxWidth, ResizeImage.MaxHeight);
                pcbox3.Size = new Size(ResizeImage.MaxWidth, ResizeImage.MaxHeight);
                pcbox4.Size = new Size(ResizeImage.MaxWidth, ResizeImage.MaxHeight);
            }

            BitmapWriteProcess(ResizeImage.filePath, ResizeWidth, ResizeHeight);

            progressBar.Value = progressBar.Maximum;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            progressBar.Value = progressBar.Minimum;

            if (serialPort.IsConnected)
            {
                if (ResizeImage.ImageBuffer != null)
                {
                    if (ResizeImage.ImageBuffer.Length != 0)
                    {
                        for (int pageNum = 0; pageNum < ResizeImage.PageNumber; pageNum++)
                        {
                            VeriPaketOlustur(pageNum);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("First of all, upload image pls!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The ST device is not connected!", "Serial Port Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Yazdirma Metotlari
        private Dictionary<bool, Point> returnProcess()
        {
            Point size = new Point();
            Dictionary<bool, Point> dict = new Dictionary<bool, Point>();
            bool retVal = false;
            int ResizeWidth = 0, ResizeHeight = 0;

            if (string.IsNullOrEmpty(txtWidth.Text) || string.IsNullOrEmpty(txtHeight.Text))
            {
                MessageBox.Show("Image width/height can not empty!", "Error", 0, MessageBoxIcon.Error);
                retVal = true;
            }
            else if (!int.TryParse(txtWidth.Text, out ResizeWidth) || !int.TryParse(txtHeight.Text, out ResizeHeight))
            {
                MessageBox.Show("Image width/height must be number!", "Error", 0, MessageBoxIcon.Error);
                retVal = true;
            }
            else if (!(ResizeWidth > 0 && ResizeWidth <= ResizeImage.MaxWidth) || !(ResizeHeight > 0 && ResizeHeight <= ResizeImage.MaxHeight))
            {
                string str = string.Format("The resized image have must be:\n0 < Width <= {0}\n0 < Height <= {1}", ResizeImage.MaxWidth, ResizeImage.MaxHeight);
                MessageBox.Show(str, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtWidth.Text = ResizeImage.MaxWidth.ToString();
                txtHeight.Text = ResizeImage.MaxHeight.ToString();
                retVal = true;
            }
            else if (ResizeWidth % 8 != 0 || ResizeHeight % 8 != 0)
            {
                MessageBox.Show("Image width/height must be multiples of 8!", "Information", 0, MessageBoxIcon.Information);
                txtWidth.Text = ResizeImage.MaxWidth.ToString();
                txtHeight.Text = ResizeImage.MaxHeight.ToString();
                retVal = true;
            }
            size.X = ResizeWidth;
            size.Y = ResizeHeight;
            dict.Add(retVal, size);
            return dict;
        }
        private void BitmapWriteProcess(string filePath, int ResizeWidth, int ResizeHeight)
        {
            Bitmap bmp = new Bitmap(ResizeImage.filePath);
            Bitmap bmpResizeRGB = Helper.ImageResize(bmp, ResizeWidth, ResizeHeight);
            pcbox2.Image = bmpResizeRGB;
            Bitmap bmpResize1BPP_1 = Helper.ConvertTo1BppImage(Helper.ImageResize(bmp, ResizeWidth, ResizeHeight));
            pcbox3.Image = bmpResize1BPP_1;
            Bitmap bmpResize1BPP_2 = bmpResizeRGB.Clone(new Rectangle(0, 0, ResizeWidth, ResizeHeight), PixelFormat.Format1bppIndexed);
            pcbox4.Image = bmpResize1BPP_2;
            Bitmap oledBitmap = bmpResize1BPP_2;

            Bitmap newBmp = new Bitmap(ResizeImage.MaxWidth, ResizeImage.MaxHeight, PixelFormat.Format1bppIndexed);
            newBmp = Helper.CreateNonIndexedImage(newBmp);
            oledBitmap = Helper.CreateNonIndexedImage(oledBitmap);

            int diffWidth = (ResizeImage.MaxWidth - oledBitmap.Width) / 2;
            int diffHeight = (ResizeImage.MaxHeight - oledBitmap.Height) / 2;

            byte[] ImageBuffer = new byte[ResizeImage.MaxWidth * ResizeImage.PageNumber];
            ResizeImage.ClearAll();
            ResizeImage.Width = ResizeImage.MaxWidth;
            ResizeImage.Height = ResizeImage.MaxHeight;

            for (int i = 0; i < ResizeImage.Height; i++)
            {
                for (int j = 0; j < ResizeImage.Width; j++)
                {
                    if ((j < diffWidth) || (j >= diffWidth + oledBitmap.Width))
                    {
                        newBmp.SetPixel(j, i, Color.White);
                    }
                    else
                    {
                        if ((i < diffHeight) || (i >= diffHeight + oledBitmap.Height))
                            newBmp.SetPixel(j, i, Color.White);
                        else
                            newBmp.SetPixel(j, i, oledBitmap.GetPixel(j - diffWidth, i - diffHeight));
                    }

                    byte pixelVal = newBmp.GetPixel(j, i).R;
                    ResizeImage.ImageFullBuffer.Add(pixelVal);
                    Helper.DrawPixel(j, i, pixelVal, ref ImageBuffer);
                }
            }
            pcbox4.Image = newBmp;

            // Resim verilerinin CRC32 toplami
            foreach (var item in ImageBuffer)
                ResizeImage.CRC32 += item;

            ResizeImage.ImageBuffer = ImageBuffer;
        }

        // ComboBox Eventlari
        private void chbox128x32_CheckedChanged(object sender, EventArgs e)
        {
            if (chbox128x64.Checked)
            {
                chbox128x64.Checked = false;
            }
            else
            {
                chbox128x32.Checked = true;

                ResizeImage.MaxWidth = 128;
                ResizeImage.MaxHeight = 32;
                txtWidth.Text = "128";
                txtHeight.Text = "32";
            }
        }
        private void chbox128x64_CheckedChanged(object sender, EventArgs e)
        {
            if (chbox128x32.Checked)
            {
                chbox128x32.Checked = false;
            }
            else
            {
                chbox128x64.Checked = true;

                ResizeImage.MaxWidth = 128;
                ResizeImage.MaxHeight = 64;
                txtWidth.Text = "128";
                txtHeight.Text = "64";
            }
        }

        // MainForm Eventlari
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort.IsConnected)
            {
                serialPort.Disposed();
            }
            serialPort.Disconnect();
        }

        
    }
}
