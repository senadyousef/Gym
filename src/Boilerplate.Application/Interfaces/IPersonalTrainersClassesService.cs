using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IPersonalTrainersClassesService
    {
        public Task<PaginatedList<GetPersonalTrainersClassesDto>> GetAllPersonalTrainersClassesWithPageSize(GetPersonalTrainersClassesFilter filter);
        public Task<AllList<GetPersonalTrainersClassesDto>> GetAllPersonalTrainersClasses(GetPersonalTrainersClassesFilter filter); 
        public Task<GetPersonalTrainersClassesDto> CreatePersonalTrainersClasses(CreatePersonalTrainersClassesDto PersonalTrainersClasses);
        public Task<GetPersonalTrainersClassesDto> GetPersonalTrainersClassesById(int id); 
        public Task<GetPersonalTrainersClassesDto> UpdatePersonalTrainersClasses(int id, UpdatePersonalTrainersClassesDto updatedPersonalTrainersClasses); 
        public Task<bool> DeletePersonalTrainersClasses(int id); 
    } 
    public class GetPersonalTrainersClassesDto
    {
        public int Id { get; set; }
        public int PersonalTrainer { get; set; } 
        public int Trainee { get; set; } 
        public DateTime Time { get; set; } 
    }
    public class CreatePersonalTrainersClassesDto
    {
        public int PersonalTrainer { get; set; }
        public int Trainee { get; set; }
        public DateTime Time { get; set; }
    } 
    public class UpdatePersonalTrainersClassesDto
    {
        public int Id { get; set; }
        public int PersonalTrainer { get; set; }
        public int Trainee { get; set; }
        public DateTime Time { get; set; }
    } 
    public class GetPersonalTrainersClassesFilter : PaginationInfoFilter
    {
        public int PersonalTrainer { get; set; }
        public int Trainee { get; set; }
        public DateTime Time { get; set; }
    } 
}
