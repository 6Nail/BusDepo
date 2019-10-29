using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DPO
{
  public class BusRepository
  {
    private readonly string connectionString;
    public BusRepository(string connectionString)
    {
      this.connectionString = connectionString;
    }

    public void Add(Bus bus)
    {
      string sql = "Insert into Buses (@Id, @Status, @Condition, @MechanicId);";

      using(var connection = new SqlConnection(connectionString))
      {
        connection.Insert(bus);
      }
    }

    public void Delete(Bus bus)
    {
      
      using (var connection = new SqlConnection(connectionString))
      {
        connection.Delete(bus);
      }
    }
    public void Update(Bus bus)
    {
      string sql = "update Buses set Id = @Id, Status = @Status, Condition = @Condition, MechanicId = @MechanicId " +
        "where Id = @id;";

      using(var connection = new SqlConnection(connectionString))
      {
        //var transaction = connection.BeginTransaction();
        //connection.Execute(sql, new
        //{
        //  Id = bus.Id,
        //  Status = bus.Status,
        //  Condition = bus.Condition,
        //  MechanicId = bus.MechanicId,
        //  id = id
        //}, transaction);
        connection.Update(bus);
      }
    }
    public ICollection<Bus> GetAll()
    {
      string sql = "select * from Buss;";
      using (var connection = new SqlConnection(connectionString))
      {
        return connection.Query<Bus>(sql).ToList();
      }
    }
  }
}
