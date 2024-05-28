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

using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;


namespace FxShow.User_Interface.Windows
{
    /// <summary>
    /// Interaction logic for wndDX_ExploreInstances.xaml
    /// </summary>
    public partial class wndDX_About : DXWindow
    {
        public wndDX_About()
        {
            InitializeComponent();
            this.Width = Data.Config.ScreenWidth_Explore;
            this.Height = Data.Config.ScreenHeight_Explore;
        }


        private void ShowAs_Checked(object sender, RoutedEventArgs e)
        {
            //if (groupContainer == null)
            //    return;

            //LayoutGroupView containerView, childView;

            //if (sender == checkShowAsGroupBoxes)
            //{
            //    containerView = LayoutGroupView.GroupBox;
            //    childView = LayoutGroupView.GroupBox;
            //}
            //else
            //{
            //    if (sender == checkShowAsTabs)
            //    {
            //        containerView = LayoutGroupView.Tabs;
            //        childView = LayoutGroupView.Group;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            //groupContainer.View = containerView;

            //foreach (FrameworkElement child in groupContainer.GetLogicalChildren(false))
            //{
            //    if (child is LayoutGroup)
            //    {
            //        ((LayoutGroup)child).View = childView;
            //    }
            //}
        }

        private void DXWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //AssemblyHelper.AssemblyInformation info = new AssemblyHelper.AssemblyInformation(System.Reflection.Assembly.GetCallingAssembly());
            //AssemblyHelper.AssemblyInformation info = new AssemblyHelper.AssemblyInformation(System.Reflection.Assembly.GetExecutingAssembly());

            //textBlock_Version.Text = info.ToString();

            //System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["serversInstancesViewSource"];
            //// Things work if this line is present.  Testing to see if it works without 6/13/2012
            //// Yup, still works.  Don't need this line as it is done in the XAML.
            //myCollectionViewSource.Source = FxShow.Common.ApplicationDataSet.Instances;

            //System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["serversViewSource"];
            //// Things work if this line is present.  Testing to see if it works without 6/13/2012
            //// Yup, still works.  Don't need this line as it is done in the XAML.
            //myCollectionViewSource.Source = FxShow.Common.ApplicationDataSet.Servers;

            //serversGridControl.GroupBy("Environment");
            //serversGridControl.Set

            // Update the views.  First ensure a row is selected.

            //tableView1.FocusedRowHandle = 1;

            //tableView1.BestFitColumns();
            //tableView2.BestFitColumns();
            //tableView3.BestFitColumns();
            ////tableView4.BestFitColumns();
            ////tableView5.BestFitColumns();
            ////tableView6.BestFitColumns();
            //tableView7.BestFitColumns();
            //tableView8.BestFitColumns();

            //serversGridControl.GroupBy("SecurityZone");
        }

        private void OnDisplayIDColumns_Checked(object sender, RoutedEventArgs e)
        {
            //HideIDColumns(((CheckBox)sender).IsChecked);
            //gridColumn_ID1.Visible = true;
            //gridColumn_ID52.Visible = true;
            //gridColumn_ID5a2.Visible = true;

            //gridColumn_ID2.Visible = true;
            //gridColumn_ID2a.Visible = true;

            //gridColumn_ID3.Visible = true;
            //gridColumn_ID3a.Visible = true;

            ////gridColumn_ID4.Visible = true;
            ////gridColumn_ID4a.Visible = true;

            ////gridColumn_ID5.Visible = true;
            ////gridColumn_ID5a.Visible = true;

            //gridColumn_ID6.Visible = true;
            //gridColumn_ID6a.Visible = true;

            ////gridColumn_ID7.Visible = true;
            ////gridColumn_ID7a.Visible = true;

            //gridColumn_ID8.Visible = true;
            //gridColumn_ID8a.Visible = true;
        }


        private void HideIDColumns(Nullable<bool> isChecked)
        {
            //if ((bool)isChecked)
            //{
            //    gridColumn_ID1.Visible = true;
            //}
            //else
            //{
            //    gridColumn_ID1.Visible = false;
            //}
        }

        private void ckDisplayIDColumns_Unchecked(object sender, RoutedEventArgs e)
        {
            //gridColumn_ID52.Visible = false;
            //gridColumn_ID5a2.Visible = false;
            //gridColumn_ID1.Visible = false;

            //gridColumn_ID2.Visible = false;
            //gridColumn_ID2a.Visible = false;

            //gridColumn_ID3.Visible = false;
            //gridColumn_ID3a.Visible = false;

            ////gridColumn_ID4.Visible = false;
            ////gridColumn_ID4a.Visible = false;

            ////gridColumn_ID5.Visible = false;
            ////gridColumn_ID5a.Visible = false;

            //gridColumn_ID6.Visible = false;
            //gridColumn_ID6a.Visible = false;

            ////gridColumn_ID7.Visible = false;
            ////gridColumn_ID7a.Visible = false;

            //gridColumn_ID8.Visible = false;
            //gridColumn_ID8a.Visible = false;
        }

        private void OnDisplaySnapShotColumns_Checked(object sender, RoutedEventArgs e)
        {
            //gridColumn_SnapShotDate1.Visible = true;
            //gridColumn_SnapShotError1.Visible = true;

            //gridColumn_SnapShotDate2.Visible = true;
            //gridColumn_SnapShotError2.Visible = true;

            //gridColumn_SnapShotDate3.Visible = true;
            //gridColumn_SnapShotError3.Visible = true;

            ////gridColumn_SnapShotDate4.Visible = true;
            ////gridColumn_SnapShotError4.Visible = true;

            ////gridColumn_SnapShotDate5.Visible = true;
            ////gridColumn_SnapShotError5.Visible = true;

            //gridColumn_SnapShotDate6.Visible = true;
            //gridColumn_SnapShotError6.Visible = true;

            ////gridColumn_SnapShotDate7.Visible = true;
            ////gridColumn_SnapShotError7.Visible = true;

            //gridColumn_SnapShotDate8.Visible = true;
            //gridColumn_SnapShotError8.Visible = true;
        }

        private void ckDisplaySnapShotColumns_Unchecked(object sender, RoutedEventArgs e)
        {
            //gridColumn_SnapShotDate1.Visible = false;
            //gridColumn_SnapShotError1.Visible = false;

            //gridColumn_SnapShotDate2.Visible = false;
            //gridColumn_SnapShotError2.Visible = false;

            //gridColumn_SnapShotDate3.Visible = false;
            //gridColumn_SnapShotError3.Visible = false;

            ////gridColumn_SnapShotDate4.Visible = false;
            ////gridColumn_SnapShotError4.Visible = false;

            ////gridColumn_SnapShotDate5.Visible = false;
            ////gridColumn_SnapShotError5.Visible = false;

            //gridColumn_SnapShotDate6.Visible = false;
            //gridColumn_SnapShotError6.Visible = false;

            ////gridColumn_SnapShotDate7.Visible = false;
            ////gridColumn_SnapShotError7.Visible = false;

            //gridColumn_SnapShotDate8.Visible = false;
            //gridColumn_SnapShotError8.Visible = false;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //Common.ApplicationDataSet.InstancesTA.Update(Common.ApplicationDataSet.Instances);
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            //Common.ApplicationDataSet.Instances.RejectChanges();
        }

        private void DXWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("DXWindow - Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}", 
            //    this.Width, this.Height, this.ActualWidth, this.ActualHeight));

            //System.Diagnostics.Debug.WriteLine(string.Format("    layoutControlRoot - Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}",
            //    layoutControlRoot.Width, layoutControlRoot.Height, layoutControlRoot.ActualWidth, layoutControlRoot.ActualHeight));

            //System.Diagnostics.Debug.WriteLine(string.Format("        layoutGroupHeader - Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}",
            //    layoutGroupHeader.Width, this.layoutGroupHeader.Height, layoutGroupHeader.ActualWidth, layoutGroupHeader.ActualHeight));

            //System.Diagnostics.Debug.WriteLine(string.Format("        layoutGroupBody   - Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}",
            //    layoutGroupBody.Width, layoutGroupBody.Height, layoutGroupBody.ActualWidth, layoutGroupBody.ActualHeight));

            //System.Diagnostics.Debug.WriteLine(string.Format("        layoutGroupFooter - Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}",
            //    layoutGroupFooter.Width, layoutGroupFooter.Height, layoutGroupFooter.ActualWidth, layoutGroupFooter.ActualHeight));

            //System.Diagnostics.Debug.WriteLine(string.Format("OnSizeChanged: Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}  \nRoot:{4}:{5} \nServers:{6}:{7}  \nInstances:{8}:{9}  \nDatabases:{10}:{11}",
            //    this.Width, this.Height, this.ActualWidth, this.ActualHeight,
            //    layoutPanelRoot.ActualHeight, layoutPanelRoot.ItemHeight,
            //    layoutGroupServers.ActualHeight, layoutGroupServers.Height,
            //    layoutGroupInstances.ActualHeight, layoutGroupInstances.Height,
            //    layoutGroupDatabases.ActualHeight, layoutGroupDatabases.Height));

            //layoutPanelRoot.ItemHeight = new GridLength(this.Height - 200);
            //layoutGroupServers.Height = layoutPanelRoot.ItemHeight.Value - 25;
            //layoutGroupInstances.Height = layoutPanelRoot.ItemHeight.Value - 20;
            //layoutGroupDatabases.Height = layoutPanelRoot.ItemHeight.Value - 15;

            //System.Diagnostics.Debug.WriteLine(string.Format("OnSizeChanged: Width:{0} Height:{1}  ActualWidth:{2} ActualHeight:{3}  \nRoot:{4}:{5} \nServers:{6}:{7}  \nInstances:{8}:{9}  \nDatabases:{10}:{11}",
            //    this.Width, this.Height, this.ActualWidth, this.ActualHeight,
            //    layoutPanelRoot.ActualHeight, layoutPanelRoot.ItemHeight,
            //    layoutGroupServers.ActualHeight, layoutGroupServers.Height,
            //    layoutGroupInstances.ActualHeight, layoutGroupInstances.Height,
            //    layoutGroupDatabases.ActualHeight, layoutGroupDatabases.Height));
        }

        private void OnSendFeedback(object sender, RoutedEventArgs e)
        {
            Helpers.Email.SendEmail(FxShow.Data.Config.Email_To, "bob@foo.com", "FxShow Feedback", "", "");
        }

    }

}
