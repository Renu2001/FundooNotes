using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private ILabelRL _labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            _labelRL = labelRL;
        }
        public LabelEntity AddLabel(LabelModel model)
        {
            try
            {
                return _labelRL.AddLabel(model);
            }
            catch
            {
                throw;
            }
        }

        public LabelEntity DeleteLabel(int id)
        {
            try
            {
                return _labelRL.DeleteLabel(id);
            }
            catch
            {
                throw;
            }
        }

        public List<LabelEntity> GetAllLabels()
        {
            try
            {
                return _labelRL.GetAllLabels();
            }
            catch
            {
                throw;
            }
        }

        public LabelEntity UpdateLabel(int id ,LabelModel model)
        {
            try
            {
                return _labelRL.UpdateLabel(id,model);
            }
            catch
            {
                throw;
            }
        }
    }
}
