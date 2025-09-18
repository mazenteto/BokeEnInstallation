using BokeEnInstallationAPI.DBContext;
using BokeEnInstallationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BokeEnInstallationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BookingController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<BokeEnInstallationForm>>> GetBookings([FromQuery] BookingQueryParameters parameters)
        {
            var query = _context.Booking.AsQueryable();

            // Apply finished filter if provided
            if (parameters.IsFinished.HasValue)
            {
                query = query.Where(b => b.Fineished == parameters.IsFinished.Value);
            }

            // Get total count
            var totalItems = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            // Calculate pagination info
            var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);

            return new PaginatedResult<BokeEnInstallationForm>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalPages = totalPages,
                HasPreviousPage = parameters.PageNumber > 1,
                HasNextPage = parameters.PageNumber < totalPages
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BokeEnInstallationForm>> GetBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPost]
        public async Task<ActionResult<BokeEnInstallationForm>> CreateBooking(BokeEnInstallationForm dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            _context.Booking.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = dto.id }, dto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] Dictionary<string, bool> updates)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            bool hasChanges = false;

            if (updates.ContainsKey("fineished"))
            {
                booking.Fineished = updates["fineished"];
                hasChanges = true;
            }

            if (updates.ContainsKey("customerPay"))
            {
                booking.CustomerPay = updates["customerPay"];
                hasChanges = true;
            }

            if (hasChanges)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }

            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
