$ErrorActionPreference = "Stop"

# Build solution
$project = get-project
$build = $project.DTE.Solution.SolutionBuild
$oldConfiguration = $build.ActiveConfiguration
$build.SolutionConfigurations.Item("Release").Activate()
$build.Build($true)
$oldConfiguration.Activate()

nuget pack -Symbols "Comparers\Comparers (NET4).csproj" -Prop Configuration=Release
nuget pack -Symbols "Comparers.Ix (NET45, Win8, WP8)\Comparers.Ix (NET45, Win8, WP8).csproj" -Prop Configuration=Release
nuget pack -Symbols "Comparers.Rx (NET4, Win8, SL5, WP8)\Comparers.Rx (NET4, Win8, SL5, WP8).csproj" -Prop Configuration=Release
