namespace Clinic.APIS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PrescriptionController : ApiBaseController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public PrescriptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePrescription(CreatePrescriptionDTO model)
        {
            var prescription = _mapper.Map<Prescription>(model);

            await _unitOfWork.Repository<Prescription, int>().AddAsync(prescription);
            await _unitOfWork.CompleteAsync();

            return Result.Success(prescription).ToActionResult();
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, UpdatePrescriptionDTO model)
        {
            var existingPrescription = await _unitOfWork.Repository<Prescription, int>().GetByIdAsync(id);
            if (existingPrescription == null)
                return Result.Failure(new Error(404, $"Prescription with ID {id} not found.")).ToActionResult();

            _mapper.Map(model, existingPrescription);
            _unitOfWork.Repository<Prescription, int>().Update(existingPrescription);
            await _unitOfWork.CompleteAsync();

            return Result.Success(existingPrescription).ToActionResult();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _unitOfWork.Repository<Prescription, int>().GetByIdAsync(id);
            if (prescription == null)
                return Result.Failure(new Error(404, $"Prescription with ID {id} not found.")).ToActionResult();
            _unitOfWork.Repository<Prescription, int>().Delete(prescription);
            await _unitOfWork.CompleteAsync();

            return Result.Success($"Prescription with ID {id} deleted successfully.").ToActionResult();
        }


        [HttpGet("GetPdf/{id}")]
        public async Task<IActionResult> GetPrescriptionPdf(int id)
        {
            var spec = new PrescriptionWithPatientAndVetSpecification(id);
            var prescription = await _unitOfWork.Repository<Prescription, int>().GetEntityWithSpecAsync(spec);

            if (prescription == null)
                return Result.Failure(new Error(404, $"Prescription with ID {id} not found.")).ToActionResult();
            var pdfBytes = PrescriptionPdfGenerator.GeneratePrescriptionPdf(prescription);
            return File(pdfBytes, "application/pdf", $"Prescription_{id}.pdf");
        }


        #endregion
    }
}
