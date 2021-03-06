﻿using System;
using System.Configuration;
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

namespace vs_test01.View
{
    using vs_test01.ViewModels;
    using vs_test01.Dao;
    using vs_test01.Models;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int val = 0;
        public MainWindow()
        {
            InitializeComponent();
            // バインディング対象のインスタンスを、MainWindowViewModelに設定
            //this.DataContext = new MainWindowViewModel();
            //this.DataContext = new MainViewModel();
            this.listBox01.SelectedIndex = -1;
            this.listBox02.SelectedIndex = -1;
            //this.listBox03.SelectedIndex = -1;
            this.listBox04.SelectedIndex = -1;
            this.listBox05.SelectedIndex = -1;
        }

        private void button01_Click(object sender, RoutedEventArgs e)
        {
            foreach (TableModel model in this.dataGrid.Items)
            {
                string cnt = OracleDao.CountTable(
                    (listBox01.SelectedItem as DBModel).DBName,
                    (listBox02.SelectedItem as SchemaModel).SchemaName,
                    model.TableName).ToString();
                SubWindow sub = new SubWindow()
                {
                    DataContext = new SubViewModel()
                    {
                        DBName = "テーブル：" + model.TableName,
                        Count = "件数：" + cnt
                    }
                };

                sub.Show();

            }
            val++;
        }
    }
}
