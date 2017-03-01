using GalaSoft.MvvmLight;
using GridWithEFCore.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GalaSoft.MvvmLight.Command;

namespace GridWithEFCore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private DatabaseContext context = new DatabaseContext();

        public ObservableCollection<Session> Sessions { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                base.RaisePropertyChanged();
            }
        }

        private string databasePath;
        public string DatabasePath
        {
            get { return databasePath; }
            set
            {
                databasePath = value;
                base.RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }
        public RelayCommand<Session> DeleteCommand { get; set; }

        public MainViewModel()
        {
            this.SaveCommand = new RelayCommand(saveCommandExecute);
            this.AddNewCommand = new RelayCommand(addNewCommandExecute);
            this.DeleteCommand = new RelayCommand<Session>(deleteCommandExecute);

            this.InitDatabase();
            this.DatabasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }

        private void deleteCommandExecute(Session obj)
        {
            context.Sessions.Remove(obj);
            this.Sessions.Remove(obj);
        }

        private async void addNewCommandExecute()
        {
            Session newSess = new Session();
            newSess.Abtract = string.Empty;
            newSess.Category = this.Categories.FirstOrDefault();
            newSess.MinutesDuration = 45;
            newSess.Speaker = string.Empty;
            newSess.Start = DateTime.Now;
            newSess.Title = string.Empty;

            await context.Sessions.AddAsync(newSess);
            this.Sessions.Insert(0, newSess);
        }

        private async void saveCommandExecute()
        {
            if (context.ChangeTracker.HasChanges())
            {
                this.IsBusy = true;
                await context.SaveChangesAsync();
                this.IsBusy = false;
            }
        }

        private async void InitDatabase()
        {
            this.IsBusy = true;
            var pending = await context.Database.GetPendingMigrationsAsync();

            if (pending.Any())
            {
                await context.Database.MigrateAsync();
            }

            var list = context.Sessions.OrderByDescending(s => s.Start);
            var categories = context.Categories.OrderBy(s => s.Name);
            this.Sessions = new ObservableCollection<Session>(list);
            this.Categories = new ObservableCollection<Category>(categories);
            base.RaisePropertyChanged(nameof(Categories));

            if (!this.Categories.Any())
            {
                string[] names = new string[] { "UWP", ".NET", "ASP.NET MVC", "HTML5", "C#" };

                foreach (string s in names)
                {
                    Category cat = new Category() { Name = s };

                    this.Categories.Add(cat);
                    context.Categories.Add(cat);
                }

                await context.SaveChangesAsync();
            }

            if (!this.Sessions.Any())
            {
                Session session = new Session();
                session.Abtract = "";
                session.MinutesDuration = 45;
                session.Speaker = "Igor Damiani";
                session.Start = new DateTime(2017, 04, 01, 11, 00, 00);
                session.Title = "UWP con Visual Studio 2017";
                session.Category = this.Categories.FirstOrDefault();

                this.Sessions.Add(session);
                context.Sessions.Add(session);
                await context.SaveChangesAsync();
            }

            this.IsBusy = false;
        }
    }
}