using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorldControllers;

namespace Renderers
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, IDisposable, IRenderer
    {
        public UserControl1()
        {
            InitializeComponent();

            Representation.Instance.RegisterRenderer(this as IRenderer);
        }

        public void Dispose()
        {
            Representation.Instance.UnregisterRenderer(this as IRenderer);
        }
    }
}
