//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace prbd_1617_G03
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public int numS { get; set; }
        public int numC { get; set; }
        public int numCat { get; set; }
        public int nbr { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Client Client { get; set; }
        public virtual Show Show { get; set; }
    }
}
