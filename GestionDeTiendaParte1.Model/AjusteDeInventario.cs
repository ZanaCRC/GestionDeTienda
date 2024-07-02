﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.Model
{
    public class AjusteDeInventario
    {
        public int Id { get; set; }

        [ForeignKey("Inventario")]
        public int Id_Inventario { get; set; }
        public Inventario Inventario { get; set; }
        public int CantidadActual { get; set; }
        [Required(ErrorMessage = "El Ajuste es requerido.")]
        public int Ajuste { get; set; }
        public TipoInventario Tipo { get; set; }
        [Required(ErrorMessage = "Las Observaciones es requerido.")]
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
