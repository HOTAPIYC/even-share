using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EvenShare
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private Project _projectContext;
        private List<Member> _participants;
        private List<Expense> _expenses;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _total;
        public int Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged("Total");
            }
        }

        private ObservableCollection<string> _individualTotals;
        public ObservableCollection<string> IndividualTotals
        {
            get => _individualTotals;
            set
            {
                _individualTotals = value;
                OnPropertyChanged("IndividualTotals");
            }
        }

        private ObservableCollection<string> _results;
        public ObservableCollection<string> Results
        {
            get => _results;
            set
            {
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        public StatisticsViewModel(Project project)
        {
            _projectContext = project;
        }

        public async Task Init()
        {                     
            // Get current values from the database
            _expenses = await App.Database.GetExpensesAsync(_projectContext);
            _participants = await App.Database.GetMembersAsync(_projectContext);

            // Calculate the total and add up how much everyone spent
            foreach(Expense expense in _expenses)
            {
                Total = Total + expense.Amount;
                
                foreach (Member participant in _participants)
                {                  
                    if(expense.Member == participant.Name)
                    {
                        _participants[_participants.IndexOf(participant)].PersonalTotal
                            = _participants[_participants.IndexOf(participant)].PersonalTotal + expense.Amount;
                    }                   
                }            
            }

            // Create overview strings
            IndividualTotals = new ObservableCollection<string>();
            foreach (Member participant in _participants)
            {
                IndividualTotals.Add(participant.Name + ": " + ((double)participant.PersonalTotal / 100));
            }

            // Determine if person is in debt or not
            var debitors = new List<Member>();
            var creditors = new List<Member>();

            foreach (Member participant in _participants)
            {
                participant.DiffToEvenShare = participant.PersonalTotal - (Total / _participants.Count);

                if(participant.DiffToEvenShare > 0)
                {
                    creditors.Add(participant);
                }
                else
                {
                    debitors.Add(participant);
                }
            }

            // Go through all creditors and debitors and determine share
            Results = new ObservableCollection<string>();

            while(creditors.Count > 0 && debitors.Count > 0)
            {
                var creditor = creditors[0];
                var debitor = debitors[0];

                var diff = creditor.DiffToEvenShare + debitor.DiffToEvenShare;

                if(diff == 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(debitor.DiffToEvenShare)) / 100;
                    Results.Add(debitor.Name + " ows " + debt.ToString() + " to " + creditor.Name);

                    debitors.Remove(debitor);
                    creditors.Remove(creditor);
                }
                else if(diff > 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(debitor.DiffToEvenShare)) / 100;
                    Results.Add(debitor.Name + " ows " + debt.ToString() + " to " + creditor.Name);

                    creditor.DiffToEvenShare = creditor.DiffToEvenShare + debitor.DiffToEvenShare;

                    creditors[0] = creditor;

                    debitors.Remove(debitor);
                }
                else if (diff < 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(creditor.DiffToEvenShare)) / 100;
                    Results.Add(debitor.Name + " ows " + debt.ToString() + " to " + creditor.Name);

                    debitor.DiffToEvenShare = debitor.DiffToEvenShare + creditor.DiffToEvenShare;

                    debitors[0] = debitor;

                    creditors.Remove(creditor);
                }
            }
        }
    }
}
