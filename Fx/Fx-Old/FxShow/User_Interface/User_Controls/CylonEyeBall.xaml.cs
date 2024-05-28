using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FxShow.User_Interface.User_Controls
{
    /// <summary>
    /// Interaction logic for CylonEyeBall.xaml
    /// </summary>
    public partial class CylonEyeBall : UserControl
    {
        const string TYPE_NAME = "CylonEyeBall";

        public CylonEyeBall()
        {
#if TRACE
            //Common.WriteToDebugWindow(string.Format("{0}:{1}()", TYPE_NAME, System.Reflection.MethodInfo.GetCurrentMethod().Name));
#endif
            InitializeComponent();

        }
    }
}
