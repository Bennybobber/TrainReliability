using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;

namespace RailWatch.ArchitectureTests;

public class DependencyRulesTests
{
    [Fact]
    public void Domain_Should_Not_Reference_Infrastructure()
    {
        var refs = LoadProjectReferences("src/RailWatch.Domain/RailWatch.Domain.csproj");
        Assert.DoesNotContain(refs, r => r.EndsWith("RailWatch.Infrastructure/RailWatch.Infrastructure.csproj", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Infrastructure_Should_Reference_Domain()
    {
        var refs = LoadProjectReferences("src/RailWatch.Infrastructure/RailWatch.Infrastructure.csproj");
        Assert.Contains(refs, r => r.EndsWith("RailWatch.Domain/RailWatch.Domain.csproj", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Api_Should_Reference_Infrastructure_And_Not_Domain_Directly()
    {
        var refs = LoadProjectReferences("src/RailWatch.Api/RailWatch.Api.csproj");
        Assert.Contains(refs, r => r.EndsWith("RailWatch.Infrastructure/RailWatch.Infrastructure.csproj", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(refs, r => r.EndsWith("RailWatch.Domain/RailWatch.Domain.csproj", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Worker_Should_Reference_Infrastructure_And_Not_Domain_Directly()
    {
        var refs = LoadProjectReferences("src/RailWatch.Worker/RailWatch.Worker.csproj");
        Assert.Contains(refs, r => r.EndsWith("RailWatch.Infrastructure/RailWatch.Infrastructure.csproj", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(refs, r => r.EndsWith("RailWatch.Domain/RailWatch.Domain.csproj", StringComparison.OrdinalIgnoreCase));
    }

    private static IReadOnlyList<string> LoadProjectReferences(string relativeCsprojPath)
    {
        var root = FindRepoRoot();
        var csprojPath = Path.GetFullPath(Path.Combine(root, relativeCsprojPath));

        var doc = XDocument.Load(csprojPath);

        // Normalize path separators to forward slashes so EndsWith checks are stable
        var refs = doc.Descendants()
            .Where(e => e.Name.LocalName == "ProjectReference")
            .Select(e => e.Attribute("Include")?.Value)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .Select(v => v!.Replace('\\', '/'))
            .ToList();

        return refs;
    }

    private static string FindRepoRoot()
    {
        // Start from the test output folder and walk upward to find RailWatch.sln or .git
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "RailWatch.sln")) ||
                Directory.Exists(Path.Combine(dir.FullName, ".git")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate repo root (RailWatch.sln or .git).");
    }
}
