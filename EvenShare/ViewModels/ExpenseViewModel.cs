using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace EvenShare
{
    public class ExpenseViewModel : INotifyPropertyChanged
    {
        private Project _projectContext;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command GoToAddExpense { get; }
        public Command GoToEditExpense { get; }
        public Command GoToStatistics { get; }
        public Command GoToExpenses { get; }
        public Command DeleteExpense { get; }
        public Command CreateNewExpense { get; }
        public Command UpdateExpense { get; }

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

        private string _amountInput;
        public string AmountInput
        {
            get => _amountInput;
            set
            {
                _amountInput = value;
                OnPropertyChanged("AmountInput");
            }
        }

        private int _selectedIndexMember;
        public int SelectedIndexMember
        {
            get => _selectedIndexMember;
            set
            {
                _selectedIndexMember = value;
                OnPropertyChanged("SelectedIndexMember");
            }
        }

        private Expense _selectedItemExpense;
        public Expense SelectedItemExpense
        {
            get => _selectedItemExpense;
            set
            {
                _selectedItemExpense = value;
                OnPropertyChanged("SelectedItemExpense");
            }
        }

        public ObservableCollection<string> MemberList { get; } = new ObservableCollection<string>();
        public ObservableCollection<Expense> ExpenseList { get; } = new ObservableCollection<Expense>();

        public ExpenseViewModel(Project project)
        {
            _projectContext = project;

            GoToAddExpense = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new AddExpenseView(this));
            });

            GoToStatistics = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new StatisticsView(new StatisticsViewModel(_projectContext)));
            });

            GoToEditExpense = new Command(async () =>
            {
                if(SelectedItemExpense != null)
                {
                    TitleInput = SelectedItemExpense.Title;
                    AmountInput = ((double)SelectedItemExpense.Amount / 100).ToString();
                    SelectedIndexMember = MemberList.IndexOf(SelectedItemExpense.Member);

                    await Application.Current.MainPage.Navigation.PushAsync(new EditExpenseView(this));
                }
            });

            GoToExpenses = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();

                Reset();
            });

            DeleteExpense = new Command(async () =>
            {
                if(SelectedItemExpense != null)
                {
                    await App.Database.DeleteExpenseAsync(SelectedItemExpense);
                    ExpenseList.Remove(SelectedItemExpense);
                }
            });

            CreateNewExpense = new Command(async () =>
            {
                if (TitleInput != null && AmountInput != null && SelectedIndexMember >= 0)
                {
                    var expense = new Expense();

                    expense.Title = TitleInput;
                    expense.Amount = Convert.ToInt32(Math.Round(double.Parse(AmountInput.Replace(',', '.')) * 100, 0));
                    expense.Member = MemberList[SelectedIndexMember];
                    expense.ProjectID = _projectContext.ID;

                    await App.Database.AddExpenseAsync(expense);
                    ExpenseList.Add(expense);

                    await Application.Current.MainPage.Navigation.PopAsync();

                    Reset();
                }
            });

            UpdateExpense = new Command(async () =>
            {
                if (TitleInput != null && AmountInput != null && SelectedIndexMember >= 0)
                {
                    var index = ExpenseList.IndexOf(SelectedItemExpense);

                    var newExpense = new Expense();

                    newExpense.ID = SelectedItemExpense.ID;
                    newExpense.ProjectID = SelectedItemExpense.ProjectID;

                    newExpense.Title = TitleInput;
                    newExpense.Amount = Convert.ToInt32(Math.Round(double.Parse(AmountInput.Replace(',', '.')) * 100, 0));
                    newExpense.Member = MemberList[SelectedIndexMember];                  

                    await App.Database.UpdateExpenseAsync(newExpense);

                    ExpenseList.RemoveAt(index);
                    ExpenseList.Insert(index, newExpense);

                    await Application.Current.MainPage.Navigation.PopAsync();

                    Reset();
                }
            });
        }

        public async Task Init()
        {
            var expenses = await App.Database.GetExpensesAsync(_projectContext);
            foreach (Expense expense in expenses)
            {
                ExpenseList.Add(expense);
            }
            
            var projectMembers = await App.Database.GetMembersAsync(_projectContext);
            foreach (Member member in projectMembers)
            {
                MemberList.Add(member.Name);
            }

            SelectedIndexMember = -1;
        }

        public void Reset()
        {
            TitleInput = "";
            AmountInput = "";
            SelectedIndexMember = -1;
        }
    }
}
