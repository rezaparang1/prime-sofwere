using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Interface.Producr;
using DataAccessLayer.Interface.Product;
using Microsoft.Extensions.Logging;
using BusinessEntity.Product;
using DataAccessLayer;

namespace BusinessLogicLayer.Repository.Product
{
    public class StoreroomProductService : IStoreroomProductService
    {
        private readonly IStoreroomProductRepository _storeroomRepository;
        private readonly ILogService _logService;
        private readonly Database _context;
        private readonly ILogger<StoreroomProductService> _logger;

        public StoreroomProductService(
            IStoreroomProductRepository storeroomRepository,
            ILogService logService,
            Database context,
            ILogger<StoreroomProductService> logger)
        {
            _storeroomRepository = storeroomRepository;
            _logService = logService;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Storeroom_Product>> GetAll()
        {
            return await _storeroomRepository.GetAll();
        }

        public async Task<Storeroom_Product?> GetById(int id)
        {
            return await _storeroomRepository.GetById(id);
        }

        public async Task<List<Storeroom_Product>> Search(
            string? name = null,
            int? sectionProductId = null,
            int? peopleId = null)
        {
            return await _storeroomRepository.Search(name, sectionProductId, peopleId);
        }

        public async Task<Result> Create(Storeroom_Product storeroom, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(storeroom.Name))
                    return Result.Failure("نام انبار الزامی است.");

                if (storeroom.SectionProductId <= 0)
                    return Result.Failure("بخش محصول باید انتخاب شود.");

                if (storeroom.PeopleId <= 0)
                    return Result.Failure("مسئول انبار باید انتخاب شود.");

                // ایجاد انبار
                var result = await _storeroomRepository.Create(storeroom);
                if (!result.IsSuccess)
                    return result;

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"ایجاد انبار جدید: {storeroom.Name} (شناسه: {storeroom.Id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("انبار با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در ایجاد انبار: {@Storeroom}", storeroom);
                return Result.Failure($"خطا در ایجاد انبار: {ex.Message}");
            }
        }

        public async Task<Result> Update(Storeroom_Product storeroom, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // اعتبارسنجی
                if (string.IsNullOrWhiteSpace(storeroom.Name))
                    return Result.Failure("نام انبار الزامی است.");

                // به‌روزرسانی انبار
                var result = await _storeroomRepository.Update(storeroom);
                if (!result.IsSuccess)
                    return result;

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"به‌روزرسانی انبار: {storeroom.Name} (شناسه: {storeroom.Id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("انبار با موفقیت به‌روزرسانی شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در به‌روزرسانی انبار: {@Storeroom}", storeroom);
                return Result.Failure($"خطا در به‌روزرسانی انبار: {ex.Message}");
            }
        }

        public async Task<Result> Delete(int id, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // دریافت اطلاعات انبار برای لاگ
                var storeroom = await _storeroomRepository.GetById(id);
                if (storeroom == null)
                    return Result.Failure("انبار یافت نشد.");

                // حذف انبار
                var result = await _storeroomRepository.Delete(id);
                if (!result.IsSuccess)
                    return result;

                // ثبت لاگ
                await _logService.CreateLogAsync(
                    $"حذف انبار: {storeroom.Name} (شناسه: {id})",
                    userId);

                await transaction.CommitAsync();
                return Result.Success("انبار با موفقیت حذف شد.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "خطا در حذف انبار با شناسه: {Id}", id);
                return Result.Failure($"خطا در حذف انبار: {ex.Message}");
            }
        }
    }
}
