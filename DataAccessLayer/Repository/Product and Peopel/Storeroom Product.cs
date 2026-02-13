using BusinessEntity.DTO.People;
using BusinessEntity.Product;
using DataAccessLayer.Interface.Product;
using DataAccessLayer.Repository.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Product
{
    //    public async Task<List<PeopleComboDto>> GetPeopleForComboAsync()
    //    {
    //        return await _context.People
    //            .Where(p => !p.IsDelete)
    //            .OrderBy(p => p.FirstName)
    //            .ThenBy(p => p.LastName)
    //            .Select(p => new PeopleComboDto
    //            {
    //                Id = p.Id,
    //                FullName = p.FirstName + " " + p.LastName
    //            })
    //            .ToListAsync();
    //    }
    public class StoreroomProductRepository : IStoreroomProductRepository
    {
        private readonly Database _context;
        private readonly ILogger<StoreroomProductRepository> _logger;

        public StoreroomProductRepository(
            Database context,
            ILogger<StoreroomProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ***** GetAll *****
        public async Task<IEnumerable<Storeroom_Product>> GetAll()
        {
            return await _context.Storeroom_Product
                .Include(s => s.Section_Product)
                .Include(s => s.People)
                .Where(s => !s.IsDelete)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        // ***** GetById *****
        public async Task<Storeroom_Product?> GetById(int id)
        {
            return await _context.Storeroom_Product
                .Include(s => s.Section_Product)
                .Include(s => s.People)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);
        }

        // ***** Search *****
        public async Task<List<Storeroom_Product>> Search(
            string? name = null,
            int? sectionProductId = null,
            int? peopleId = null)
        {
            var query = _context.Storeroom_Product
                .Include(s => s.Section_Product)
                .Include(s => s.People)
                .Where(s => !s.IsDelete)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(s => s.Name.Contains(name));

            if (sectionProductId.HasValue && sectionProductId > 0)
                query = query.Where(s => s.SectionProductId == sectionProductId.Value);

            if (peopleId.HasValue && peopleId > 0)
                query = query.Where(s => s.PeopleId == peopleId.Value);

            return await query.OrderBy(s => s.Name).ToListAsync();
        }

        // ***** Create *****
        public async Task<Result> Create(Storeroom_Product storeroom)
        {
            try
            {
                // بررسی تکراری بودن نام
                bool duplicateName = await _context.Storeroom_Product
                    .AnyAsync(s => s.Name == storeroom.Name && !s.IsDelete);

                if (duplicateName)
                    return Result.Failure("نام انبار تکراری است.");

                // بررسی وجود بخش محصول
                bool sectionExists = await _context.Section_Product
                    .AnyAsync(s => s.Id == storeroom.SectionProductId && !s.IsDelete);

                if (!sectionExists)
                    return Result.Failure("بخش محصول انتخاب شده وجود ندارد.");

                // بررسی وجود شخص
                bool peopleExists = await _context.People
                    .AnyAsync(p => p.Id == storeroom.PeopleId && !p.IsDelete);

                if (!peopleExists)
                    return Result.Failure("شخص انتخاب شده وجود ندارد.");

                storeroom.IsDelete = false;

                await _context.Storeroom_Product.AddAsync(storeroom);
                await _context.SaveChangesAsync();

                return Result.Success("انبار با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ایجاد انبار: {@Storeroom}", storeroom);
                return Result.Failure($"خطا در ایجاد انبار: {ex.Message}");
            }
        }

        // ***** Update *****
        public async Task<Result> Update(Storeroom_Product storeroom)
        {
            try
            {
                // بررسی وجود انبار
                var existingStoreroom = await _context.Storeroom_Product
                    .FirstOrDefaultAsync(s => s.Id == storeroom.Id && !s.IsDelete);

                if (existingStoreroom == null)
                    return Result.Failure("انبار یافت نشد.");

                // بررسی تکراری بودن نام (به جز خودش)
                bool duplicateName = await _context.Storeroom_Product
                    .AnyAsync(s => s.Id != storeroom.Id && s.Name == storeroom.Name && !s.IsDelete);

                if (duplicateName)
                    return Result.Failure("نام انبار تکراری است.");

                // بررسی وجود بخش محصول
                bool sectionExists = await _context.Section_Product
                    .AnyAsync(s => s.Id == storeroom.SectionProductId && !s.IsDelete);

                if (!sectionExists)
                    return Result.Failure("بخش محصول انتخاب شده وجود ندارد.");

                // بررسی وجود شخص
                bool peopleExists = await _context.People
                    .AnyAsync(p => p.Id == storeroom.PeopleId && !p.IsDelete);

                if (!peopleExists)
                    return Result.Failure("شخص انتخاب شده وجود ندارد.");

                // به‌روزرسانی فیلدها
                existingStoreroom.Name = storeroom.Name;
                existingStoreroom.SectionProductId = storeroom.SectionProductId;
                existingStoreroom.PeopleId = storeroom.PeopleId;
                existingStoreroom.Description = storeroom.Description;
                existingStoreroom.Address = storeroom.Address;
                existingStoreroom.NegativeBalancePolicy = storeroom.NegativeBalancePolicy;

                _context.Storeroom_Product.Update(existingStoreroom);
                await _context.SaveChangesAsync();

                return Result.Success("انبار با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در به‌روزرسانی انبار: {@Storeroom}", storeroom);
                return Result.Failure($"خطا در به‌روزرسانی انبار: {ex.Message}");
            }
        }

        // ***** Delete *****
        public async Task<Result> Delete(int id)
        {
            try
            {
                var storeroom = await _context.Storeroom_Product
                    .Include(s => s.Products)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDelete);

                if (storeroom == null)
                    return Result.Failure("انبار یافت نشد.");

                // بررسی استفاده در محصولات
                if (storeroom.Products.Any(p => !p.IsDelete))
                    return Result.Failure("امکان حذف انبار وجود ندارد، زیرا محصولاتی به آن مرتبط هستند.");

                // Soft Delete
                storeroom.IsDelete = true;
                await _context.SaveChangesAsync();

                return Result.Success("انبار با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در حذف انبار با شناسه: {Id}", id);
                return Result.Failure($"خطا در حذف انبار: {ex.Message}");
            }
        }
    }

}
