using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EvenShare
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command GoToAddProject { get; }
        public Command GoToEditProject { get; }
        public Command GoToProjects { get; }
        public Command CreateNewProject { get; }
        public Command UpdateProject { get; }
        public Command OpenProject { get; }
        public Command DeleteProject { get; }
        public Command AddMember { get; }
        public Command DeleteMember { get; }

        private Project _selectedItemProject;
        public Project SelectedItemProject
        {
            get => _selectedItemProject;
            set
            {
                _selectedItemProject = value;
                OnPropertyChanged("SelectedProject");
            }
        }

        private Member _selectedItemMember;
        public Member SelectedItemMember
        {
            get => _selectedItemMember;
            set
            {
                _selectedItemMember = value;
                OnPropertyChanged("SelectedItemMember");
            }
        }

        private string _titleInput;
        public string TitleInput
        {
            get => _titleInput;
            set
            {
                _titleInput = value;
                OnPropertyChanged("TitleInput");
            }
        }

        private string _memberInput;
        public string MemberInput
        {
            get => _memberInput;
            set
            {
                _memberInput = value;
                OnPropertyChanged("MemberInput");
            }
        }

        public ObservableCollection<Project> ProjectList { get; } = new ObservableCollection<Project>();
        public ObservableCollection<Member> MemberList { get; } = new ObservableCollection<Member>();

        public ProjectViewModel()
        {
            GoToAddProject = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddProjectView(this));
            });

            GoToEditProject = new Command(async () =>
            {
                if(SelectedItemProject != null)
                {
                    TitleInput = SelectedItemProject.Title;

                    MemberList.Clear();

                    var projectMembers = await App.Database.GetMembersAsync(SelectedItemProject);
                    foreach(Member member in projectMembers)
                    {
                        MemberList.Add(member);
                    }

                    await Application.Current.MainPage.Navigation.PushAsync(new EditProjectView(this));
                }
            });

            CreateNewProject = new Command(async () =>
            {
                if (TitleInput != null && MemberList.Count > 0)
                {
                    // Create a project
                    var project = new Project();
                    project.Title = TitleInput;
                    project.Timestamp = DateTime.Now.ToString();

                    // Create info string
                    foreach (Member member in MemberList)
                    {
                        if (project.Members != null)
                        {
                            project.Members = project.Members + ", " + member.Name;
                        }
                        else
                        {
                            project.Members = member.Name;
                        }

                    }

                    // Add project to the database
                    var projectID = await App.Database.AddProjectAsync(project);

                    // Add members to the database
                    foreach (Member member in MemberList)
                    {
                        member.ProjectID = projectID;
                        await App.Database.AddMemberAsync(member);
                    }

                    ProjectList.Add(project);

                    Reset();

                    await Application.Current.MainPage.Navigation.PopAsync();                                    
                }
            });

            UpdateProject = new Command(async () =>
            {
                if (TitleInput != null && MemberList.Count > 0)
                {
                    var index = ProjectList.IndexOf(SelectedItemProject);

                    var newProject = new Project();

                    newProject.ID = SelectedItemProject.ID;

                    newProject.Title = TitleInput;
                    
                    // Update info string and member database
                    foreach (Member member in MemberList)
                    {
                        if (newProject.Members != null)
                        {
                            newProject.Members = newProject.Members + ", " + member.Name;
                        }
                        else
                        {
                            newProject.Members = member.Name;
                        }

                        if(!await App.Database.MemberExists(member))
                        {
                            member.ProjectID = SelectedItemProject.ID;
                            await App.Database.AddMemberAsync(member);
                        }
                    }

                    await App.Database.UpdateProjectAsync(newProject);

                    ProjectList.RemoveAt(index);
                    ProjectList.Insert(index, newProject);

                    Reset();

                    await Application.Current.MainPage.Navigation.PopAsync();                    
                }
            });

            OpenProject = new Command(async () =>
            {
                if(SelectedItemProject != null)
                {
                    var expenseViewModel = new ExpenseViewModel(SelectedItemProject);
                    await Application.Current.MainPage.Navigation.PushAsync(new ExpenseView(expenseViewModel));
                }
            });

            DeleteProject = new Command(async () =>
            {
                if (SelectedItemProject != null)
                {
                    await App.Database.DeleteProjectAsync(SelectedItemProject);
                    ProjectList.Remove(SelectedItemProject);
                    SelectedItemProject = null;
                }
            });

            GoToProjects = new Command(async () =>
            {
                Reset();

                await Application.Current.MainPage.Navigation.PopAsync();              
            });

            AddMember = new Command(() =>
            {
                if(MemberInput != null && MemberInput != "")
                {
                    var member = new Member();
                    member.Name = MemberInput;
                    MemberList.Add(member);
                    MemberInput = "";
                }
            });

            DeleteMember = new Command(async () =>
            {
                if(SelectedItemMember != null)
                {
                    MemberList.Remove(SelectedItemMember);
                    await App.Database.DeleteMembersAsync(SelectedItemMember, SelectedItemProject);
                }
            });
        }

        public async Task Init()
        {         
            var projects = await App.Database.GetProjectsAsync();
            foreach(Project project in projects)
            {
                ProjectList.Add(project);
            }
        }

        public void Reset()
        {
            MemberList.Clear();
            MemberInput = "";
            TitleInput = "";
        }
    }
}
