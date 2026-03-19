using Bacon.Build;

namespace Bacon.Tools;

[EnumParameter]
public enum DotNetWorkloadConfigUpdateMode
{
    [EnumName("manifests")] Manifests,
    [EnumName("workload-set")] WorkloadSet
}