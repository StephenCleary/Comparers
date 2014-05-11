$ErrorActionPreference = "Stop"

# Build solution
$project = get-project
$build = $project.DTE.Solution.SolutionBuild
$oldConfiguration = $build.ActiveConfiguration
$build.SolutionConfigurations.Item("Release").Activate()
$build.Build($true)
$oldConfiguration.Activate()

nuget pack -Symbols "Comparers (NET4, Win8, SL5, WP8, WPA)\Comparers (NET4, Win8, SL5, WP8, WPA).csproj" -Prop Configuration=Release -IncludeReferencedProjects
nuget pack -Symbols "Comparers.Ix (NET45, Win8, WP8)\Comparers.Ix (NET45, Win8, WP8).csproj" -Prop Configuration=Release -IncludeReferencedProjects
nuget pack -Symbols "Comparers.Rx (NET4, Win8, SL5, WP8)\Comparers.Rx (NET4, Win8, SL5, WP8).csproj" -Prop Configuration=Release -IncludeReferencedProjects
