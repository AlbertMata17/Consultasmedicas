//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpointLiteVersion.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HosHistoriaClinica
    {
        public int idHistorio { get; set; }
        public string AntecedentesMedicos { get; set; }
        public string antecedentesGinecologico { get; set; }
        public Nullable<int> idpaciente { get; set; }
        public string alergia { get; set; }
        public string medicamentos { get; set; }
        public string Revisionporsistema { get; set; }
        public string gruposanguineo { get; set; }
        public Nullable<System.DateTime> fecnacimiento { get; set; }
        public string historia { get; set; }
        public string antsociales { get; set; }
        public string antecedentesfamiliares { get; set; }
        public string vacunas { get; set; }
        public string habitos { get; set; }
        public string estaturas { get; set; }
        public string unidad { get; set; }
        public string peso { get; set; }
        public string cantidadpeso { get; set; }
        public string temperatura { get; set; }
        public string medidatemperatura { get; set; }
        public string motivoconsulta { get; set; }
        public Nullable<int> Estatus { get; set; }
        public Nullable<int> Empresaid { get; set; }
        public Nullable<int> Usuarioid { get; set; }
    
        public virtual HosEmpresa HosEmpresa { get; set; }
        public virtual HosLogin HosLogin { get; set; }
    }
}