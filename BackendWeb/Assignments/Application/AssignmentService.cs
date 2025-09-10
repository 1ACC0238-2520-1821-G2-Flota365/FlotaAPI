using BackendWeb.Assignments.Domain;

namespace Assignments.Application
{
    public class AssignmentService
    {
        private readonly IAssignmentRepository _repository;

        public AssignmentService(IAssignmentRepository repository) => _repository = repository;

        public async Task<Assignment> Create(Guid vehicleId, Guid driverId, string route)
        {
            var assignment = new Assignment(vehicleId, driverId, route);
            await _repository.AddAsync(assignment);
            return assignment;
        }

        public async Task<List<Assignment>> GetAll() => await _repository.GetAllAsync();

        public async Task Start(Guid id)
        {
            var assignment = await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Assignment not found");
            assignment.Start();
            await _repository.UpdateAsync(assignment);
        }

        public async Task Complete(Guid id)
        {
            var assignment = await _repository.GetByIdAsync(id)
                ?? throw new InvalidOperationException("Assignment not found");
            assignment.Complete();
            await _repository.UpdateAsync(assignment);
        }
    }
}
