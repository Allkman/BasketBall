using BasketBall.Client.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Shared
{
    public partial class MultipleSelectorComp
    {
        private string removeAllText = "<<"; //compiler does not allow <<
        [Parameter]
        public List<MultipleSelectorStruct> NoSelected { get; set; } =
            new List<MultipleSelectorStruct>();
        [Parameter]
        public List<MultipleSelectorStruct> Selected { get; set; } =
            new List<MultipleSelectorStruct>();

        private void Select(MultipleSelectorStruct item)
        {
            NoSelected.Remove(item);
            Selected.Add(item);
        }

        private void Deselect(MultipleSelectorStruct item)
        {
            Selected.Remove(item);
            NoSelected.Add(item);
        }

        private void SelectAll()
        {
            Selected.AddRange(NoSelected);
            NoSelected.Clear();
        }

        private void DeselectAll()
        {
            NoSelected.AddRange(Selected);
            Selected.Clear();
        }
    }
}
