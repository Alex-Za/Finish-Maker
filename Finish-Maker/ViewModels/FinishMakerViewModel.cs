using Finish_Maker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Finish_Maker.Models.FileModels;
using Finish_Maker.ViewModels.AdditionalClasses;
using Finish_Maker.Models.Interfaces;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Windows.Media;
using System.IO;
using System.Windows;

namespace Finish_Maker.ViewModels
{
    class FinishMakerViewModel : INotifyPropertyChanged
    {
        FinishMakerModel finishMakerModel = new FinishMakerModel();
        private BackgroundWorker worker;
        Dispatcher dispatcher;

        public FinishMakerViewModel()
        {
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += workerDoWork;
            worker.ProgressChanged += workerProgressChanged;

            dispatcher = Dispatcher.CurrentDispatcher;

            ExportLinksList = new ObservableCollection<ExportLinks>();
            OldExportLinksList = new ObservableCollection<OldExportLinks>();
            ChtDuplicatesList = new ObservableCollection<ChildTitleDuplicates>();

            StartButton = "Run";
            UserName = "User Name";

        }



        private void workerDoWork(object sender, DoWorkEventArgs e)
        {
            if (!CheckIfNotFirstStart())
            {
                return;
            }

            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.SelectedPath = Directory.GetCurrentDirectory();

            dispatcher.BeginInvoke(new Action(() =>
            {
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ConsoleTextProperty = new ConsoleText { TheColor = Brushes.White, TheText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum." };
                }
            }));




        }


        public string UserName
        {
            get { return finishMakerModel.UserName; }
            set
            {
                finishMakerModel.UserName = value;
                OnPropertyChanged("UserName");
            }
        }
        public int Progress
        {
            get { return finishMakerModel.Progress; }
            set
            {
                finishMakerModel.Progress = value;
                OnPropertyChanged("Progress");
            }
        }
        public string StartButton
        {
            get { return finishMakerModel.StartButton; }
            set
            {
                finishMakerModel.StartButton = value;
                OnPropertyChanged("StartButton");
            }
        }
        public ObservableCollection<ExportLinks> ExportLinksList { get; set; }
        public ObservableCollection<OldExportLinks> OldExportLinksList { get; set; }
        public ObservableCollection<ChildTitleDuplicates> ChtDuplicatesList { get; set; }

        private RelayCommand addExportLinksCommand;
        private RelayCommand addOldExportLinksCommand;
        private RelayCommand addChtDuplicatesCommand;
        private RelayCommand startCommand;
        private RelayCommand deleteCommand;
        private RelayCommand fullScreenCommand;
        private RelayCommand openSettingsCommand;
        private ConsoleText consoleTextProperty;
        public RelayCommand AddExportLinksCommand
        {
            get
            {
                return addExportLinksCommand ??
                    (addExportLinksCommand = new RelayCommand(obj =>
                    {
                        AddFile("ExportLinks");
                    }));
            }
        }
        public RelayCommand AddOldExportLinksCommand
        {
            get
            {
                return addOldExportLinksCommand ??
                    (addOldExportLinksCommand = new RelayCommand(obj =>
                    {
                        AddFile("OldExportLinks");
                    }));
            }
        }
        public RelayCommand AddChtDuplicatesCommand
        {
            get
            {
                return addChtDuplicatesCommand ??
                    (addChtDuplicatesCommand = new RelayCommand(obj =>
                    {
                        AddFile("ChtDuplicates");
                    }));
            }
        }
        public RelayCommand Start
        {
            get
            {
                return startCommand ??
                    (startCommand = new RelayCommand(obj =>
                    {
                        worker.RunWorkerAsync();
                    }));
            }
        }
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand(obj =>
                {
                    UploadedFile currentFile = obj as UploadedFile;
                    if (currentFile != null)
                    {
                        if (currentFile is ExportLinks)
                        {
                            ExportLinksList.Remove(currentFile as ExportLinks);
                        }
                        else if (currentFile is OldExportLinks)
                        {
                            OldExportLinksList.Remove(currentFile as OldExportLinks);
                        }
                        else if (currentFile is ChildTitleDuplicates)
                        {
                            ChtDuplicatesList.Remove(currentFile as ChildTitleDuplicates);
                        }
                    }
                }));
            }
        }
        public RelayCommand OpenSettingsCommand
        {
            get
            {
                return openSettingsCommand ?? (openSettingsCommand = new RelayCommand(obj =>
                {
                    List<string> allExportLinksFiles = new List<string>();
                    if (ExportLinksList.Count > 0)
                    {
                        foreach (ExportLinks item in ExportLinksList)
                        {
                            allExportLinksFiles.Add(item.Path);
                        }
                    }
                    
                    SettingsWindow settingsWindow = new SettingsWindow(allExportLinksFiles);
                    


                    settingsWindow.Show();
                }));
            }
        }
        public RelayCommand FullScreenCommand
        {
            get
            {
                return fullScreenCommand ?? (fullScreenCommand = new RelayCommand(obj =>
                {
                    ConsoleWindow consoleWindow = new ConsoleWindow();
                    
                    if (ConsoleTextProperty != null)
                    {
                        consoleWindow.ConsoleText = ConsoleTextProperty.TheText;
                        consoleWindow.Color = ConsoleTextProperty.TheColor;
                    }
                    else
                    {
                        consoleWindow.ConsoleText = "";
                        consoleWindow.Color = Brushes.White;
                    }
                    consoleWindow.Show();
                }));
            }
        }
        public ConsoleText ConsoleTextProperty
        {
            get { return consoleTextProperty; }
            set
            {
                consoleTextProperty = value;
                OnPropertyChanged("ConsoleTextProperty");
            }
        }


        private void AddFile(string currentFile)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { Multiselect = true };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (file!=null)
                    {
                        if (currentFile == "ExportLinks")
                        {
                            ExportLinks exportLinks = new ExportLinks();
                            exportLinks.Path = file;
                            exportLinks.ViewPath = file.Substring(file.LastIndexOf("\\") + 1);
                            exportLinks.ID = ExportLinksList.Count + 1;
                            ExportLinksList.Add(exportLinks);
                        }
                        if (currentFile == "OldExportLinks")
                        {
                            OldExportLinks oldExportLinks = new OldExportLinks();
                            oldExportLinks.Path = file;
                            oldExportLinks.ViewPath = file.Substring(file.LastIndexOf("\\") + 1);
                            oldExportLinks.ID = OldExportLinksList.Count + 1;
                            OldExportLinksList.Add(oldExportLinks);
                        }
                        if (currentFile == "ChtDuplicates")
                        {
                            ChildTitleDuplicates chtDuplicates = new ChildTitleDuplicates();
                            chtDuplicates.Path = file;
                            chtDuplicates.ViewPath = file.Substring(file.LastIndexOf("\\") + 1);
                            chtDuplicates.ID = ChtDuplicatesList.Count + 1;
                            ChtDuplicatesList.Add(chtDuplicates);
                        }
                    }
                }
            }
        }
        private bool CheckIfNotFirstStart()
        {
            if (StartButton == "Clear")
            {
                changeProgress(0);
                dispatcher.BeginInvoke(new Action(() =>
                {
                    ExportLinksList.Clear();
                    OldExportLinksList.Clear();
                    ChtDuplicatesList.Clear();
                }));
                StartButton = "Run";
                ConsoleTextProperty = new ConsoleText { TheColor = Brushes.Black, TheText = "" };
                return false;
            }
            else
            {
                return true;
            }
        }
        private void changeProgress(int count)
        {
            this.worker.ReportProgress(count);
        }
        void workerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.ProgressPercentage;
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
