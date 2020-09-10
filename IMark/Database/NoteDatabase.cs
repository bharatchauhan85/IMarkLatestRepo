using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMark.Database
{
 public   class NoteDatabase
    {

        readonly SQLiteAsyncConnection _database;

        public NoteDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Customer_Add>().Wait();
            _database.CreateTableAsync<Customer_Sqlite>().Wait();
        }

        public Task<int> SaveNoteAsync(Customer_Add note)
        {

            if (Convert.ToString(note.Cus_id) != "0")
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }

        public Task<List<Customer_Add>> GetNoteAsync()
        {
            //  _database.CreateTableAsync<Customer_Add>().Wait();
            return _database.Table<Customer_Add>().ToListAsync();
        }
        public Task<Customer_Add> GetSingleNoteAsync()
        {
            //  _database.CreateTableAsync<Customer_Add>().Wait();
           return  _database.Table<Customer_Add>().FirstAsync();
        }

        public Task<int> DeleteNoteAsync(Customer_Add note)
        {
            //  _database.CreateTableAsync<Customer_Add>().Wait();
            return _database.DeleteAsync(note);
        }
        public Task<int> SaveCustomerAsync(Customer_Sqlite note)
        {

            if (Convert.ToString(note.id) != "0")
            {
                return _database.UpdateAsync(note);
            }
            else
            {
                return _database.InsertAsync(note);
            }
        }
        public Task<List<Customer_Sqlite>> GetCustomer()
        {
            //  _database.CreateTableAsync<Customer_Add>().Wait();
            return _database.Table<Customer_Sqlite>().ToListAsync();
        }
        public Task<int> DeleteCustomer(Customer_Sqlite note)
        {
            //  _database.CreateTableAsync<Customer_Add>().Wait();
            return _database.DeleteAsync(note);
        }


    }
}
