using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using System.Data;
using RivTech.CABoxTruck.WebService.DTO.Enum;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly AppDbContext _context;

        public SequenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public int GetNextSequence(SequenceType sequence)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            command.CommandText = $"select NEXT VALUE FOR {sequence}";
            var nextSequence = int.Parse(command.ExecuteScalar().ToString());
            return nextSequence;
        }

        public string GetNextPolicyNumber()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            command.CommandText = "EXEC GetNextPolicyNumber";
            var nextSequence = command.ExecuteScalar().ToString();
            return nextSequence;
        }
    }
}
