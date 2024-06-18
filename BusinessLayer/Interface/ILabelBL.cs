using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILabelBL
    {
        LabelEntity AddLabel(LabelModel model);
        LabelEntity DeleteLabel(int id);
        List<LabelEntity> GetAllLabels();
        LabelEntity UpdateLabel(int id,LabelModel model);
    }
}
