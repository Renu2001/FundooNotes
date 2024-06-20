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
        Task<LabelEntity> AddLabel(LabelModel model);
        Task<LabelEntity> DeleteLabel(int id);
        Task<List<LabelEntity>> GetAllLabels();
        Task<LabelEntity> UpdateLabel(int id,LabelModel model);
    }
}
