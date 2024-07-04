﻿using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
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
        private readonly ILogger<LabelRL> _logger;
        //private ProducerConfig _configuration;
        private readonly IConfiguration _config;
        private readonly KafkaProducer _kafkaProducer;
        public LabelRL(FundooContext fundooContext, ILogger<LabelRL> logger, IConfiguration config, KafkaProducer kafkaProducer)
        {
            this.fundooContext = fundooContext;
            _logger = logger;
            _logger.LogInformation("Nlog is integrated");
            //_configuration = configuration;
            _config = config;
            _kafkaProducer = kafkaProducer;
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
                    await fundooContext.SaveChangesAsync();
                    var entity = await fundooContext.Labels?.FirstOrDefaultAsync(x => x.LabelName == model.LabelName);
                    _logger.LogInformation("LabelController.AddLabel method called!!!");
                    int partition = entity.LabelId % 2 == 0 ? 0 : 1;
                    await _kafkaProducer.ProduceMessageAsync(model, partition);

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
