using BusinessEntity;
using BusinessEntity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Repository.Settings
{
    public class ReminderRepository : Interface.Settings.IReminderRepository
    {
        private readonly Database _context;
        private readonly ILogger<ReminderRepository> _logger;

        public ReminderRepository(Database context, ILogger<ReminderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //*****SEARCH*****
        public async Task<List<Reminder>> Search(int? UserId=null, DateTime? Date = null, string? Description = null)
        {
            _logger.LogInformation("Searching for Aysnc: {UserId}{Date}{Description}", UserId,Description,Date);
            var query = _context.Reminder.AsQueryable();
            if (UserId.HasValue)
            {
                query = query.Where(r => r.UserId == UserId.Value);
            }
            if (Date.HasValue)
            {
                query = query.Where(r => r.Date == Date.Value.Date);
            }
            if (!string.IsNullOrEmpty(Description))
            {
                query = query.Where(r => r.Description.Contains(Description));
            }
            var result = await query.ToListAsync();

            _logger.LogInformation("{Count} results found for search name: {UserId}{Date}{Description}", result.Count, UserId,Description,Date);
            return result;
        }
        public async Task<List<Reminder>> SearchByUserId(int? UserId = null)
        {
            _logger.LogInformation("Searching for UserId: {UserId}", UserId);
            var query = _context.Reminder.AsQueryable();
            if (UserId.HasValue)
            {
                query = query.Where(r => r.UserId == UserId.Value);
            }
            var result = await query.ToListAsync();

            _logger.LogInformation("{Count} results found for search name: {UserId}", result.Count, UserId);
            return result;
        }
        //******READ*******
        public async Task<IEnumerable<Reminder>> GetAll()
        {
            _logger.LogInformation("All Reminder have started to be received from the database.");

            var result = await _context.Reminder.ToListAsync();

            _logger.LogInformation("{Count} records received.", result.Count);
            return result;
        }
        public async Task<Reminder?> GetById(int id)
        {
            _logger.LogInformation("Request to receive Reminder  with ID: {Id}", id);

            var entity = await _context.Reminder.FindAsync(id);

            if (entity == null)
                _logger.LogWarning("Reminder  with ID: {Id} not found", id);
            else
                _logger.LogInformation("Reminder name with ID: {Id} successfully retrieved", id);

            return entity;
        }
        //******CRUD*****
        public async Task<string> Create(Reminder Reminder)
        {
            try
            {
                _logger.LogInformation("Adding new Reminder: {@Reminder}", Reminder);
                if (Reminder == null)
                {
                    _logger.LogWarning("Null Reminder submitted.");
                    throw new ArgumentNullException(nameof(Reminder));
                }

                await _context.Reminder.AddAsync(Reminder);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Reminder added successfully. ID: {Id}", Reminder.Id);
                    return "عملیات با موفقیت انجام شد.";
                }
                _logger.LogWarning("No changes saved when adding Reminder: {@Reminder}", Reminder);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding Reminder: {@Reminder}", Reminder);
                return "خطایی در ذخیره اطلاعات رخ داد. لطفاً داده‌ها را بررسی کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding Reminder: {@Reminder}", Reminder);
                return "خطای غیرمنتظره رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
        public async Task<string> Update(Reminder Reminder)
        {
            try
            {
                _logger.LogInformation("Request update for Reminder: {@Reminder}", Reminder);

                if (Reminder == null)
                {
                    _logger.LogWarning("Null Reminder submitted.");
                    throw new ArgumentNullException(nameof(Reminder));
                }

                var existing = await _context.Reminder.FindAsync(Reminder.Id);

                if (existing == null)
                {
                    _logger.LogWarning("Reminder with ID: {Id} not found.", Reminder.Id);
                    return "شناسه وارد شده با شناسه ذخیره در سیستم مطابقت ندارد.";
                }

                _context.Entry(existing).CurrentValues.SetValues(Reminder);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Reminder with ID: {Id} successfully updated", Reminder.Id);
                    return "عملیات با موفقیت انجام شد.";
                }

                _logger.LogWarning("No changes detected for Reminder with ID: {Id}", Reminder.Id);
                return "هیچ تغییری اعمال نشد.";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error updating Reminder with ID: {Id}", Reminder?.Id);
                return "این رکورد توسط کاربر دیگری تغییر یافته است. لطفاً صفحه را رفرش کنید و مجدد تلاش کنید.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating Reminder with ID: {Id}", Reminder?.Id);
                return "خطایی در ذخیره تغییرات رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating Reminder");
                return "خطای غیرمنتظره رخ داد. لطفاً مجدداً تلاش کنید.";
            }
        }
        public async Task<string> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Request to delete Reminder with ID: {Id}", id);

                var entity = await _context.Reminder.FindAsync(id);

                if (entity == null)
                {
                    _logger.LogWarning("Reminder with ID: {Id} not found for deletion.", id);
                    return "درخواست مورد نظر شما یافت نشد .";
                }

                _context.Reminder.Remove(entity);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation("Reminder with ID: {Id} was successfully deleted.", id);
                    return "عملیات با موفقیت انجام شد .";
                }

                _logger.LogWarning("Failed to delete Reminder with ID: {Id}", id);
                return "عملیات حذف انجام نشد. لطفاً مجدداً تلاش کنید.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Reminder with ID: {Id}", id);
                return "خطایی در عملیات مورد نظر رخ داد. لطفاً با پشتیبانی تماس بگیرید.";
            }
        }
    }
}
