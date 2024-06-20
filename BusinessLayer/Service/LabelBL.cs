﻿using BusinessLayer.Interface;
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
        public async Task<LabelEntity> AddLabel(LabelModel model)
        {
            try
            {
                return  await _labelRL.AddLabel(model);
            }
            catch
            {
                throw;
            }
        }

        public async Task<LabelEntity> DeleteLabel(int id)
        {
            try
            {
                return await _labelRL.DeleteLabel(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LabelEntity>> GetAllLabels()
        {
            try
            {
                return await _labelRL.GetAllLabels();
            }
            catch
            {
                throw;
            }
        }

        public async Task<LabelEntity> UpdateLabel(int id ,LabelModel model)
        {
            try
            {
                return await _labelRL.UpdateLabel(id,model);
            }
            catch
            {
                throw;
            }
        }
    }
}
