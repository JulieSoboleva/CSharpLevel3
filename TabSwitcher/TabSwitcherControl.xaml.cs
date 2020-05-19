using System.Windows;
using System.Windows.Controls;

namespace TabSwitcher
{
    public partial class TabSwitcherControl : UserControl
    {
        public event RoutedEventHandler Previous;
        public event RoutedEventHandler Next;

        public TabSwitcherControl() => InitializeComponent();
        
        #region properties

        private bool bHideBtnPrevious = false;
        private bool bHideBtnNext = false;
        
        public bool IsHideBtnPrevious
        {
            get { return bHideBtnPrevious; }
            set
            {
                bHideBtnPrevious = value;
                SetButtons();
            }
        }

        public bool IsHideBtnNext
        {
            get { return bHideBtnNext; }
            set
            {
                bHideBtnNext = value;
                SetButtons();
            }
        }
        
        /// <summary>
        /// Метод, который отвечает за отрисовку кнопок.
        /// </summary>
        private void SetButtons()
        {
            if (bHideBtnPrevious)
                btnPrevious.Visibility = Visibility.Hidden;
            else
                btnPrevious.Visibility = Visibility.Visible;

            if (bHideBtnNext)
                btnNext.Visibility = Visibility.Hidden;
            else
                btnNext.Visibility = Visibility.Visible;
        }
        #endregion

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
            => Previous?.Invoke(sender, e);

        private void btnNext_Click(object sender, RoutedEventArgs e)
            => Next?.Invoke(sender, e);
    }
}
