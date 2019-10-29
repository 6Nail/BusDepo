using System;
using System.Collections.Generic;
using System.Text;

namespace DPO
{
  public class Bus
  {
    public int Id { get; set; } 
    public int Status { get; set; }
    public string Condition { get; set; }
    public int? MechanicId { get; set; }
  }
}
