$ErrorActionPreference = "Stop"

# Build VS2013 solution
Write-Output "Building VS2013 Solution..."
$project = Get-Project
$build = $project.DTE.Solution.SolutionBuild
$oldConfiguration = $build.ActiveConfiguration
$build.SolutionConfigurations.Item("Release").Activate()
$build.Build($true)
$oldConfiguration.Activate()
Write-Output "... done building VS2013 Solution."

# Build VS2012 solution
Write-Output "Building VS2012 Solution..."
$buildVS2012 = "`"" + $env:VS110COMNTOOLS + "VsDevCmd.bat" + "`" && devenv VS2012\VS2012.sln /rebuild Release"
cmd /c $buildVS2012
Write-Output "... done building VS2012 Solution."

nuget pack -Symbols -Prop Configuration=Release -IncludeReferencedProjects "Comparers (NET4, Win8, SL5, WP8, WPA)\Comparers (NET4, Win8, SL5, WP8, WPA).csproj"
nuget pack -Symbols -Prop Configuration=Release -IncludeReferencedProjects "Comparers.Ix (NET45, Win8, WP8)\Comparers.Ix (NET45, Win8, WP8).csproj"
nuget pack -Symbols -Prop Configuration=Release -IncludeReferencedProjects "Comparers.Rx (NET4, Win8, SL5, WP8)\Comparers.Rx (NET4, Win8, SL5, WP8).csproj"
