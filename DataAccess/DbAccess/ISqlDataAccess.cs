using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default");
}
