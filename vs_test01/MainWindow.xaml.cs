﻿using System;
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

namespace vs_test01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            label01.Content = OracleDao.hogehoge();
        }

        private void button01_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Event handler was created manually.");
            label01.Content = OracleDao.hogehoge();
            label02.Content = textBox01.Text;
        }
    }
}
