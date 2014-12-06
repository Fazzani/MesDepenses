using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MesDepenses.Models
{
  [DataContract]
  [System.Diagnostics.DebuggerDisplay("History = {History}")]
  public class OperationModel
  {
    [DataMember(Name = "compte")]
    public long Compte { get; set; }
    [DataMember(Name = "libelle")]
    public string Libelle { get; set; }
    [DataMember(Name = "montant")]
    public decimal Montant { get; set; }
    [DataMember(Name = "datedecomptabilisation")]
    public string DateComptabilisation { get; set; }
    [DataMember(Name = "datevaleur")]
    public string DateValeur { get; set; }
    [DataMember(Name = "dateoperation")]
    public string DateOperation { get; set; }
    [DataMember(Name = "reference")]
    public string Reference { get; set; }
    [DataMember(Name = "history")]
    public History History { get; set; }

  }

  [System.Diagnostics.DebuggerDisplay("CreationDate = {CreationDate}")]
  public class History
  {
    public DateTime CreationDate { get; set; }
    public bool IsNew { get; set; }
    public string OperationName { get; set; }
  }
}