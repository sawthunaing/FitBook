namespace Stn.FitBook.Domain.IServices
{
    public interface ICachesService
    {
        public Task<string> GetClassScheduleSlots(string key);
        public Task SetClassScheduleSlots(string key,string value);
        public Task RemoveClassScheduleSlots(string key);
        public Task UpdateClassScheduleSlots(string key,string value);

    }
}
