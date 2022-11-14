using Exercise9.LoggingServices;
using Exercise9.Models;
using Exercise9.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercise9.Controllers
{
    [Authorize]
    [Route("api/")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private MainDbContext _mainDbContext;
        private readonly ILoggerManager _logger;

        public DoctorController(MainDbContext mainDbContext, ILoggerManager loggerManager)
        {
            _mainDbContext = mainDbContext;
            _logger = loggerManager;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var context = new MainDbContext();

            var doctors = context.Doctors.Select(d => new DoctorsDto
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email
            });
            return Ok(doctors);
        }


        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(DoctorsDto doctor)
        {
            var context = new MainDbContext();
            Doctor newDoctor = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            };

            context.Doctors.Add(newDoctor);

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("doctors/{idDoctor}")]
        public async Task<IActionResult> UpdateDoctor(int idDoctor, DoctorsDto doctor)
        {
            var context = new MainDbContext();

            Doctor doctorToUpdate = context.Doctors
                .Where(d => d.IdDoctor == idDoctor).FirstOrDefault();

            if (doctorToUpdate == null)
            {
                return NotFound("Nie ma takiego lekarza w bazie");

            }

            doctorToUpdate.FirstName = doctor.FirstName;
            doctorToUpdate.LastName = doctor.LastName;
            doctorToUpdate.Email = doctor.Email;
            try
            {
                context.Doctors.Attach(doctorToUpdate);
                context.Entry(doctorToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpDelete("doctors/{idDoctor}")]
        public async Task<IActionResult> DeleteDoctor(int idDoctor)
        {
            var context = new MainDbContext();

            Doctor doctorToDelete = context.Doctors
                .Where(d => d.IdDoctor == idDoctor).FirstOrDefault();

            if (doctorToDelete == null)
            {
                return NotFound("Nie ma takiego lekarza w bazie");

            }

            try
            {
                context.Doctors.Attach(doctorToDelete);
                context.Doctors.Remove(doctorToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet("prescription/{idPrescription}")]
        public async Task<IActionResult> GetPrescription(int idPrescription)
        {
            var context = new MainDbContext();

            var prescription = context.Prescriptions.Where(p => p.IdPrescription == idPrescription)
                .Include(p => p.Prescription_Medicaments).ThenInclude(p => p.Medicament)
                .Select(p => new PrescriptionDto
                {
                    DoctorName = p.Doctor.FirstName + " " + p.Doctor.LastName,
                    PatientName = p.Patient.FirstName + " " + p.Patient.LastName,
                    Medicaments = (ICollection<MedicamentDto>)p.Prescription_Medicaments.Select(m => m.Medicament)
                    .Select(m => new MedicamentDto
                    {
                        Name = m.Name
                    })
                });

            if(prescription.ToList().Count() == 0)
            {
                return NotFound();
            }

            return Ok(prescription);
        }
    }
}