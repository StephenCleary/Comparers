$ErrorActionPreference = "Stop"
pushd
cd artifacts\bin\Nito.Comparers\Debug\net45

Function Verify-OnlyOnePackage
{
  param ($name)

  $location = $env:USERPROFILE + '\.k\packages\' + $name
  If ((Get-ChildItem $location).Count -ne 1)
  {
    throw 'Invalid number of packages installed at ' + $location
  }
}

Verify-OnlyOnePackage 'OpenCover'
Verify-OnlyOnePackage 'coveralls.io'
Verify-OnlyOnePackage 'ReportGenerator'

# Execute OpenCover with a target of "k test"
$original_KRE_APPBASE = $env:KRE_APPBASE
$env:KRE_APPBASE = "../../../../../test/UnitTests"
iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\OpenCover'))[0].FullName + '\OpenCover.Console.exe' + ' -register:user -target:"k.cmd" -targetargs:"test" -output:coverage.xml -skipautoprops -returntargetcode -filter:"+[Nito*]*"')
$env:KRE_APPBASE = $original_KRE_APPBASE

# Either display or publish the results
If ($env:CI -eq 'True')
{
  iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\coveralls.io'))[0].FullName + '\tools\coveralls.net.exe' + ' --opencover coverage.xml --full-sources')
}
Else
{
  iex ((Get-ChildItem ($env:USERPROFILE + '\.k\packages\ReportGenerator'))[0].FullName + '\ReportGenerator.exe -reports:coverage.xml -targetdir:.')
  ./index.htm
}

popd