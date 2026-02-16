using BusinessEntity.Customer_Club;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interface.Customer_Club;
using DataAccessLayer.Interface;              // برای IUnitOfWork
using DataAccessLayer.Interface.Customer_Club;


namespace BusinessLogicLayer.Repository.Customer_Club
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CustomerDto>>> SearchCustomersAsync(
      string? firstName = null,
      string? lastName = null,
      int? customerLevelId = null,
      int? minPoints = null,
      int? maxPoints = null,
      string? barcode = null)
        {
            try
            {
                var customers = await _unitOfWork.Customers.FindAsync(
                    c =>
                        (string.IsNullOrEmpty(firstName) || c.FirstName.Contains(firstName)) &&
                        (string.IsNullOrEmpty(lastName) || c.LastName.Contains(lastName)) &&
                        (!customerLevelId.HasValue || c.CustomerLevelId == customerLevelId) &&
                        (!minPoints.HasValue || c.CurrentPoints >= minPoints) &&
                        (!maxPoints.HasValue || c.CurrentPoints <= maxPoints) &&
                        (string.IsNullOrEmpty(barcode) || c.Barcode == barcode),
                    default,
                    c => c.Wallet,
                    c => c.CustomerLevel,
                    c => c.People
                );

                var result = new List<CustomerDto>();
                foreach (var customer in customers)
                {
                    result.Add(await MapToDto(customer));
                }

                return Result<List<CustomerDto>>.Success(result);
            }
            catch (Exception ex)
            {
                
                return Result<List<CustomerDto>>.Failure("خطا در انجام جستجو");
            }
        }
        public async Task<Result<CustomerDto>> RegisterCustomerAsync(CustomerRegisterDto dto)
        {
            // اعتبارسنجی
            if (string.IsNullOrWhiteSpace(dto.Mobile))
                return Result<CustomerDto>.Failure("شماره موبایل الزامی است");

            var existing = await _unitOfWork.Customers.GetByMobileAsync(dto.Mobile);
            if (existing != null)
                return Result<CustomerDto>.Failure("این شماره موبایل قبلاً ثبت شده است");

            // تولید بارکد یکتا
            var barcodeResult = await GenerateCustomerBarcodeAsync();
            if (!barcodeResult.IsSuccess)
                return Result<CustomerDto>.Failure(barcodeResult.Message);

            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Mobile = dto.Mobile,
                Email = dto.Email,
                Barcode = barcodeResult.Data,
                RegisterDate = DateTime.UtcNow,
                IsActive = true,
                IsClubMember = dto.RegisterInClub,
                StoreId = dto.StoreId,
                PeopleId = dto.PeopleId, // اتصال به People در صورت وجود
                TotalPoints = 0,
                CurrentPoints = 0
            };

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            // اگر عضو باشگاه شد، کیف پول ایجاد کن
            if (dto.RegisterInClub)
            {
                var wallet = new Wallet
                {
                    CustomerId = customer.Id,
                    Balance = 0,
                    LastUpdate = DateTime.UtcNow
                };
                await _unitOfWork.Wallets.AddAsync(wallet);
                await _unitOfWork.SaveChangesAsync();
            }

            var resultDto = await MapToDto(customer);
            return Result<CustomerDto>.Success(resultDto, "مشتری با موفقیت ثبت شد");
        }

        public async Task<Result<string>> GenerateCustomerBarcodeAsync()
        {
            string barcode;
            bool exists;
            int attempts = 0;
            do
            {
                barcode = $"C{DateTime.UtcNow:yyMMddHHmmss}{new Random().Next(100, 999)}";
                exists = await _unitOfWork.Customers.IsBarcodeExistsAsync(barcode);
                attempts++;
            } while (exists && attempts < 10);

            if (exists)
                return Result<string>.Failure("خطا در تولید بارکد یکتا");

            return Result<string>.Success(barcode);
        }

        public async Task<Result<CustomerDto>> GetCustomerByBarcodeAsync(string barcode)
        {
            var customer = await _unitOfWork.Customers.GetByBarcodeAsync(barcode);
            if (customer == null)
                return Result<CustomerDto>.Failure("مشتری با این بارکد یافت نشد");

            var dto = await MapToDto(customer);
            return Result<CustomerDto>.Success(dto);
        }

        public async Task<Result<CustomerDto>> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetWithDetailsAsync(id);
            if (customer == null)
                return Result<CustomerDto>.Failure("مشتری یافت نشد");

            var dto = await MapToDto(customer);
            return Result<CustomerDto>.Success(dto);
        }

        public async Task<Result<CustomerDto>> GetCustomerByPeopleIdAsync(int peopleId)
        {
            var customers = await _unitOfWork.Customers.FindAsync(c => c.PeopleId == peopleId);
            var cust = customers.FirstOrDefault();
            if (cust == null)
                return Result<CustomerDto>.Failure("مشتری باشگاه برای این شخص یافت نشد");

            var dto = await MapToDto(cust);
            return Result<CustomerDto>.Success(dto);
        }

        public async Task<Result> UpdateCustomerPointsAsync(int customerId, int points, string description, int? invoiceId = null)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
                return Result.Failure("مشتری یافت نشد");

            var pointTransaction = new PointTransaction
            {
                CustomerId = customerId,
                Points = points,
                Type = points > 0 ? PointTransactionType.Earn : PointTransactionType.Redeem,
                TransactionDate = DateTime.UtcNow,
                Description = description,
                InvoiceId = invoiceId,
                ExpireDate = points > 0 ? DateTime.UtcNow.AddMonths(6) : null
            };

            await _unitOfWork.PointTransactions.AddAsync(pointTransaction);

            customer.CurrentPoints += points;
            if (points > 0)
                customer.TotalPoints += points;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();

            await UpgradeCustomerLevelAsync(customerId);
            return Result.Success();
        }

        public async Task<Result> UpgradeCustomerLevelAsync(int customerId)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null)
                return Result.Failure("مشتری یافت نشد");

            var suitableLevels = await _unitOfWork.CustomerLevels
                .FindAsync(cl => cl.IsActive &&
                                 cl.MinPoints <= customer.CurrentPoints &&
                                 (cl.MaxPoints == null || cl.MaxPoints >= customer.CurrentPoints) &&
                                 cl.StoreId == customer.StoreId);

            var newLevel = suitableLevels.OrderByDescending(cl => cl.MinPoints).FirstOrDefault();

            if (newLevel != null && customer.CustomerLevelId != newLevel.Id)
            {
                // بستن سطح قبلی
                var currentHistory = await _unitOfWork.CustomerLevelHistories
                    .FindAsync(clh => clh.CustomerId == customerId && clh.ToDate == null);
                foreach (var history in currentHistory)
                {
                    history.ToDate = DateTime.UtcNow;
                    _unitOfWork.CustomerLevelHistories.Update(history);
                }

                var newHistory = new CustomerLevelHistory
                {
                    CustomerId = customerId,
                    CustomerLevelId = newLevel.Id,
                    FromDate = DateTime.UtcNow,
                    ToDate = null
                };

                await _unitOfWork.CustomerLevelHistories.AddAsync(newHistory);
                customer.CustomerLevelId = newLevel.Id;
                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.SaveChangesAsync();
            }
            return Result.Success();
        }

        public async Task<Result<CustomerLevelDto?>> GetCustomerCurrentLevelAsync(int customerId)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer?.CustomerLevelId == null)
                return Result<CustomerLevelDto?>.Success(null, "مشتری دارای سطح نیست");

            var level = await _unitOfWork.CustomerLevels.GetByIdAsync(customer.CustomerLevelId.Value);
            if (level == null)
                return Result<CustomerLevelDto?>.Failure("سطح یافت نشد");

            return Result<CustomerLevelDto?>.Success(MapToLevelDto(level));
        }

        // ============ متدهای کمکی ============
        private async Task<CustomerDto> MapToDto(Customer customer)
        {
            var wallet = await _unitOfWork.Wallets.GetByCustomerIdAsync(customer.Id);
            var level = customer.CustomerLevelId != null
                ? await _unitOfWork.CustomerLevels.GetByIdAsync(customer.CustomerLevelId.Value)
                : null;
            var people = customer.PeopleId != null
                ? await _unitOfWork.People.GetByIdAsync(customer.PeopleId.Value)
                : null;

            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Mobile = customer.Mobile,
                Email = customer.Email,
                Barcode = customer.Barcode,
                RegisterDate = customer.RegisterDate,
                IsClubMember = customer.IsClubMember,
                TotalPoints = customer.TotalPoints,
                CurrentPoints = customer.CurrentPoints,
                TotalPurchaseAmount = customer.TotalPurchaseAmount,
                TotalPurchaseCount = customer.TotalPurchaseCount,
                LastPurchaseDate = customer.LastPurchaseDate,
                CustomerLevelId = customer.CustomerLevelId,
                CustomerLevelName = level?.Name,
                WalletBalance = wallet?.Balance ?? 0,
                PeopleId = customer.PeopleId,
                PeopleName = people != null ? $"{people.FirstName} {people.LastName}" : null
            };
        }

        private CustomerLevelDto MapToLevelDto(CustomerLevel level)
        {
            return new CustomerLevelDto
            {
                Id = level.Id,
                Name = level.Name,
                Description = level.Description,
                MinPoints = level.MinPoints,
                MaxPoints = level.MaxPoints,
                DiscountPercent = level.DiscountPercent
            };
        }
    }
}
