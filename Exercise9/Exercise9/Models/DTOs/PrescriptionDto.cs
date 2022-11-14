namespace Exercise9.Models.DTOs
{
    public class PrescriptionDto
    {
        public PrescriptionDto()
        {
            Medicaments = new HashSet<MedicamentDto>();
        }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public ICollection<MedicamentDto> Medicaments { get; set; }
    }

    public class DoctorDataDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PatientDataDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class MedicamentDto
    {
        public string Name { get; set; }
    }

}
