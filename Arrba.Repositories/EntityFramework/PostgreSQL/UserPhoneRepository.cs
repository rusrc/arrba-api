using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrba.Domain;
using Arrba.Domain.Models;

namespace Arrba.Repositories.EntityFramework.PostgreSQL
{
    public class UserPhoneRepository : Repository<UserPhone>, IUserPhoneRepository
    {
        public UserPhoneRepository(DbArrbaContext context) : base(context)
        {
        }


        public async Task<UserPhone> GetByVehicleIdAsync(long adVehicleId)
        {
            var vehicle = await _context.AdVehicles.FindAsync(adVehicleId);
            if (vehicle != null)
            {
                await _context.Entry(vehicle).Reference(v => v.User).LoadAsync();
                await _context.Entry(vehicle.User).Collection(v => v.UserPhones).LoadAsync();

                UserPhone phone = null;
                var phones = vehicle.User.UserPhones.Where(uPh => uPh.Status == ActiveStatus.active).ToList();

                if ((phone = phones.FirstOrDefault(uPh => uPh.PriorityStatus == PriorityStatus.main)) != null)
                {
                    return phone;
                }

                if ((phone = phones.FirstOrDefault(uPh => uPh.PriorityStatus == PriorityStatus.@default)) != null)
                {
                    return phone;
                }

                await _context.Entry(vehicle).Reference(v => v.Dealership).LoadAsync();
                var phoneNumber = vehicle?.Dealership?.PhoneNumber;

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    return new UserPhone { Number = phoneNumber };
                }

                return vehicle.User.UserPhones.FirstOrDefault();
            }

            return null;
        }

        public async Task<IEnumerable<UserPhone>> GetAllAsync(long adVehicleId)
        {
            var vehicle = await _context.AdVehicles.FindAsync(adVehicleId);
            if (vehicle == null)
            {
                return null;
            }

            await _context.Entry(vehicle).Reference(v => v.User).LoadAsync();
            await _context.Entry(vehicle.User).Collection(v => v.UserPhones).LoadAsync();

            var phones = vehicle.User
                .UserPhones
                .Where(uPh => uPh.Status == ActiveStatus.active)
                .ToList();

            return phones;
        }
    }
}
