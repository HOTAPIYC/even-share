using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace EvenShare
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            try
            {
                _database.CreateTableAsync<Project>().Wait();
                _database.CreateTableAsync<Expense>().Wait();
                _database.CreateTableAsync<Member>().Wait();
            }
            catch
            {
                // Error
            }

        }

        // Projects
        public async Task<List<Project>> GetProjectsAsync()
        {
            try
            {
                return await _database.Table<Project>().ToListAsync();
            }
            catch
            {
                return await Task.FromResult(new List<Project>());
            }
        }
        public async Task<int> AddProjectAsync(Project project)
        {
            var maxId = 0;

            try
            {
                await _database.InsertAsync(project);

                var result = await _database.QueryAsync<Project>("SELECT * FROM Project");

                foreach (Project item in result)
                {
                    if (item.ID > maxId) { maxId = item.ID; }
                }
            }
            catch
            {
                // Error
            }

            return maxId;
        }
        public async Task UpdateProjectAsync(Project project)
        {
            try
            {
                await _database.UpdateAsync(project);
            }
            catch
            {
                // Error
            }

        }
        public async Task DeleteProjectAsync(Project project)
        {
            try
            {
                await _database.DeleteAsync(project);

                string queryMember = "DELETE FROM Member WHERE ProjectID=\"" + project.ID.ToString() + "\";";
                await _database.QueryAsync<Member>(queryMember);

                string queryExpense = "DELETE FROM Expense WHERE ProjectID=\"" + project.ID.ToString() + "\";";
                await _database.QueryAsync<Member>(queryExpense);
            }
            catch
            {
                // Error
            }

        }

        // Expenses
        public async Task<List<Expense>> GetExpensesAsync(Project project)
        {
            try
            {
                string query = "SELECT * FROM Expense WHERE ProjectID=\"" + project.ID.ToString() + "\";";

                return await _database.QueryAsync<Expense>(query);
            }
            catch
            {
                return await Task.FromResult(new List<Expense>());
            }
        }
        public async Task AddExpenseAsync(Expense expense)
        {
            try
            {
                await _database.InsertAsync(expense);
            }
            catch
            {
                // Error
            }
        }
        public async Task UpdateExpenseAsync(Expense expense)
        {
            try
            {
                await _database.UpdateAsync(expense);
            }
            catch
            {
                // Error
            }
        }
        public async Task DeleteExpenseAsync(Expense expense)
        {
            try
            {
                await _database.DeleteAsync(expense);
            }
            catch
            {
                // Error
            }
        }

        // Members
        public async Task<List<Member>> GetMembersAsync(Project project)
        {
            try
            {
                string query = "SELECT * FROM Member WHERE ProjectID=\"" + project.ID.ToString() + "\";";

                return await _database.QueryAsync<Member>(query);
            }
            catch
            {
                return await Task.FromResult(new List<Member>());
            }
        }
        public async Task AddMemberAsync(Member member)
        {
            try
            {
                await _database.InsertAsync(member);
            }
            catch
            {
                // Error
            }
        }
        public async Task DeleteMembersAsync(Member member, Project project)
        {
            try
            {
                await _database.DeleteAsync(member);

                string queryExpense = "DELETE FROM Expense WHERE ProjectID=\"" + project.ID.ToString() + "\" AND Member=\"" + member.Name + "\";";
                await _database.QueryAsync<Member>(queryExpense);
            }
            catch
            {
                // Error
            }
        }
        public async Task<bool> MemberExists(Member member)
        {
            try
            {
                string query = "SELECT * FROM Member WHERE ID=\"" + member.ID.ToString() + "\";";

                var result = await _database.QueryAsync<Member>(query);

                if (result.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
                // Error
            }
        }
    }
}
