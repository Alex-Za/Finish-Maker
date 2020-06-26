using Finish_Maker.Models.FileModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using Finish_Maker.Models;
using Finish_Maker.Additional_Classes;

namespace Finish_Maker.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        FileReader fileReader;
        SettingsModel settingsModel = new SettingsModel();
        ChooseCatWindow chooseCatWindow;
        private List<string> allExportLinksFiles { get; set; }
        public SettingsViewModel(List<string> allExportLinksFiles)
        {
            this.allExportLinksFiles = allExportLinksFiles;
            fileReader = new FileReader(allExportLinksFiles);
            MassloadBrandWithSKU = new BrandWithSKU();
            FileVisibility = "Hidden";
        }
        public bool CategoryCheck
        {
            get { return settingsModel.CheckCategory; }
            set
            {
                settingsModel.CheckCategory = value;
                OnPropertyChanged("CategoryCheck");
            }
        }
        public bool SubtypeCheck
        {
            get { return settingsModel.CheckSubtype; }
            set
            {
                settingsModel.CheckSubtype = value;
                OnPropertyChanged("SubtypeCheck");
            }
        }
        private RelayCommand addMassBrandWithSKUCommand;
        private RelayCommand deleteCommand;
        private RelayCommand splitByCategoryCommand;
        private RelayCommand splitBySubtypeCommand;
        public BrandWithSKU MassloadBrandWithSKU { get; set; }
        private string fileVisibility;
        public string FileVisibility {
            get { return fileVisibility; }
            set
            {
                fileVisibility = value;
                OnPropertyChanged("FileVisibility");
            }
        }


        public RelayCommand AddMassBrandWithSKUCommand
        {
            get
            {
                return addMassBrandWithSKUCommand ??
                    (addMassBrandWithSKUCommand = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        if (openFileDialog.ShowDialog() == true)
                        {
                            foreach (string file in openFileDialog.FileNames)
                            {
                                MassloadBrandWithSKU.Path = file;
                                MassloadBrandWithSKU.ViewPath = file.Substring(file.LastIndexOf("\\") + 1);
                                MassloadBrandWithSKU.ID = 1;
                            }
                            FileVisibility = "Visible";
                        }
                    }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(obj =>
                {
                    MassloadBrandWithSKU.Path = "";
                    MassloadBrandWithSKU.ViewPath = "";
                    FileVisibility = "Hidden";
                }));
            }
        }
        public RelayCommand SplitByCategoryCommand
        {
            get
            {
                return splitByCategoryCommand ?? (splitByCategoryCommand = new RelayCommand(obj =>
                {
                    if (allExportLinksFiles.Count > 0)
                    {
                        if (FinishFileData.ChooseCategoryStatus == false)
                        {
                            List<List<string>> categoryData = fileReader.CategoryData;

                            chooseCatWindow = new ChooseCatWindow(categoryData, "categorys");
                            chooseCatWindow.Show();
                        }
                        else
                        {
                            chooseCatWindow.Show();
                        }
                        CategoryCheck = true;
                        SubtypeCheck = false;
                    }
                    else
                    {
                        MessageBox.Show("Не выбран файл експорт линков");
                        CategoryCheck = false;
                        return;
                    }

                    
                }));
            }
        }
        public RelayCommand SplitBySubtypeCommand
        {
            get
            {
                return splitBySubtypeCommand ?? (splitBySubtypeCommand = new RelayCommand(obj =>
                {
                    if (allExportLinksFiles.Count > 0)
                    {
                        if (FinishFileData.ChooseCategoryStatus == false)
                        {
                            List<List<string>> categoryData = fileReader.CategoryData;

                            chooseCatWindow = new ChooseCatWindow(categoryData, "subtypes");
                            chooseCatWindow.Show();
                        }
                        else
                        {
                            chooseCatWindow.Show();
                        }
                        SubtypeCheck = true;
                        CategoryCheck = false;
                    }
                    else
                    {
                        MessageBox.Show("Не выбран файл експорт линков");
                        SubtypeCheck = false;
                        return;
                    }


                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
