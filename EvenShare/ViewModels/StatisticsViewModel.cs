using EvenShare.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EvenShare
{
    public class StatisticsViewModel : ViewModelBase
    {
        private List<Member> _memberList;
        private List<Expense> _expenseList;

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

        public ObservableCollection<string> IndividualTotals { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Shares { get; set; } = new ObservableCollection<string>();

        public StatisticsViewModel(Project project)
        {
            ProjectContext = project;
        }

        public async Task Init()
        {                     
            _expenseList = await App.Database.GetExpensesAsync(ProjectContext);
            _memberList = await App.Database.GetMembersAsync(ProjectContext);

            // The statistic operations not only calculate display properties,
            // they also work on properties of the listitems that are needed in
            // different steps of the calculation. Therefore these lists can 
            // not be handed over as inputs.
            Total = CalcTotal();
            IndividualTotals = CalcIndividualTotals();
            Shares = CalcShares();
        }

        private int CalcTotal()
        {
            int total = 0;

            foreach (Expense expense in _expenseList)
            {
                total = total + expense.Amount;
            }

            return total;
        } 

        private ObservableCollection<string> CalcIndividualTotals()
        {
            var individualTotals = new ObservableCollection<string>();

            foreach (Member member in _memberList)
            {
                foreach(Expense expense in _expenseList)
                {
                    if (expense.Member == member.Name)
                    {
                        member.PersonalTotal = member.PersonalTotal + expense.Amount;
                    }
                }
                
                individualTotals.Add(member.Name + ": " + ((double)member.PersonalTotal / 100).ToString().Replace(",","."));
            }

            return individualTotals;
        }

        private ObservableCollection<string> CalcShares()
        {
            var shares = new ObservableCollection<string>();

            // Determine if person is in debt or not
            var debitors = new List<Member>();
            var creditors = new List<Member>();

            foreach (Member member in _memberList)
            {
                member.DiffToEvenShare = member.PersonalTotal - (Total / _memberList.Count);

                if (member.DiffToEvenShare > 0)
                {
                    creditors.Add(member);
                }
                else
                {
                    debitors.Add(member);
                }
            }

            // Go through all creditors and debitors and determine share
            while (creditors.Count > 0 && debitors.Count > 0)
            {
                var creditor = creditors[0];
                var debitor = debitors[0];

                var diff = creditor.DiffToEvenShare + debitor.DiffToEvenShare;

                if (diff == 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(debitor.DiffToEvenShare)) / 100;
                    shares.Add(
                        debitor.Name 
                        + AppResources.StatisticsFrag1
                        + debt.ToString().Replace(",", ".") 
                        + AppResources.StatisticsFrag2
                        + creditor.Name);

                    debitors.Remove(debitor);
                    creditors.Remove(creditor);
                }
                if (diff > 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(debitor.DiffToEvenShare)) / 100;
                    shares.Add(
                        debitor.Name 
                        + AppResources.StatisticsFrag1 
                        + debt.ToString().Replace(",", ".") 
                        + AppResources.StatisticsFrag2
                        + creditor.Name);

                    creditor.DiffToEvenShare = creditor.DiffToEvenShare + debitor.DiffToEvenShare;

                    creditors[0] = creditor;

                    debitors.Remove(debitor);
                }
                if (diff < 0)
                {
                    var debt = Math.Abs(Convert.ToDecimal(creditor.DiffToEvenShare)) / 100;
                    shares.Add(
                        debitor.Name 
                        + AppResources.StatisticsFrag1
                        + debt.ToString().Replace(",", ".") 
                        + AppResources.StatisticsFrag2
                        + creditor.Name);

                    debitor.DiffToEvenShare = debitor.DiffToEvenShare + creditor.DiffToEvenShare;

                    debitors[0] = debitor;

                    creditors.Remove(creditor);
                }
            }

            return shares;
        }
    }
}
