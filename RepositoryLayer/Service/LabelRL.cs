using Microsoft.EntityFrameworkCore;
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
        public async Task<LabelEntity> AddLabel(LabelModel model)
        {
            LabelEntity labelEntity = new LabelEntity();
            var result = await fundooContext.Labels?.FirstOrDefaultAsync(x => x.LabelName == model.LabelName);

            if (result == null)
            {
                labelEntity.LabelName = model.LabelName;
                try
                {
                    fundooContext.Labels?.Add(labelEntity);
                    await fundooContext.SaveChangesAsync(); // Await the SaveChangesAsync call
                    return labelEntity;
                }
                catch (Exception ex)
                {
                    throw new CustomizeException(ex.Message);
                }
            }
            else
            {
                throw new CustomizeException("Label Already Exists");
            }
        }

        public async Task<LabelEntity> DeleteLabel(int id)
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

        public async Task<List<LabelEntity>> GetAllLabels()
        {
            var result = await fundooContext.Labels?.ToListAsync();
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new CustomizeException("No Label Found");
            }
        }

        public async Task<LabelEntity> UpdateLabel(int id ,LabelModel model)
        {
            var entity =  await fundooContext.Labels.FindAsync(id);
            if (entity != null)
            {
                entity.LabelName = model.LabelName;
                try
                {
                    fundooContext.Labels.Update(entity);
                    await fundooContext.SaveChangesAsync();
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
