using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModifiedComboBox
{
    public partial class ModifiedComboBoxControl : UserControl
    {
        private string strLabel;
        private string[] strArray;
        private Dictionary<string, string> cbSourse;
        private bool isHideComboBox = false;
        private bool isHidebtnAdd = false;
        private bool isHidebtnEdit = false;
        private bool isHidebtnDelete = false;
        private int selectedComboIndex = 0;
        
        public event RoutedEventHandler btnAddClick;
        public event RoutedEventHandler btnEditClick;
        public event RoutedEventHandler btnDeleteClick;

        public ModifiedComboBoxControl() => InitializeComponent();

        public string StrLabel
        {
            get { return strLabel; }
            set
            {
                strLabel = value;
                label.Content = strLabel;
            }
        }

        public string[] StrArray
        {
            get { return strArray; }
            set
            {
                strArray = value;
                comboBox.ItemsSource = strArray;
                comboBox.SelectedIndex = 0;
            }
        }

        public Dictionary<string, string> CbSourse
        {
            get { return cbSourse; }
            set 
            {
                cbSourse = value;
                comboBox.ItemsSource = cbSourse;
                comboBox.DisplayMemberPath = "Key";
                comboBox.SelectedValuePath = "Value";
                comboBox.SelectedIndex = 0;
            }
        }

        public string Text { get { return comboBox.Text; } }
        public string Value { get { return comboBox.SelectedValue.ToString(); } }

        public int SelectedComboIndex
        {
            get 
            { 
                selectedComboIndex = comboBox.SelectedIndex; 
                return selectedComboIndex; 
            }
            set
            {
                selectedComboIndex = value;
                comboBox.SelectedIndex = selectedComboIndex;
            }
        }

        public bool IsHideComboBox
        {
            get { return isHideComboBox; }
            set
            {
                isHideComboBox = value;
                SetButtons(); // метод, который отвечает на отрисовку кнопок
            }
        }

        public bool IsHidebtnAdd
        {
            get { return isHidebtnAdd; }
            set
            {
                isHidebtnAdd = value;
                SetButtons(); // метод, который отвечает на отрисовку кнопок
            }
        }

        public bool IsHidebtnEdit
        {
            get { return isHidebtnEdit; }
            set
            {
                isHidebtnEdit = value;
                SetButtons(); // метод, который отвечает на отрисовку кнопок
            }
        }

        public bool IsHidebtnDelete
        {
            get { return isHidebtnDelete; }
            set
            {
                isHidebtnDelete = value;
                SetButtons(); // метод, который отвечает на отрисовку кнопок
            }
        }

        private void SetButtons()
        {
            if (isHideComboBox)
                comboBox.Visibility = Visibility.Hidden;
            else
                comboBox.Visibility = Visibility.Visible;

            if (isHidebtnAdd)
                btnAdd.Visibility = Visibility.Hidden;
            else
                btnAdd.Visibility = Visibility.Visible;

            if (isHidebtnEdit)
                btnEdit.Visibility = Visibility.Hidden;
            else
                btnEdit.Visibility = Visibility.Visible;

            if (isHidebtnDelete)
                btnDelete.Visibility = Visibility.Hidden;
            else
                btnDelete.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
            => btnAddClick?.Invoke(sender, e);

        private void btnEdit_Click(object sender, RoutedEventArgs e)
            => btnEditClick?.Invoke(sender, e);

        private void btnDelete_Click(object sender, RoutedEventArgs e)
            => btnDeleteClick?.Invoke(sender, e);
    }
}
