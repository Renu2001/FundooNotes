using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext fundooContext;
        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public LabelEntity AddLabel(LabelModel model, int id)
        {
            LabelEntity labelentity = new LabelEntity();
            labelentity.LabelName = model.LabelName;
            try
            {
                fundooContext.Labels?.Add(labelentity);
                fundooContext.SaveChanges();
                return labelentity;
            }
            catch (Exception ex)
            {
                throw new CustomizeException(ex.Message);
            }
        }

        public LabelEntity DeleteLabel(int id)
        {
            var result = fundooContext.Labels?.Find(id);
            if (result != null)
            {
                try
                {
                    fundooContext.Labels?.Remove(result);
                    fundooContext.SaveChanges();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }

            }
            else
            {
                throw new CustomizeException("No Note Found");
            }
        }

        public List<LabelEntity> GetAllLabels()
        {
            var result = fundooContext.Labels?.ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new CustomizeException("No Label Found");
            }
        }

        public LabelEntity UpdateLabel(int id ,LabelModel model)
        {
            var entity = fundooContext.Labels?.Find(id);
            if (entity != null)
            {
                entity.LabelName = model.LabelName;
                try
                {
                    fundooContext.Labels?.Update(entity);
                    fundooContext.SaveChanges();
                    return entity;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }
            }
            else
            {
                throw new CustomizeException("Label Not Found");
            }
        }
    }
}
