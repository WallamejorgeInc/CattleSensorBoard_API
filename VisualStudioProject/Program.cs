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
  || Clase: Program.cs                                                                               || 
  || Descripción: Main Code del programa, arranca la  MainGUI.cs y la ejecuta                        || 
  || Comentarios:                                                                                    ||
  ||-------------------------------------------------------------------------------------------------||
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISPBoardAPI
{
    static class Program
    {
        static MainGUI mainGui;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            mainGui = new MainGUI();
            Application.Run(mainGui);
        }
    }
}
