using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pysmennyi02.Services
{
    public static class ExceptionHandlingService
    {
        public static void ShowMessage(string message, string title = "Error")
        {
            MessageBox.Show(message, title);
        }
    }
}
