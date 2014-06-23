/* Header of Code:
  ||-------------------------------------------------------------------------------------------------||
  || Nombre del Proyecto: CattleBoard API c#                                                         ||  
  || Autor: Jorge Luis Mayorga Taborda                                                               ||
  || Lenguaje: C#                                                                                    ||
  ||-------------------------------------------------------------------------------------------------||
  || Grupo de Investigación en Sensado Participativo y Sistemas Distribuidos GISP Uniandes           || 
  || Universidad de los Andes                                                                        || 
  || Bogota DC, Colombia 2014                                                                        || 
  ||-------------------------------------------------------------------------------------------------|| 
  || Fecha de Actualización:22/06/2014                                                               || 
  ||-------------------------------------------------------------------------------------------------||
  || Clase: MainGUI.cs                                                                               || 
  || Descripción: Entorno Grafico de la aplicación, Forma principal de la API                        || 
  || Comentarios:                                                                                    ||
  ||-------------------------------------------------------------------------------------------------||
 */

/* Importar Librerias */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Windows.Forms.DataVisualization.Charting;

namespace GISPBoardAPI
{
    public partial class MainGUI : Form
    {
        
      
        double[] Gas1PPM = new double[25]; // Gas1: CH4 [ppm]
        double[] Gas2PPM = new double[25]; // Gas2: CO2 [ppm]
        double[] Latitude = new double[25]; // Latitude: [°Rectangular]N
        double[] Longitude = new double[25]; // Latitude: [°Rectangular]W
        string[] serialportsnames = SerialPort.GetPortNames();
        double[] baudrates = {2400,4800,9600,19200,38400};
        int[] bitparidad = {0,1};
        int[] bits = {8,9};
        int[] stopbites = {0,1};
   

        //-------------------------------------------------------------//
        //------Entorno Grafico GUI -----------------------------------//
        //-------------------------------------------------------------//
        public MainGUI()
        {
            InitializeComponent(); // Iniciar la GUI y la ventana
            // Iniciar los ComboBox del Panel SerialCOM
            for (int i = 0; i < serialportsnames.Length; i++){this.comboBoxPuerto.Items.Add(serialportsnames[i]);}
            for (int i = 0; i < baudrates.Length; i++){this.comboBoxBaud.Items.Add(baudrates[i].ToString());}
            for (int i = 0; i < bits.Length; i++){this.comboBoxBits.Items.Add(bits[i].ToString());}
            for (int i = 0; i < bitparidad.Length; i++) { this.comboBoxBitsParidad.Items.Add(bitparidad[i].ToString()); }
            for (int i = 0; i < stopbites.Length; i++) { this.comboBoxStopBits.Items.Add(stopbites[i].ToString()); }
        }
        private void MainGUI_Load(object sender, EventArgs e)
        {

        }
        private void LoadFilesButton_Click(object sender, EventArgs e)
        {

            string GPSpath = @"" + Directory.GetCurrentDirectory() + @"\GPS.html";
            string[] GPSHTML = System.IO.File.ReadAllLines(GPSpath);

            chart1.Series["Series1"].Points.DataBindY(Gas1PPM);
            chart1.Series["Series2"].Points.DataBindY(Gas2PPM);

            var realSize = chart1.Size;
            chart1.Size = new Size(1200, 800);
            this.chart1.SaveImage(@"" + Directory.GetCurrentDirectory() + @"\DataPlot.png" + "", ChartImageFormat.Png);
            chart1.Size = realSize;

            StringBuilder sb = new StringBuilder();
            int k = -1;
            
            for (int j = 0; j < GPSHTML.Length;j++)
            {
                
                if (GPSHTML[j] == "//LocationsDataDownBelove")
                { k = j + 2; }
                if (j==k)
                {
                    int l = Latitude.Length - 4;
                    for(int gpsi=1;gpsi<l;gpsi++)
                    {
                        if (gpsi == l - 1) { sb.Append("['<h4> CH4:" + Gas1PPM[gpsi].ToString() + " CO2: " + Gas2PPM[gpsi].ToString() + "</h4>', " + Latitude[gpsi].ToString() + ", " + Longitude[gpsi].ToString() + "] \r\n"); }
                        else
                        {
                            sb.Append("['<h4> CH4:" + Gas1PPM[gpsi].ToString() + " CO2: " + Gas2PPM[gpsi].ToString() + " Lat: "+Latitude[gpsi].ToString()+" Lon: "+Longitude[gpsi].ToString()+" </h4>' , " + Latitude[gpsi].ToString() + ", " + Longitude[gpsi].ToString() + "], \r\n");
                        }
                        } 
                    k = -1;
                }
                else { 
                sb.Append(GPSHTML[j] + "\r\n"); 
                }
               
            }

            string path = @"" + Directory.GetCurrentDirectory() + @"\GPS2.html";
            StreamWriter bw = new StreamWriter(File.Create(path));
            StringBuilder df = new StringBuilder();
            for (int i = 0; i < sb.Length; i++)
            {

                df.Append(sb.ToString()[i]);
            }
            bw.Write(df.ToString());
            bw.Dispose();

            string curDir = Directory.GetCurrentDirectory();
            this.webBrowser1.Url = new Uri(String.Format(@"file:///{0}/GPS2.html", curDir));

        }
        private void cOM2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void gISPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("© GISP es un grupo de Investación de la Universidad de los Andes.", "GISP About",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
        if (MessageBox.Show("¿Realmente esta seguro de salir de GISP API Data Center?", "GISP API No quiere irse :'(",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void usartBoton_Click(object sender, EventArgs e)
        {
            var NamePort = this.comboBoxPuerto.GetItemText(this.comboBoxPuerto.SelectedItem);
            var BaudPort = this.comboBoxBaud.GetItemText(this.comboBoxBaud.SelectedItem);
            var BitsPort = this.comboBoxBits.GetItemText(this.comboBoxBits.SelectedItem);
            var ParidadPort = this.comboBoxBitsParidad.GetItemText(this.comboBoxBitsParidad.SelectedItem);
            var StopPort = this.comboBoxStopBits.GetItemText(this.comboBoxStopBits.SelectedItem);
            SerialPort ComPort = new SerialPort(NamePort.ToString(), Convert.ToInt32(BaudPort));
            ComPort.Close();
            ComPort.Open();
            ComPort.Write("API Conectada..Lista para Recibir Datos Motherfucker!");
            int NoDatos = 0;
            byte[] comBuffer = new byte[8];
            byte[] comBuffer2 = new byte[8];
            byte[] comBuffer3 = new byte[8];
            string str;
            
            int[] Gas1 = new int[9];
            int[] Gas2 = new int[9];
            int[] LatTemp = new int[9];
            int[] LonTemp = new int[9];


            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            progressBar1.Maximum = 20;
            progressBar1.Value = 0;
            // Get Char //


 
           
            while (NoDatos<20)
            {
                
                USARTlabel.Text = NoDatos.ToString();
                // Get Char //
                ComPort.Read(comBuffer, 0, 8);
                str = enc.GetString(comBuffer);
                
                USARTlabel.Text = str;
               
          
                if (string.Compare(str, "$") == 0)
                {
                    NoDatos = NoDatos + 1;
                    progressBar1.Value += 1;
                    str = "";

                    ComPort.Read(comBuffer, 0, 8);//[
                    str = enc.GetString(comBuffer);

                    //Gas1
                    for (int j = 0; j < 8; j++)
                    {
                        ComPort.Read(comBuffer, 0, 8);//1000
                        str = enc.GetString(comBuffer);

                        if (string.Compare(str, ".") == 0)
                        {
                            ComPort.Read(comBuffer, 0, 8);
                            str = enc.GetString(comBuffer);
                        }
                        USARTlabel.Text = str;
                        Gas1[j] = Convert.ToInt32(str);
                    }
                    ComPort.Read(comBuffer, 0, 8);//]
                    str = enc.GetString(comBuffer);
                    ComPort.Read(comBuffer, 0, 8);//[
                    str = enc.GetString(comBuffer);
                    //Gas2
                    for (int j = 0; j < 8; j++)
                    {
                        ComPort.Read(comBuffer, 0, 8);//1000
                        str = enc.GetString(comBuffer);
                        if (string.Compare(str, ".") == 0)
                        {
                            ComPort.Read(comBuffer, 0, 8);
                            str = enc.GetString(comBuffer);
                        }
                        Gas2[j] = Convert.ToInt32(str);
                    }
                    ComPort.Read(comBuffer, 0, 8);//]
                    str = enc.GetString(comBuffer);
                    ComPort.Read(comBuffer, 0, 8);//[
                    str = enc.GetString(comBuffer);
                    //Lat

                    for (int j = 0; j < 8; j++)
                    {
                        ComPort.Read(comBuffer, 0, 8);//1000
                        str = enc.GetString(comBuffer);
                        if (string.Compare(str, ".") == 0)
                        {
                            ComPort.Read(comBuffer, 0, 8);
                            str = enc.GetString(comBuffer);
                        }
                        LatTemp[j] = Convert.ToInt32(str);
                    }
                    ComPort.Read(comBuffer, 0, 8);//]
                    str = enc.GetString(comBuffer);
                    ComPort.Read(comBuffer, 0, 8);//[
                    str = enc.GetString(comBuffer);
                    //Lat
                    for (int j = 0; j < 8; j++)
                    {
                        ComPort.Read(comBuffer, 0, 8);//1000
                        str = enc.GetString(comBuffer);
                        if (string.Compare(str, ".") == 0)
                        {
                            ComPort.Read(comBuffer, 0, 8);
                            str = enc.GetString(comBuffer);
                        }
                        LonTemp[j] = Convert.ToInt32(str);
                    }
                    ComPort.Read(comBuffer, 0, 8);//]
                    str = enc.GetString(comBuffer);




                    double ppmGas1 = Gas1[0] * 1000 + Gas1[1] * 100 + Gas1[2] * 10 + Gas1[3] * 1 + 0.1 * Gas1[4] + 0.01 * Gas1[5] + 0.001 * Gas1[6] + 0.0001 * Gas1[7];
                    double ppmGas2 = Gas2[0] * 1000 + Gas2[1] * 100 + Gas2[2] * 10 + Gas2[3] * 1 + 0.1 * Gas2[4] + 0.01 * Gas2[5] + 0.001 * Gas2[6] + 0.0001 * Gas2[7];
                    double gpsLat = LatTemp[0] * 10 + LatTemp[1] * 1 + LatTemp[2] * 0.1 + LatTemp[3] * 0.01 + LatTemp[4] * 0.001 + LatTemp[5] * 0.0001 + LatTemp[6] * 0.00001 + LatTemp[7] * 0.000001;
                    double gpsLon = LonTemp[0] * 100 + LonTemp[1] * 10 + LonTemp[2] * 1 + LonTemp[3] * 0.1 + LonTemp[4] * 0.01 + LonTemp[5] * 0.001 + LonTemp[6] * 0.0001 + LonTemp[7] * 0.00001;


                    Gas1Label.Text = ppmGas1.ToString();
                    Gas2Label.Text = ppmGas2.ToString();

                    Gas1PPM[NoDatos] = ppmGas1;
                    Gas2PPM[NoDatos] = ppmGas2;
                    Latitude[NoDatos] = gpsLat;
                    Longitude[NoDatos] = -1*gpsLon;
                }
                else
                {
                    NoDatos = NoDatos + 0;
                }
                USARTlabel.Text = NoDatos.ToString();
                
            }
            
            ComPort.Close();
            chart1.Series["Series1"].Points.DataBindY(Gas1PPM);
            chart1.Series["Series2"].Points.DataBindY(Gas2PPM);

           
            this.chart1.SaveImage(@"" + Directory.GetCurrentDirectory() + @"\DataPlot.png" + "", ChartImageFormat.Png);
            
        }
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TXT|*.txt";
            sfd.FileName = "GasDataSensor";
            sfd.Title = "Guardar Archivo de datos de CH4 y CO2";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = sfd.FileName;
                StreamWriter bw = new StreamWriter(File.Create(path));
                StringBuilder df = new StringBuilder();
                df.Append("CH4  CO2  Lat Lon\r\n");
                for (int i = 1; i < Gas1PPM.Length; i++)
                {
                    df.Append(Gas1PPM[i].ToString() + "  " + Gas2PPM[i].ToString() + " " + Latitude[i].ToString() + "" + Longitude[i].ToString() + "\r\n");
                }
                bw.Write(df.ToString());
                bw.Dispose();
            }
        }
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Gas1PPM.Length; i++)
            {
                Gas1PPM[i] = 0;
                Gas2PPM[i] = 0;
                chart1.Series["Series1"].Points.DataBindY(Gas1PPM);
                chart1.Series["Series2"].Points.DataBindY(Gas2PPM);

            }
        }
        private void cOM1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        private void FTPBoton_Click(object sender, EventArgs e)
        {
            String sourcefilepath = @"" + Directory.GetCurrentDirectory() + @"\DataPlot.png"; // e.g. “d:/test.docx”
            String ftpurl = "@ftp://cattlesensorwireless.2trweb.com/public_html/"; // e.g. ftp://serverip/foldername/foldername
            String ftpusername = "@u904348523"; // e.g. username
            String ftppassword = "@lvosca.inc"; // e.g. password
            try
            {
                string filename = Path.GetFileName(sourcefilepath);
                string ftpfullpath = ftpurl;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpusername, ftppassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(sourcefilepath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }



        private string[] GPSWebUpdate(double[] x, double[] y)
        {
            string[] outString=new string[5];
            outString[0]="Hola";
            outString[1]="Hola";
            outString[2]="Hola";
            outString[3]="Hola";
            outString[4]="Hola";
            return outString;
        }
    }
}
