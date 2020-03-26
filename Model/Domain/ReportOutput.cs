using System;
using System.Collections.Generic;

namespace GloEpidBot.Model.Domain
{
    public class ReportOutput
    {  
      public int Count { get; set; }  
      public List<ReportInfo> Items { get; set; }  
    }  
  
  public class ReportInfo  
  {  
      public int Id { get; set; }  
      public string Symptoms { get; set; }  
      public string Location { get; set; }  
      public string RiskStatus { get; set; }  
      public string ReporterName { get; set; } 
      public int Age { get; set; } 
      public DateTime LastReadAt { get; set; }  
      public DateTime DateReported { get; set; }
  } 

}