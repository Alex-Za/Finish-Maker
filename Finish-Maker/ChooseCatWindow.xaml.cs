using Finish_Maker.Models.FileModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Finish_Maker
{
    /// <summary>
    /// Логика взаимодействия для ChooseCatWindow.xaml
    /// </summary>
    public partial class ChooseCatWindow : Window
    {
        private int finishCount;
        public ChooseCatWindow(List<List<string>> categoryData, string dataType)
        {
            InitializeComponent();

            if (dataType == "subtypes")
            {
                CreateSubtypeData(categoryData);
            }
            else
            {
                CreateCategoryData(categoryData);
            }


        }

        private void CreateSubtypeData(List<List<string>> categoryData)
        {
            for (int i = 0; i < 2; i++)
            {
                StackPanel sp = new StackPanel
                {
                    Name = "stackPanel" + (i + 1)
                };
                sp.Drop += txtTargetDrop;

                TextBlock tb = new TextBlock
                {
                    Text = "Finish # " + (i + 1)
                };
                sp.Children.Add(tb);

                if (i == 0)
                {
                    foreach (string item in categoryData[0])
                    {
                        Label lb = new Label
                        {
                            Content = item
                        };
                        lb.MouseDown += lblMouseDown;
                        sp.Children.Add(lb);
                    }
                }

                finishFilesPanel.Children.Add(sp);
                finishCount = 2;
            }
        }
        private void CreateCategoryData(List<List<string>> categoryData)
        {
            for (int i = 0; i < 2; i++)
            {
                StackPanel sp = new StackPanel
                {
                    Name = "stackPanel" + (i + 1)
                };
                sp.Drop += txtTargetDrop;

                TextBlock tb = new TextBlock
                {
                    Text = "Finish # " + (i + 1)
                };
                sp.Children.Add(tb);

                if (i == 0)
                {
                    foreach (string item in categoryData[1])
                    {
                        Label lb = new Label
                        {
                            Content = item
                        };
                        lb.MouseDown += lblMouseDown;
                        sp.Children.Add(lb);
                    }
                }

                finishFilesPanel.Children.Add(sp);
                finishCount = 2;
            }
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            FinishFileData.FileData = null;
            FinishFileData.ChooseCategoryStatus = false;
            ChooseCategoryWindow.Close();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            int finishCount = finishFilesPanel.Children.OfType<StackPanel>().Count();
            for (int i = 1; i < finishCount; i++)
            {
                StackPanel sp = finishFilesPanel.Children.OfType<StackPanel>().ElementAt(i);
                if (sp.Children.OfType<Label>().FirstOrDefault() != null)
                {
                    FinishFileData.ChooseCategoryStatus = true;
                    break;
                }
            }

            if (FinishFileData.ChooseCategoryStatus)
            {
                FinishFileData.FileData = new List<List<string>>();

                for (int i = 0; i < finishCount; i++)
                {
                    StackPanel sp = finishFilesPanel.Children.OfType<StackPanel>().ElementAt(i);
                    if (sp.Children.OfType<Label>().FirstOrDefault() != null)
                    {
                        int labelCount = sp.Children.OfType<Label>().Count();
                        List<string> categorys = new List<string>();
                        for (int x = 0; x < labelCount; x++)
                        {
                            Label lb = sp.Children.OfType<Label>().ElementAt(x);
                            categorys.Add(lb.Content.ToString());
                        }
                        FinishFileData.FileData.Add(categorys);
                    }
                }
                ChooseCategoryWindow.Hide();
            }
            else
            {
                FinishFileData.FileData = null;
                FinishFileData.ChooseCategoryStatus = false;
                ChooseCategoryWindow.Close();
            }
        }
        private void ChooseCatWindowClosing(object sender, EventArgs e)
        {
            FinishFileData.ChooseCategoryStatus = false;
            FinishFileData.FileData = null;
        }
        private void AddNewFinish(object sender, RoutedEventArgs e)
        {
            if (finishCount < 15)
            {
                StackPanel sp = new StackPanel
                {
                    Name = "stackPanel" + (finishCount + 1)
                };
                sp.Drop += txtTargetDrop;

                TextBlock tb = new TextBlock
                {
                    Text = "Finish # " + (finishCount + 1)
                };
                sp.Children.Add(tb);
                finishFilesPanel.Children.Add(sp);
                finishCount += 1;
            }
            
        }
        private void lblMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataObject data = new DataObject();
            data.SetData("Object", sender);
            
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move);

        }
        private void txtTargetDrop(object sender, DragEventArgs e)
        {
            
            UIElement element = (UIElement)e.Data.GetData("Object");
            StackPanel stackPanel = (StackPanel)VisualTreeHelper.GetParent(element);
            stackPanel.Children.Remove(element);

            Label label = (Label)element;
            
            ((StackPanel)sender).Children.Add(label);
        }
    }
}
