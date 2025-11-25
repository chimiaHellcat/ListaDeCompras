using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ListaCompras.ViewModels;



namespace ListaCompras.Models
{
    public class ProdutoModel : BaseViewModel

    {
        public string Id { get; set; }
        public string Descricao { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

    }
}

