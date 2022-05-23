using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Control_CS
{

    public static class StaticVariables
    {
        public static string AppName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
        public static CancellationTokenSource tokenSource = null;
        //public static CancellationToken ct;
    }

    class GeneralFunctions
    {
        public static void SetStartup(bool cb_runOnStartUpChecked)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (cb_runOnStartUpChecked)
                rk.SetValue(StaticVariables.AppName, Application.ExecutablePath.ToString());
            else
                rk.DeleteValue(StaticVariables.AppName, false);
        }

        public static bool checkStartupState()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk.GetValue(StaticVariables.AppName) != null)
                return true;
            else
                 return false;
        }

        public static bool UnorderedEqual(List<Port> list1, List<Port> list2)
        {

            if (list1.Count == list2.Count)
            {
                list1.Sort(delegate (Port x, Port y)
                {
                    if (x.Name == null && y.Name == null) return 0;
                    else if (x.Name == null) return -1;
                    else if (y.Name == null) return 1;
                    else return x.Name.CompareTo(y.Name);
                });
                list2.Sort(delegate (Port x, Port y)
                {
                    if (x.Name == null && y.Name == null) return 0;
                    else if (x.Name == null) return -1;
                    else if (y.Name == null) return 1;
                    else return x.Name.CompareTo(y.Name);
                });
                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i].Name == list2[i].Name)
                        continue;
                    else
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

}

#region Custom Exceptions
public class PortNotFoundException : Exception
    {
        public PortNotFoundException()
        {
        }

        public PortNotFoundException(string message)
            : base(message)
        {
        }

        public PortNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class NoDefaultPortFoundException : Exception
    {
        public NoDefaultPortFoundException()
        {
        }

        public NoDefaultPortFoundException(string message)
            : base(message)
        {
        }

        public NoDefaultPortFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    #endregion

}
