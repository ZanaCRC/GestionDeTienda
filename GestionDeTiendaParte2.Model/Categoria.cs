using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public enum Categoria
    {
        [Display(Name="Clase A: Artículos caros y de alta gama con controles estrictos e inventarios reducidos")]
        ClaseA,
        [Display(Name="Clase B: Artículos de precio medio, de prioridad media, con un volumen de ventas y unas existencias medias")]
        ClaseB,
        [Display(Name="Clase C: Artículos de bajo valor y bajo coste con grandes ventas y enormes inventarios ")]
        ClaseC
    }
}
