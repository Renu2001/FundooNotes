using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        LabelEntity AddLabel(LabelModel model,int id);
        LabelEntity DeleteLabel(int id);
        List<LabelEntity> GetAllLabels();
        LabelEntity UpdateLabel(int id ,LabelModel model);
    }
}
