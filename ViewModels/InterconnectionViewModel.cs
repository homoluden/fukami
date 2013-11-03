using CustomBodies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fukami.ViewModels
{
    public class InterconnectionViewModel : BaseGeneViewModel<InterconnectionModel>
    {
        public override InterconnectionModel GetModelDuplicate()
        {
            return Model.Duplicate();
        }
    }
}
