using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FeedbackService : IFeedbackService
    {
        IFeedbackRepo _feedbackRepo;
        public FeedbackService(IFeedbackRepo feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        public async Task AddFeedbackAsync(string text)
        {
            await _feedbackRepo.Insert(new Feedback() { Description = text });
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            await _feedbackRepo.Delete(new Feedback() { FeedbackId = feedbackId });
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbackAsync()
        {
            return await _feedbackRepo.GetMany();
        }

        public async Task<Feedback> GetFeedbackAsync(int feedbackId)
        {
            return await _feedbackRepo.GetByPKAsync(feedbackId);
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            await _feedbackRepo.Update(feedback);
        }
    }
}
