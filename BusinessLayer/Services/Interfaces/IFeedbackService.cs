using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<Feedback> GetFeedbackAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetAllFeedbackAsync();
        Task AddFeedbackAsync(string text);        
        Task DeleteFeedbackAsync(int feedbackId);
        Task UpdateFeedbackAsync(Feedback feedback);
    }
}
