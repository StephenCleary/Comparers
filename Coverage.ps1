$testProjectLocations = @('test/UnitTests', 'test/Linq.UnitTests', 'test/Ix.UnitTests', 'test/Rx.UnitTests')
$outputLocation = 'testResults'
iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/StephenCleary/BuildTools/bd44aef57d6df1acb1cab45eebb983f68a218528/Coverage.ps1'))
