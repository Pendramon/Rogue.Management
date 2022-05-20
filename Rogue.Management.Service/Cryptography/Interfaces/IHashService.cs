namespace Rogue.Management.Service.Cryptography.Interfaces;

public interface IHashService
{
    Task<string> ComputeHashAsync(string text);

    Task<CompareResult> CompareHashAsync(string text, string correctTextHash);
}