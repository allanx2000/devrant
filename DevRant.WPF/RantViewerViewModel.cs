using DevRant.WPF.ViewModels;
using System;

namespace DevRant.WPF
{
    internal class RantViewerViewModel
    {
        private IDevRantClient api;
        
        public RantViewerViewModel(Rant rant, IDevRantClient api)
        {
            Rant = rant;
            this.api = api;

            GetComments();
        }

        public Rant Rant { get; set; } //TODO: Make private, check DP

        private void GetComments()
        {
            //throw new NotImplementedException();
        }
    }
}