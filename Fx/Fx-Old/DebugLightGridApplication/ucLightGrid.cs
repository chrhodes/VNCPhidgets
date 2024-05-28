using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugLightGridApplication
{
    //public partial class ucLightGrid : UserControl, IDisplayLight
    public partial class ucLightGrid : UserControl
    {
        public ucLightGrid()
        {
            InitializeComponent();
        }

        #region IDisplayLight Members

        public void On(int displayLightId)
        {
            switch(displayLightId)
            {
                case 0:
                    displayLight0.BackColor = Color.Green;
                    break;

                case 1:
                    displayLight1.BackColor = Color.Red;
                    break;

                case 2:
                    displayLight2.BackColor = Color.Blue;
                    break;

                case 3:
                    displayLight3.BackColor = Color.Green;
                    break;

                case 4:
                    displayLight4.BackColor = Color.Red;
                    break;

                case 5:
                    displayLight5.BackColor = Color.Blue;
                    break;

                case 6:
                    displayLight6.BackColor = Color.Red;
                    break;

                case 7:
                    displayLight7.BackColor = Color.Blue;
                    break;

                case 8:
                    displayLight8.BackColor = Color.Green;
                    break;

                case 9:
                    displayLight9.BackColor = Color.Red;
                    break;

                case 10:
                    displayLight10.BackColor = Color.Blue;
                    break;

                case 11:
                    displayLight11.BackColor = Color.Green;
                    break;

                case 12:
                    displayLight12.BackColor = Color.Blue;
                    break;

                case 13:
                    displayLight13.BackColor = Color.Green;
                    break;

                case 14:
                    displayLight14.BackColor = Color.Red;
                    break;

                case 15:
                    displayLight15.BackColor = Color.Blue;
                    break;

                case 16:
                    displayLight16.BackColor = Color.Green;
                    break;

                case 17:
                    displayLight17.BackColor = Color.Red;
                    break;

                case 18:
                    displayLight18.BackColor = Color.Green;
                    break;

                case 19:
                    displayLight19.BackColor = Color.Red;
                    break;

                case 20:
                    displayLight20.BackColor = Color.Blue;
                    break;

                case 21:
                    displayLight21.BackColor = Color.Green;
                    break;

                case 22:
                    displayLight22.BackColor = Color.Red;
                    break;

                case 23:
                    displayLight23.BackColor = Color.Blue;
                    break;

                default:

                    break;
            }
        }

        public void Off(int displayLightId)
        {
            switch(displayLightId)
            {
                case 0:
                    displayLight0.BackColor = Color.White;
                    break;

                case 1:
                    displayLight1.BackColor = Color.White;
                    break;

                case 2:
                    displayLight2.BackColor = Color.White;
                    break;

                case 3:
                    displayLight3.BackColor = Color.White;
                    break;

                case 4:
                    displayLight4.BackColor = Color.White;
                    break;

                case 5:
                    displayLight5.BackColor = Color.White;
                    break;

                case 6:
                    displayLight6.BackColor = Color.White;
                    break;

                case 7:
                    displayLight7.BackColor = Color.White;
                    break;

                case 8:
                    displayLight8.BackColor = Color.White;
                    break;

                case 9:
                    displayLight9.BackColor = Color.White;
                    break;

                case 10:
                    displayLight10.BackColor = Color.White;
                    break;

                case 11:
                    displayLight11.BackColor = Color.White;
                    break;

                case 12:
                    displayLight12.BackColor = Color.White;
                    break;

                case 13:
                    displayLight13.BackColor = Color.White;
                    break;

                case 14:
                    displayLight14.BackColor = Color.White;
                    break;

                case 15:
                    displayLight15.BackColor = Color.White;
                    break;

                case 16:
                    displayLight16.BackColor = Color.White;
                    break;

                case 17:
                    displayLight17.BackColor = Color.White;
                    break;

                case 18:
                    displayLight18.BackColor = Color.White;
                    break;

                case 19:
                    displayLight19.BackColor = Color.White;
                    break;

                case 20:
                    displayLight20.BackColor = Color.White;
                    break;

                case 21:
                    displayLight21.BackColor = Color.White;
                    break;

                case 22:
                    displayLight22.BackColor = Color.White;
                    break;

                case 23:
                    displayLight23.BackColor = Color.White;
                    break;

                default:

                    break;
            }
        }

        #endregion
    }
}
