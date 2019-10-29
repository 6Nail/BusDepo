using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DPO
{
  public class MechanicRepository
  {
    public readonly string connectionString;
    public MechanicRepository(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public void Add(Mechanic mechanic)
    {
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Insert(mechanic);
      }
    }
    public void Update(Mechanic mechanic)
    {
      string sql = "update Mechanics set Id = @Id, Name = @Name, BusId = @BusId " +
        "where Id = @id;";

      using (var connection = new SqlConnection(connectionString))
      {
        //var transaction = connection.BeginTransaction();
        //connection.Execute(sql, new
        //{
        //  Id = mechanic.Id,
        //  Status = mechanic.Name,
        //  Condition = mechanic.BusId,
        //  id = id
        //}, transaction);
        connection.Update(mechanic);
      }
    }
    public ICollection<Mechanic> GetAll()
    {
      string sql = "select * from Mechanics;";
      using (var connection = new SqlConnection(connectionString))
      {
        return connection.Query<Mechanic>(sql).ToList();
      }
    }
  }
}
